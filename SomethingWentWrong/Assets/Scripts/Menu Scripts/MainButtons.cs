using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainButtons : MonoBehaviour
{
    private MainMenuScript mainMenu;
    private MainSettingsScript settings;
    private Button[] buttons;
    public Animator backAnimator;
    

     
    private void Start()
    {
        mainMenu = GetComponentInParent<MainMenuScript>();
        settings = GetComponentInParent<MainSettingsScript>();
        buttons = GetComponentsInChildren<Button>();
    }
    
    public void OnButtonLoadScene(string sceneName)
    {
        Debug.Log($"Loading scene {sceneName}");
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
        foreach (var button in buttons)
        {
            button.animator.Update(1);
        }
        
        settings.ShowHideSettings();
        mainMenu.ShowHideMenu();
    }
    public void OnButtonBack()
    {
        backAnimator.Update(1);
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
            progressText.text = (int)progress * 100 + "%";
            
            yield return null;
        }
    }
    
    
}