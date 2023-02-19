using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public InGameMenuScript pause;
    public SettingsScript settings;
    public Button[] buttons;
    
    
    public void OnButtonLoadScene(string sceneName)
    {
        Debug.Log($"Loading scene {sceneName}");
        pause.PauseGame(false);
        StartCoroutine(LoadAsync(sceneName));
        IsometricPlayerMovementController.IsAbleToMove = true;
        // InventoryManager.instance.SetDefault();
        SurvivalManager.Instance.transform.gameObject.SetActive(true);
        SurvivalManager.Instance.SetDefault();
        GameManagerScript.instance.isUIOpened = false;
    }
    
    public void OnContinueButton()
    {
        RefreshAnimation();
        pause.HideMenu();
        pause.PauseGame(false);
    }
    
    public void OnButtonExit()
    {
        Debug.Log("Quit application");
        Application.Quit();
    }

    public void OnButtonSettings()
    {
        RefreshAnimation();
        settings.ShowSettings();
        pause.HideMenu();
    }
    
    public void OnButtonControlKeys()
    {
        RefreshAnimation();
        pause.ShowHideMenu();
        pause.controls.SetActive(true);
        
    }
    public void OnButtonBack()
    {
        settings.HideSettings();
        pause.ShowMenu();
    }
    
    public void OnButtonControlBack()
    {
        RefreshAnimation();
        pause.controls.SetActive(false);
        pause.ShowHideMenu();
    }
    
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI progressText;
    
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

    private void RefreshAnimation()
    {
        foreach (var button in buttons)
        {
            button.animator.Update(1);
        }
    }

}
