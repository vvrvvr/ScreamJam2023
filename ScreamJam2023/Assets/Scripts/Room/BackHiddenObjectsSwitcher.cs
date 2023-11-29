using UnityEngine;

public class BackHiddenObjectsSwitcher : MonoBehaviour
{
    private BoxCollider _collider;
    
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    public void TurnSwitcherOn()
    {
        _collider.enabled = true;
    }
    
}
