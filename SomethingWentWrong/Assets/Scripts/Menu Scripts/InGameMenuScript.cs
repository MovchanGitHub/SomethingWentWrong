using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;
using Button = UnityEngine.UI.Button;

public class InGameMenuScript : MonoBehaviour
{
    private GameObject pauseMenu;
    private GameObject endScreen;
    private GameObject controlsMenu;
    private GameObject aboutWindow;
    private GameObject confirmExit;
    private GameObject confirmEndScreenMenuExit;
    private GameObject confirmEndScreenExit;
    private SettingsScript settingsScript;
    [HideInInspector] public bool isOpened;
    [HideInInspector] public bool isPaused;
    private bool showTutorialPopupAfterHidingMenu = false;

    public bool isAvailableToOpenInventoryTutorial;

    private void Awake()
    {
        PauseGame(false);
    }

    private void Start() {
        settingsScript = GM.UI.SettingsScript;
        if (GM.IsTutorial)
            isAvailableToOpenInventoryTutorial = false;
        //Debug.Log(isAvailableToOpenInventoryTutorial);

        pauseMenu = GM.UI.PauseMenu;
        endScreen = GM.UI.EndScreen;
        controlsMenu = GM.UI.ControlsMenu;
        aboutWindow = GM.UI.AboutGame;
        confirmExit = GM.UI.ConfirmExit;
        confirmEndScreenMenuExit = GM.UI.EndScreenMenuExit;
        confirmEndScreenExit = GM.UI.EndScreenExit;

        pauseMenu.SetActive(isOpened);
        endScreen.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void EscapeIsPressed (UnityEngine.InputSystem.InputAction.CallbackContext context) {
        if (GM.UI.Encyclopedia.ExtraInfoLorePanel.activeSelf)
        {
            Debug.Log(1);
            GM.UI.Encyclopedia.EncyclopediaScript.CloseLoreNote();
            return;
        }

        if (GM.UI.Encyclopedia.EncyclopediaScript.isOpened && !GM.IsTutorial)
        {
            Debug.Log(2);
            GM.UI.Encyclopedia.EncyclopediaScript.OpenCloseEncyclopedia(context);
            return;
        }

        if (GM.InventoryManager.isCanvasActive) {
            GM.InventoryManager.activateInventory(false);
        }

        if (settingsScript.isOpened) {
            var b = GM.UI.SettingsMenu.transform.GetChild(7).GetComponent<Button>();
            EventSystem.current.SetSelectedGameObject(b.GameObject());
            b.onClick.Invoke();
        }

        else if (aboutWindow.activeSelf) {
            var b = GM.UI.AboutGame.transform.GetChild(3).GetComponent<Button>();
            EventSystem.current.SetSelectedGameObject(b.GameObject());
            b.onClick.Invoke();
        }
        else if (controlsMenu.activeSelf) {
            var b = GM.UI.ControlsMenu.transform.GetChild(3).GetComponent<Button>();
            EventSystem.current.SetSelectedGameObject(b.GameObject());
            b.onClick.Invoke();
        }
        else if (confirmExit.activeSelf)
        {
            var b = GM.UI.ConfirmExit.transform.GetChild(2).GetComponent<Button>();
            EventSystem.current.SetSelectedGameObject(b.GameObject());
            b.onClick.Invoke();
        }
        else
            if (isOpened) {
            HideMenu();
        }
        else {
            ShowMenu();
        }
    }

    public void PauseGame(bool state) {
        isPaused = state;
        Time.timeScale = state ? 0f : 1f;
    }

    public void HideMenu() {
        isOpened = false;
        pauseMenu.SetActive(false);
        GM.InputSystem.UnblockPlayerInputs();
        if (!GM.IsTutorial || isAvailableToOpenInventoryTutorial)
            GM.InputSystem.openInventoryInput.Enable();
        GM.InputSystem.openEncyclopediaInput.Enable();
        if (GM.IsTutorial && showTutorialPopupAfterHidingMenu)
        {
            GM.Tutorial.PopupSystem.CurrentPopupObject.SetActive(true);
            showTutorialPopupAfterHidingMenu = false;
        }
        PauseGame(false);
    }

    public void ShowMenu() {
        isOpened = true;
        pauseMenu.SetActive(true);
        GM.InputSystem.BlockPlayerInputs();
        GM.InputSystem.openInventoryInput.Disable();
        GM.InputSystem.openEncyclopediaInput.Disable();
        if (GM.IsTutorial && GM.Tutorial.PopupSystem.CurrentPopupObject.activeInHierarchy)
        {
            GM.Tutorial.PopupSystem.CurrentPopupObject.SetActive(false);
            showTutorialPopupAfterHidingMenu = true;
        }
        PauseGame(true);
    }
    public void ShowHideMenu() {
        isOpened = !isOpened;
        pauseMenu.SetActive(isOpened);
    }
    
}

