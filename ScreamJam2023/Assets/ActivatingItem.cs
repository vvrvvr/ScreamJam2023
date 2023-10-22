using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatingItem : MonoBehaviour
{
    public GameObject taken;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveCassete()
    {
        taken.SetActive(true);
     //   Player.Instance.ActiveItem = taken;
    }
}
