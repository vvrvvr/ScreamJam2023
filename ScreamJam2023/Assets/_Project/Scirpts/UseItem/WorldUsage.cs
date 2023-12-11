using UnityEngine;

public class WorldUsage : MonoBehaviour
{
    // Common properties or methods for world usage can be defined here

    protected void Start()
    {
        // Common initialization logic can go here
    }

    protected void Update()
    {
        // Common update logic can go here
    }

    protected void OnDestroy()
    {
        // Common cleanup logic can go here
    }

    // Common use method that can be overridden by child classes
    public virtual void Use()
    {
        Debug.Log("WorldUsage: Default Use method");
    }

    // Add more methods or properties as needed
}
