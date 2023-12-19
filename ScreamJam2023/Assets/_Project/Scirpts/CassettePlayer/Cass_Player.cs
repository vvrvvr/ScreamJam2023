using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Cass_Player : MonoBehaviour
{
    public Animator activeAnimator;
    public int activeTape = 0;
    [SerializeField] TextMeshProUGUI activeTapeName;
    public List<int> tapes_Owned = new List<int> { 0 };
    [SerializeField] List<Animator> tapeAnimators;
    public ToggleGroup functionToggleGroup;

    [SerializeField] Canvas CassPlayer_UI;

    private UnityEngine.UI.Toggle playToggle;
    private Toggle stopToggle;
    private Toggle pauseToggle;
    private Toggle rewindToggle;
    private Toggle nextToggle;

    bool switchedTape = false;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Cass_Player_Input.Next.performed += ctx => NextTape();
        controls.Cass_Player_Input.Play.performed += ctx => Play();
        controls.Cass_Player_Input.Stop.performed += ctx => Stop();
        controls.Cass_Player_Input.Pause.performed += ctx => Pause();
        controls.Cass_Player_Input.Rewind.performed += ctx => RewindTape();

        // Initialize toggles
        playToggle = FindToggleInChildren("PlayToggle");
        stopToggle = FindToggleInChildren("StopToggle");
        pauseToggle = FindToggleInChildren("PauseToggle");
        rewindToggle = FindToggleInChildren("RewindToggle");
        nextToggle = FindToggleInChildren("NextToggle");
    }

    private void OnEnable()
    {
        controls.Cass_Player_Input.Enable();
    }

    private void OnDisable()
    {
        controls.Cass_Player_Input.Disable();
    }

    private void Start()
    {
        // Ensure activeTape is within bounds
        activeTape = Mathf.Clamp(activeTape, 0, tapes_Owned.Count - 1);
        // Initialize the activeAnimator with the first tape (if available)
        if (tapeAnimators.Count > 0)
            activeAnimator = tapeAnimators[tapes_Owned[activeTape]];
    }

    public void AddTape(int num)
    {
        tapes_Owned.Add(num);

        if (activeTape == 0)
        {
            AssignActiveTape(num);
            Debug.Log("Added tape num: " + num);
        }
    }

    public void AssignActiveTape(int num)
    {
        activeTape = Mathf.Clamp(num, 0, tapes_Owned.Count - 1);
        // Change the active animator to the assigned tape
        activeAnimator = tapeAnimators[tapes_Owned[activeTape]];
    }

    public void NextTape()
    {
        if (nextToggle != null)
        {
            nextToggle.isOn = true;
            StartCoroutine(TurnOffToggleAfterDelay(nextToggle, 1f));
        }
        else
        {
            Debug.LogError("NextToggle not found.");
        }

        if (activeAnimator == null)
            return;
        if (tapes_Owned.Count <= 1)
        {
            Debug.LogWarning("No additional tapes available.");
            return;
        }

        // Change the active animator to the next tape
        int indexOfActiveTape = tapes_Owned.IndexOf(activeTape);
        int nextTapeIndex = (indexOfActiveTape + 1) % tapes_Owned.Count;

        if (nextTapeIndex == indexOfActiveTape)
        {
            Debug.LogWarning("No different next tape available.");
            return;
        }

        activeTape = nextTapeIndex;
        activeAnimator = tapeAnimators[tapes_Owned[activeTape]];
        activeTapeName.text = activeAnimator.gameObject.name;
        switchedTape = true;
        Debug.Log("Switched tapes");

        // Set the "NextToggle" and turn it off after 2 seconds
    }

    public void Play()
    {
        if (activeAnimator == null)
            return;

        playToggle.isOn = true;

        // Check if the animator is paused or stopped
        /* if (activeAnimator.GetCurrentAnimatorStateInfo(0).IsName("RootAnim"))
        {
            // Check if the animation has reached its full length
            if (activeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                // Animation is complete, restart from the beginning
                activeAnimator.Play("RootAnim", 0, 0f);
            }
            else
            {
                // Resume playing from the current position
                activeAnimator.Play("RootAnim", 0, activeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
        else
        {
            // Play the animation from the start
            activeAnimator.Play("RootAnim", 0, 0f);
        }
        */

        if (activeAnimator.GetCurrentAnimatorStateInfo(0).IsName("RootAnim"))
        {
            // Check if the animation has reached its full length
            if (activeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                // Animation is complete, restart from the beginning
                activeAnimator.Play("RootAnim", 0, 0f);
            }
            else
            {
                // Resume playing from the current position
                activeAnimator.Play("RootAnim", 0, activeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
        else if (activeAnimator.GetCurrentAnimatorStateInfo(0).IsName("RootAnim-1"))
        {
            // Check if the animation has reached its full length
            if (activeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                // Animation is complete, restart from the beginning
                activeAnimator.Play("RootAnim", 0, 0f);
            }
            else
            {
                // Resume playing from the current position
                activeAnimator.Play("RootAnim", 0, activeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
        else
        {
            // Play the animation from the start
            activeAnimator.Play("RootAnim", 0, 0f);
        }
        // Set the playback speed to 1 right away
        activeAnimator.speed = 1f;

        // UpdateFunctionToggle();
    }

    private IEnumerator SetPlaybackSpeedAfterDelay(float speed, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Set the playback speed back to normal
        activeAnimator.SetFloat("Speed", speed);
    }


    public void Stop()
    {
        if (activeAnimator == null)
            return;

        stopToggle.isOn = true;
        
        // Stop the animation without resetting it
        activeAnimator.Play("RootAnim", 0, 0f); // Set current time to 0
        activeAnimator.StopPlayback();
        
        StartCoroutine(TurnOffToggleAfterDelay(stopToggle, 1f));
    }

    public void Pause()
    {
        if (activeAnimator == null)
            return;

        pauseToggle.isOn = true;
        // Pause the animation
        activeAnimator.speed = 0f; // Set speed to 0 to pause
    }

    public void RewindTape()
    {
        if (activeAnimator == null)
            return;

        rewindToggle.isOn = true;

        // Get the array of currently playing clips
        var currentClipInfo = activeAnimator.GetCurrentAnimatorClipInfo(0);
        
        Debug.Log("current clip" + currentClipInfo.Length );

        if (activeAnimator.GetCurrentAnimatorStateInfo(0).IsName("RootAnim-1"))
        {
            // Check if the animation has reached its full length
            if (activeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                // Animation is complete, restart from the beginning
                activeAnimator.Play("RootAnim-1", 0, 0f);
            }
            else
            {
                // Resume playing from the current position
                activeAnimator.Play("RootAnim-1", 0, activeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
        else if (activeAnimator.GetCurrentAnimatorStateInfo(0).IsName("RootAnim"))
        {
            // Check if the animation has reached its full length
            if (activeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                // Animation is complete, restart from the beginning
                activeAnimator.Play("RootAnim-1", 0, 0f);
            }
            else
            {
                // Resume playing from the current position
                activeAnimator.Play("RootAnim-1", 0, activeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
        else
        {
            // Play the animation from the start
            activeAnimator.Play("RootAnim-1", 0, 0f);
        }
        
        activeAnimator.speed = 1f;
    }




    private IEnumerator TurnOffToggleAfterDelay(Toggle toggle, float delay)
    {
        switchedTape = false;
        yield return new WaitForSeconds(delay);

        // Check if the animator is not playing any animation on the root layer
        toggle.isOn = false;
    }

    private Toggle FindToggleInChildren(string toggleName)
    {
        Toggle[] toggles = GetComponentsInChildren<Toggle>(true); // Include inactive toggles
        return toggles.FirstOrDefault(toggle => toggle.name == toggleName);
    }
}
