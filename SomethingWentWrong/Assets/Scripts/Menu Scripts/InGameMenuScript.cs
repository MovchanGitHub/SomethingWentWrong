using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static GameManager;
using Button = UnityEngine.UI.Button;

public class InGameMenuScript : MonoBehaviour
{
    private GameObject pauseMenu;
    // public InventoryManager inventory;
    private GameObject endScreen;
    private GameObject controlsMenu;
    private GameObject aboutWindow;
    private SettingsScript settingsScript;
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
        aboutWindow = GM.UI.AboutGame;
        

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
            var b = GM.UI.SettingsMenu.transform.GetChild(7).GetComponent<Button>();
            EventSystem.current.SetSelectedGameObject(b.GameObject());
            b.onClick.Invoke();
        }

        else if (aboutWindow.activeSelf)
        {
            var b = GM.UI.AboutGame.transform.GetChild(3).GetComponent<Button>();
            EventSystem.current.SetSelectedGameObject(b.GameObject());
            b.onClick.Invoke();
        }
        else if (controlsMenu.activeSelf)
        {
            var b = GM.UI.ControlsMenu.transform.GetChild(3).GetComponent<Button>();
            EventSystem.current.SetSelectedGameObject(b.GameObject());
            b.onClick.Invoke();
        }
        else
            if (isOpened)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }

    public void PauseGame(bool state)
    {
        isPaused = state;
        Time.timeScale = state ? 0f : 1f;
    }

    public void HideMenu()
    {
        isOpened = false;
        pauseMenu.SetActive(false);
        GM.InputSystem.UnblockPlayerInputs();
        GM.InputSystem.openInventoryInput.Enable();
        //GM.InputSystem.openEncyclopediaInput.Enable();
        PauseGame(false);
    }

    public void ShowMenu()
    {
        isOpened = true;
        pauseMenu.SetActive(true);
        GM.InputSystem.BlockPlayerInputs();
        GM.InputSystem.openInventoryInput.Disable();
        //GM.InputSystem.openEncyclopediaInput.Disable();
        PauseGame(true);
    }
    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        pauseMenu.SetActive(isOpened);
    }
    
}

