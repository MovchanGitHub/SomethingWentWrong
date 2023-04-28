using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class Buttons : MonoBehaviour
{
    private InGameMenuScript pauseScript;
    private SettingsScript settingsScript;

    private GameObject loadingScreen;
    private Slider slider;
    private TextMeshProUGUI progressText;

    private void Awake()
    {
    }

    private void Start()
    {
        pauseScript = GM.UI.InGameMenuScript;
        settingsScript = GM.UI.SettingsScript;
        loadingScreen = GM.UI.LoadingScreen;
        slider = loadingScreen.GetComponentInChildren<Slider>();
        progressText = loadingScreen.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnButtonLoadScene(string sceneName) {
        StartCoroutine(LoadAsync(sceneName));
        GM.PlayerMovement.IsAbleToMove = true;
        GM.SurvivalManager.gameObject.SetActive(true);
        GM.SurvivalManager.SetDefault();
        pauseScript.PauseGame(false);
    }

    public void OnContinueButton() {
        pauseScript.HideMenu();
        EventSystem.current.SetSelectedGameObject(pauseScript.transform.GetChild(0).gameObject);
    }
    /// Settings 
    public void OnButtonSettings() {
        settingsScript.ShowSettings();
    }
    public void OnButtonSettingsBack() {
        settingsScript.HideSettings();
    }
    /// Controls
    public void OnButtonControlKeys() {
        pauseScript.ShowHideMenu();
        GM.UI.ControlsMenu.SetActive(true);
    }
    public void OnButtonControlBack() {
        GM.UI.ControlsMenu.SetActive(false);
        pauseScript.ShowHideMenu();
    }
    /// AboutGame
    public void OnButtonAboutGame() {
        pauseScript.ShowHideMenu();
        GM.UI.AboutGame.SetActive(true);
    }
    public void OnButtonAboutGameBack() {
        GM.UI.AboutGame.SetActive(false);
        pauseScript.ShowHideMenu();
    }
    
    /// Exit
    public void OnButtonExit() {
        Debug.Log("Quit application");
        Application.Quit();
    }
    
    IEnumerator LoadAsync(string sceneName) {
        loadingScreen.SetActive(true);
        
        var oper = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitForSecondsRealtime(2);

        while (!oper.isDone) {
            float progress = Mathf.Clamp01(oper.progress / .9f);
            slider.value = progress;
            progressText.text = (int)progress * 100 + "%";
            
            yield return null;
        }
    }

    public void OnButtonConfirmExit()
    {
        GM.UI.PauseMenu.SetActive(false);
        GM.UI.ConfirmExit.SetActive(true);
    }
    public void OnButtonCancelExit()
    {
        GM.UI.ConfirmExit.SetActive(false);
        GM.UI.PauseMenu.SetActive(true);
    }
}
