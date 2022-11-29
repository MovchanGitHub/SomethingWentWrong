using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainButtons : MonoBehaviour
{
    private MainMenuScript mainMenu;
    private MainSettingsScript settings;
    
     
    private void Start()
    {
        mainMenu = GetComponentInParent<MainMenuScript>();
        settings = GetComponentInParent<MainSettingsScript>();
    }
    
    public void OnButtonLoadScene(string sceneName)
    {
        Debug.Log($"Loading scene {sceneName}");
        // SceneManager.LoadScene(sceneName);
        StartCoroutine(LoadAsync(sceneName));
    }

   
    
    public void OnContinueButton()
    {
        StartCoroutine(LoadAsync("Level One"));

    }
    
    public void OnButtonExit()
    {
        Debug.Log("Quit application");
        Application.Quit();
    }

    public void OnButtonSettings()
    {
        settings.ShowHideSettings();
        mainMenu.ShowHideMenu();
    }
    public void OnButtonBack()
    {
        settings.ShowHideSettings();
        mainMenu.ShowHideMenu();
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
            progressText.text = progress * 100f + "%";
            
            yield return null;
        }
    }
    
    
}