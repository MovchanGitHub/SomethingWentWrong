using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameMenuScript : MonoBehaviour
{
    public GameObject pause;
    public SettingsScript settings;
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
            if (settings.isOpened)
            {
                settings.HideSettings();
                ShowMenu();
            }
            else
                if (isOpened)
                {
                    HideMenu();
                    PauseGame(false);
                }
                else
                {
                    PauseGame(true);
                    ShowMenu();
                }
        }
        
    }

    public void PauseGame(bool state)
    {
        Time.timeScale = state ? 0 : 1;
    }
    
    public void HideMenu()
    {
        isOpened = false;
        pause.SetActive(false);
    }

    public void ShowMenu()
    {
        isOpened = true;
        pause.SetActive(true);
    }
    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        pause.SetActive(isOpened);
    }
    
}

