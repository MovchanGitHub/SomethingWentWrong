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
    GameObject endScreen;
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
        endScreen = GM.UI.EndScreen;
        controlsMenu = GM.UI.ControlsMenu;

        pauseMenu.SetActive(isOpened);
        endScreen.SetActive(false);
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
        GM.InputSystem.UnblockPlayerInputs();
        GM.InputSystem.openInventoryInput.Enable();
        //GM.InputSystem.openEncyclopediaInput.Enable();
    }

    public void ShowMenu()
    {
        isOpened = true;
        pauseMenu.SetActive(true);
        GM.InventoryManager.canBeOpened = !isOpened;
        GM.InputSystem.BlockPlayerInputs();
        GM.InputSystem.openInventoryInput.Disable();
        //GM.InputSystem.openEncyclopediaInput.Disable();
    }
    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        pauseMenu.SetActive(isOpened);
    }
    
}

