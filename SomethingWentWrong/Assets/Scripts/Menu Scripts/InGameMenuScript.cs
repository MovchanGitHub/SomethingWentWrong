using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameMenuScript : MonoBehaviour
{
    public GameObject pause;
    public GameObject deathScreen;
    public GameObject winScreen;
    public GameObject controls;
    public bool isOpened;

    
    private void Awake()
    {
        pause.SetActive(isOpened);
        deathScreen.SetActive(false);
        winScreen.SetActive(false);
        controls.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowHideMenu();
            if (controls.activeSelf)
                controls.SetActive(false);
        }
        
    }

    public void HideMenu()
    {
        isOpened = false;
        pause.SetActive(false);
    }
    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        // Time.timeScale = isOpened ? 0 : 1;
        pause.SetActive(isOpened);
    }
}

