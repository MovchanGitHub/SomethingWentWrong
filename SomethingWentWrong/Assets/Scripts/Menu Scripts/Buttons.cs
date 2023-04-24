using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class Buttons : MonoBehaviour
{
    InGameMenuScript pauseScript;
    SettingsScript settingsScript;

    GameObject loadingScreen;
    Slider slider;
    TextMeshProUGUI progressText;

    private void Awake() {
        pauseScript = GetComponent<InGameMenuScript>();
        settingsScript = GetComponent<SettingsScript>();
    }

    private void Start()
    {
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

    public void OnButtonSettings() {
        settingsScript.ShowSettings();
    }
    
    public void OnButtonControlKeys() {
        pauseScript.ShowHideMenu();
        GM.UI.ControlsMenu.SetActive(true);
    }
    
    public void OnButtonControlBack() {
        GM.UI.ControlsMenu.SetActive(false);
        pauseScript.ShowHideMenu();
    }
    
    public void OnButtonAboutGame() {
        pauseScript.ShowHideMenu();
        GM.UI.AboutGame.SetActive(true);
    }
    
    public void OnButtonAboutGameBack() {
        GM.UI.AboutGame.SetActive(false);
        pauseScript.ShowHideMenu();
    }
    
    public void OnButtonBack() {
        settingsScript.HideSettings();
    }
    
    public void OnButtonExit() {
        Debug.Log("Quit application");
        Application.Quit();
    }
    
    IEnumerator LoadAsync(string sceneName) {
        loadingScreen.SetActive(true);
        
        var oper = SceneManager.LoadSceneAsync(sceneName);
        while (!oper.isDone) {
            float progress = Mathf.Clamp01(oper.progress / .9f);
            slider.value = progress;
            progressText.text = (int)progress * 100 + "%";
            
            yield return null;
        }
    }
}
