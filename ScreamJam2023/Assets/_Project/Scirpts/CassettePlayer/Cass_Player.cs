using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Cass_Player : MonoBehaviour
{
    public PlayableDirector activePlayable;
    public int activeTape = 0;
    [SerializeField] TextMeshProUGUI activeTapeName;
    public List<int> tapes_Owned = new List<int> { 0 };
    [SerializeField] List<PlayableDirector> tapeClips;
    public ToggleGroup functionToggleGroup; // Reference to the ToggleGroup for play, stop, pause, rewind

    [SerializeField] Canvas CassPlayer_UI;

    private Toggle playToggle;
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
        // Initialize the activePlayable with the first tape (if available)
        if (tapeClips.Count > 0)
            activePlayable = tapeClips[tapes_Owned[activeTape]];
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
        // Change the active playable to the assigned tape
        activePlayable = tapeClips[tapes_Owned[activeTape]];
    }

    public void NextTape()
    {
        if (nextToggle != null)
        {
            nextToggle.isOn = true;
            StartCoroutine(TurnOffToggleAfterDelay(nextToggle, 2f));
        }
        else
        {
            Debug.LogError("NextToggle not found.");
        }

        if(activePlayable == null)
            return;
        if (tapes_Owned.Count <= 1)
        {
            Debug.LogWarning("No additional tapes available.");
            return;
        }

        activePlayable.Stop();

        int indexOfActiveTape = tapes_Owned.IndexOf(activeTape);
        int nextTapeIndex = (indexOfActiveTape + 1) % tapes_Owned.Count;

        if (nextTapeIndex == indexOfActiveTape)
        {
            Debug.LogWarning("No different next tape available.");
            return;
        }

        // Change the active playable to the next tape
        activeTape = nextTapeIndex;
        activePlayable = tapeClips[tapes_Owned[activeTape]];
        activeTapeName.text = activePlayable.gameObject.name;
        switchedTape = true;
        Debug.Log("Switched tapes");

        // Set the "NextToggle" and turn it off after 2 seconds
        
    }

    public void Play()
    {
        if(activePlayable == null)
            return;
        activePlayable.Play();
        playToggle.isOn = true;
        //UpdateFunctionToggle();
    }

    public void Stop()
    {   
        if(activePlayable == null)
            return;
        activePlayable.Pause();
        stopToggle.isOn = true;
        StartCoroutine(TurnOffToggleAfterDelay(stopToggle, 2f));
    }

    public void Pause()
    {
        if(activePlayable == null)
            return;
        activePlayable.Stop();
        pauseToggle.isOn = true;
    }

    public void RewindTape()
    {
        if(activePlayable == null)
            return;
        // Check if the activePlayable is valid
        if (activePlayable.playableAsset != null)
        {
            // Calculate the new time by subtracting a certain duration (e.g., 1 second)
            double newTime = activePlayable.time - 2.0; // Adjust the rewind duration as needed

            // Clamp the new time to ensure it doesn't go below 0
            newTime = Mathf.Clamp((float)newTime, 0f, (float)activePlayable.duration);

            // Set the new time for the activePlayable
            activePlayable.time = newTime;

            // Set the speed to make it play backward
            activePlayable.playableGraph.GetRootPlayable(0).SetSpeed(-1.0f);

            // Play from the new time
            activePlayable.Play();
        }
        else
        {
            Debug.LogWarning("No playable asset assigned to activePlayable.");
        }
    }


    private IEnumerator TurnOffToggleAfterDelay(Toggle toggle, float delay)
    {
        switchedTape = false;
        yield return new WaitForSeconds(delay);
        toggle.isOn = false;
    }

    private Toggle FindToggleInChildren(string toggleName)
    {
        Toggle[] toggles = GetComponentsInChildren<Toggle>(true); // Include inactive toggles
        return toggles.FirstOrDefault(toggle => toggle.name == toggleName);
    }
}
