using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    #endregion

    public void Update()
    {
    }
    
    public void ActivateTape(GameObject tape)
    {
        if (tape.activeSelf)
            SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
