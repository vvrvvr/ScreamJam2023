using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class EnemyBehavior : MonoBehaviour
{
    public float raycastDistance = 80f;
    public string switchGroup = "switch_monster";
    public string calmSwitch = "calm";
    public string angrySwitch = "angry";

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (player == null)
        {
            // Player not found, cannot perform the raycast
            return;
        }

        Vector3 directionToPlayer = player.transform.position - transform.position;

        if (directionToPlayer == Vector3.zero)
        {
            // Handle the case where the direction is zero or too small
            return;
        }

        // Debug ray to visualize the raycast
        Debug.DrawRay(transform.position, directionToPlayer, Color.green);

        Ray ray = new Ray(transform.position, directionToPlayer);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Player is within line of sight, switch to angry state
                AkSoundEngine.SetSwitch(switchGroup, angrySwitch, gameObject);
            }
            else
            {
                // Player is not within line of sight, switch to calm state
                AkSoundEngine.SetSwitch(switchGroup, calmSwitch, gameObject);
            }
        }
        else
        {
            // No collision detected, switch to calm state
            AkSoundEngine.SetSwitch(switchGroup, calmSwitch, gameObject);
        }
    }
}
