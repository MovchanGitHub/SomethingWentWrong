using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class Buttons : MonoBehaviour
{
    InGameMenuScript pauseScript;
    SettingsScript settingsScript;
    public Button[] buttons;

    [SerializeField] GameObject loadingScreen;
    Slider slider;
    TextMeshProUGUI progressText;

    private void Awake()
    {
        pauseScript = GetComponent<InGameMenuScript>();
        settingsScript = GetComponent<SettingsScript>();
    }

    private void Start()
    {
        slider = loadingScreen.GetComponentInChildren<Slider>();
        progressText = loadingScreen.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnButtonLoadScene(string sceneName)
    {
        Debug.Log($"Loading scene {sceneName}");
        StartCoroutine(LoadAsync(sceneName));
        GM.PlayerMovement.IsAbleToMove = true;
        GM.InventoryManager.canBeOpened = true;
        GM.SurvivalManager.gameObject.SetActive(true);
        GM.SurvivalManager.SetDefault();
        pauseScript.PauseGame(false);
    }
    
    public void OnContinueButton()
    {
        RefreshAnimation();
        pauseScript.HideMenu();
        pauseScript.PauseGame(false);
    }
    
    public void OnButtonExit()
    {
        Debug.Log("Quit application");
        Application.Quit();
    }

    public void OnButtonSettings()
    {
        RefreshAnimation();
        settingsScript.ShowSettings();
        pauseScript.HideMenu();
    }
    
    public void OnButtonControlKeys()
    {
        RefreshAnimation();
        pauseScript.ShowHideMenu();
        GM.UI.ControlsMenu.SetActive(true);
    }
    
    public void OnButtonAboutGame()
    {
        // RefreshAnimation();
        pauseScript.ShowHideMenu();
        GM.UI.AboutGame.SetActive(true);
    }
    
    
    public void OnButtonBack()
    {
        settingsScript.HideSettings();
        pauseScript.ShowMenu();
    }
    
    public void OnButtonControlBack()
    {
        RefreshAnimation();
        GM.UI.ControlsMenu.SetActive(false);
        pauseScript.ShowHideMenu();
    }
    public void OnButtonAboutGameBack()
    {
        // RefreshAnimation();
        GM.UI.AboutGame.SetActive(false);
        pauseScript.ShowHideMenu();
    }
    
    IEnumerator LoadAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        
        var oper = SceneManager.LoadSceneAsync(sceneName);
        while (!oper.isDone)
        {
            float progress = Mathf.Clamp01(oper.progress / .9f);
            slider.value = progress;
            progressText.text = (int)progress * 100 + "%";
            
            yield return null;
        }
    }

    public void RefreshAnimation()
    {
        foreach (var button in buttons)
        {
            button.animator.Rebind();
            button.animator.Update(0f);
        }
    }

}
