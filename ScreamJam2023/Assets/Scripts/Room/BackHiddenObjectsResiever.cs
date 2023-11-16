using UnityEngine;

public class BackHiddenObjectsResiever : MonoBehaviour
{
    [SerializeField] private GameObject visibleObject;
    private Renderer _renderer;
    private BoxCollider _collider;
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = false;
        visibleObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag($"Switcher"))
        {
            _collider.enabled = false;
            visibleObject.SetActive(true);
            Debug.Log("switcher");
            
        }
    }
}
