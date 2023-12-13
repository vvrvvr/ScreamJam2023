using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Cass_Player : MonoBehaviour
{
    public PlayableDirector activePlayable;
    public int activeTape = 0;
    public List<int> tapes = new List<int> { 0 };
    [SerializeField] List<TimelineAsset> tapeClips;
    public ToggleGroup functionToggleGroup; // Reference to the ToggleGroup for play, stop, pause, rewind

    private void Start()
    {
        // Ensure activeTape is within bounds
        activeTape = Mathf.Clamp(activeTape, 0, tapes.Count - 1);
        // Initialize the activePlayable with the first tape (if available)
        if (tapeClips.Count > 0)
            activePlayable.playableAsset = tapeClips[tapes[activeTape]];
    }

    public void AddTape(int num)
    {
        tapes.Add(num);
    }

    public void AssignActiveTape(int num)
    {
        activeTape = Mathf.Clamp(num, 0, tapes.Count - 1);
        // Change the active playable to the assigned tape
        activePlayable.playableAsset = tapeClips[tapes[activeTape]];
    }

    public void NextTape()
    {
        if (tapes.Count == 0)
        {
            Debug.LogWarning("No tapes available.");
            return;
        }

        activeTape = (activeTape + 1) % tapes.Count;
        // Change the active playable to the next tape
        activePlayable.playableAsset = tapeClips[tapes[activeTape]];
        UpdateFunctionToggle();
    }

    public void Play()
    {
        activePlayable.Play();
        UpdateFunctionToggle();
    }

    public void Stop()
    {
        activePlayable.Stop();
        UpdateFunctionToggle();
    }

    public void Pause()
    {
        activePlayable.Pause();
        UpdateFunctionToggle();
    }

    public void RewindTape()
    {
        // Check if the activePlayable is valid
        if (activePlayable.playableAsset != null)
        {
            // Calculate the new time by subtracting a certain duration (e.g., 1 second)
            double newTime = activePlayable.time - 1.0; // Adjust the rewind duration as needed

            // Clamp the new time to ensure it doesn't go below 0
            newTime = Mathf.Clamp((float)newTime, 0f, (float)activePlayable.duration);

            // Set the new time for the activePlayable
            activePlayable.time = newTime;
            activePlayable.Play(); // Play from the new time
            UpdateFunctionToggle();
        }
        else
        {
            Debug.LogWarning("No playable asset assigned to activePlayable.");
        }
    }

    private void UpdateFunctionToggle()
    {
        // Determine the active function based on the state of Play, Stop, Pause, Rewind
        Toggle activeToggle = null;

        if (activePlayable.state == PlayState.Playing)
        {
            activeToggle = functionToggleGroup.ActiveToggles().FirstOrDefault(toggle => toggle.name == "PlayToggle");
        }
        else if (activePlayable.state == PlayState.Paused)
        {
            activeToggle = functionToggleGroup.ActiveToggles().FirstOrDefault(toggle => toggle.name == "PauseToggle");
        }
        else
        {
            if (activePlayable.time > 0)
            {
                activeToggle = functionToggleGroup.ActiveToggles().FirstOrDefault(toggle => toggle.name == "RewindToggle");
            }
            else
            {
                activeToggle = functionToggleGroup.ActiveToggles().FirstOrDefault(toggle => toggle.name == "StopToggle");
            }
        }

        // Toggle on the active toggle and toggle off others
        if (activeToggle != null)
        {
            activeToggle.isOn = true;
        }
        else
        {
            functionToggleGroup.SetAllTogglesOff();
        }
    }

}
