using UnityEngine;

public class DeactivationSound : MonoBehaviour
{
    // Reference to the Wwise sound event
    public AK.Wwise.Event wwiseEvent;

    private void OnDisable()
    {
        // Check if the Wwise event is set
        if (wwiseEvent != null)
        {
            // Trigger the Wwise sound event when the object is deactivated
            wwiseEvent.Post(gameObject);
        }
    }
}
