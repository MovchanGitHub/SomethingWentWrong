using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    private InGameMenuScript pause;
    private SettingsScript settings;
    
    
    private void Start()
    {
        pause = GetComponentInParent<InGameMenuScript>();
        settings = GetComponentInParent<SettingsScript>();
    }
    
    public void OnButtonLoadScene(string sceneName)
    {
        Debug.Log($"Loading scene {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
    
    public void OnContinueButton()
    {
        pause.ShowHideMenu();
    }
    
    public void OnButtonExit()
    {
        Debug.Log("Quit application");
        Application.Quit();
    }

    public void OnButtonSettings()
    {
        settings.ShowHideSettings();
        pause.ShowHideMenu();
    }
    public void OnButtonBack()
    {
        settings.ShowHideSettings();
        pause.ShowHideMenu();
    }

}
