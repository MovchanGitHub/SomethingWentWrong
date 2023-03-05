using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static GameManager;

public class InGameMenuScript : MonoBehaviour
{
    GameObject pauseMenu;
    // public InventoryManager inventory;
    GameObject deathScreen;
    GameObject winScreen;
    GameObject controlsMenu;
    SettingsScript settingsScript;
    public bool isOpened;
    public bool isPaused;

    private void Awake()
    {
        settingsScript = GetComponent<SettingsScript>();
    }

    private void Start()
    {
        pauseMenu = GM.UI.PauseMenu;
        deathScreen = GM.UI.DeathScreen;
        winScreen = GM.UI.WinScreen;
        controlsMenu = GM.UI.ControlsMenu;

        pauseMenu.SetActive(isOpened);
        deathScreen.SetActive(false);
        winScreen.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void EscapeIsPressed (UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // if (inventory.isOpened)
        //     inventory.gameObject.SetActive(false);
        // else 
        if (GM.InventoryManager.isCanvasActive)
        {
            GM.InventoryManager.activateInventory(false);
        }

        if (settingsScript.isOpened)
        {
            settingsScript.HideSettings();
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

    public void PauseGame(bool state)
    {
        isPaused = state;
        Time.timeScale = state ? 0 : 1;
    }
    
    public void HideMenu()
    {
        isOpened = false;
        pauseMenu.SetActive(false);
        GM.InventoryManager.canBeOpened = !isOpened;
    }

    public void ShowMenu()
    {
        isOpened = true;
        pauseMenu.SetActive(true);
        GM.InventoryManager.canBeOpened = !isOpened;
    }
    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        pauseMenu.SetActive(isOpened);
    }
    
}

