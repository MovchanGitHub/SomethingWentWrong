using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameMenuScript : MonoBehaviour
{
    public GameObject pause;
    public SettingsScript settings;
    // public InventoryManager inventory;
    public GameObject deathScreen;
    public GameObject winScreen;
    public GameObject controls;
    public bool isOpened;
    public bool isPaused;

    
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
            // if (inventory.isOpened)
            //     inventory.gameObject.SetActive(false);
            // else 
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
        isPaused = state;
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

