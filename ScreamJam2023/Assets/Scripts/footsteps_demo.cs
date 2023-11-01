using UnityEngine;
using AK.Wwise;
using UHFPS.Runtime;

public class FootstepController : MonoBehaviour
{
    public AK.Wwise.Event FootstepSoundEvent; // Reference to your Wwise Footstep Event

    public float minFootstepIntervalWalking = 0.6f;
    public float maxFootstepIntervalWalking = 0.8f;
    public float minFootstepIntervalRunning = 0.4f; // Faster interval for running
    public float maxFootstepIntervalRunning = 0.6f; // Faster interval for running

    private float nextFootstepTimeWalking;
    private float nextFootstepTimeRunning;

    private PlayerStateMachine playerStateMachine;
    private string currentTerrainTag = "Default"; // Default switch value

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        CalculateNextFootstepTime();
    }

    private void Update()
    {
        if (ShouldPlayFootstepSound())
        {
            PlayFootstepSound();
        }
    }

    bool ShouldPlayFootstepSound()
    {
        // Check if the player is in the walk or run state
        bool isWalking = playerStateMachine.IsCurrent(PlayerStateMachine.WALK_STATE);
        bool isRunning = playerStateMachine.IsCurrent(PlayerStateMachine.RUN_STATE);

       // Debug.Log("Player is " + (isWalking ? "walking" : isRunning ? "running" : "neither"));

        return (isWalking || isRunning);
    }

    void PlayFootstepSound()
    {
        if (FootstepSoundEvent != null)
        {
            float minInterval, maxInterval;

            if (playerStateMachine.IsCurrent(PlayerStateMachine.WALK_STATE))
            {
                minInterval = minFootstepIntervalWalking;
                maxInterval = maxFootstepIntervalWalking;
            }
            else if (playerStateMachine.IsCurrent(PlayerStateMachine.RUN_STATE))
            {
                minInterval = minFootstepIntervalRunning;
                maxInterval = maxFootstepIntervalRunning;
            }
            else
            {
                minInterval = maxInterval = 0f; // No intervals for other states
            }

            float nextFootstepTime;

            if (playerStateMachine.IsCurrent(PlayerStateMachine.WALK_STATE))
            {
                nextFootstepTime = nextFootstepTimeWalking;
            }
            else if (playerStateMachine.IsCurrent(PlayerStateMachine.RUN_STATE))
            {
                nextFootstepTime = nextFootstepTimeRunning;
            }
            else
            {
                nextFootstepTime = Time.time;
            }

            if (Time.time >= nextFootstepTime)
            {
                // Randomize the interval within the specified min and max intervals
                nextFootstepTime = Time.time + Random.Range(minInterval, maxInterval);

                // Post the sound event with the terrain-based switch
                AkSoundEngine.SetSwitch("switch_material", currentTerrainTag, gameObject);
                FootstepSoundEvent.Post(gameObject);

                if (playerStateMachine.IsCurrent(PlayerStateMachine.WALK_STATE))
                {
                    nextFootstepTimeWalking = nextFootstepTime;
                }
                else if (playerStateMachine.IsCurrent(PlayerStateMachine.RUN_STATE))
                {
                    nextFootstepTimeRunning = nextFootstepTime;
                }
            }
        }
    }

    string GetTerrainTagUnderPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f))
        {
            return hit.collider.gameObject.tag;
        }
        return "Default"; // Return a default tag if no terrain is found
    }

    void FixedUpdate()
    {
        // Update the terrain tag under the player
        currentTerrainTag = GetTerrainTagUnderPlayer();
    }

    // Define the CalculateNextFootstepTime function
    void CalculateNextFootstepTime()
    {
        if (playerStateMachine.IsCurrent(PlayerStateMachine.WALK_STATE))
        {
            nextFootstepTimeWalking = Time.time + Random.Range(minFootstepIntervalWalking, maxFootstepIntervalWalking);
        }
        else if (playerStateMachine.IsCurrent(PlayerStateMachine.RUN_STATE))
        {
            nextFootstepTimeRunning = Time.time + Random.Range(minFootstepIntervalRunning, maxFootstepIntervalRunning);
        }
    }
}
