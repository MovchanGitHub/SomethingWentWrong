using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;



public class SettingsScript : MonoBehaviour
{

    GameObject settingsMenu;
    InGameMenuScript pauseScript;
    private GameObject resolutionCB;
    
    [SerializeField] public float musicVolume;
    [SerializeField] public float soundsVolume;
    public bool isOpened;
    
    public TMPro.TMP_Dropdown dropdown;
    
    private void Awake()
    {
        pauseScript = GetComponent<InGameMenuScript>();
    }

    private void Start()
    {
        settingsMenu = GM.UI.SettingsMenu;
        settingsMenu.SetActive(false);
        dropdown = GM.UI.SettingsMenu.GetComponentInChildren<TMPro.TMP_Dropdown>();
    }
    
    public void HideSettings()
    {
        settingsMenu.SetActive(false);
        pauseScript.ShowHideMenu();
        isOpened = false;
    }
    public void ShowSettings()
    {
        isOpened = true;
        settingsMenu.SetActive(true);
        pauseScript.ShowHideMenu();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
    }
    public void SetSoundsVolume(float value)
    {
        soundsVolume = value;

    }
    public void SetResolution(int value)
    {
        var options = dropdown.options;
        var data = options[value].text.Split('X');
        Screen.SetResolution(int.Parse(data[0]), int.Parse(data[1]), Screen.fullScreen);
    }

    public void SetFullscreen(bool is_fullscreen)
    {
        Screen.SetResolution(Screen.width, Screen.height, is_fullscreen);
        Debug.Log($"Is full screen - {is_fullscreen}");
    }

}
