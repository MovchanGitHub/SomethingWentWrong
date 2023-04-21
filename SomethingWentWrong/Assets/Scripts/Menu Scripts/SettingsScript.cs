using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;


public class SettingsScript : MonoBehaviour
{

    GameObject settingsMenu;
    InGameMenuScript pauseScript;
    
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

        GM.UI.SettingsMenu.transform.GetChild(4).GetComponent<Toggle>().isOn = Screen.fullScreen;
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
        var match = Regex.Match(options[value].text, @"(\d+)X(\d+)");
        Screen.SetResolution(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), Screen.fullScreen);
    }

    public void SetFullscreen(bool is_fullscreen)
    {
        Screen.SetResolution(Screen.width, Screen.height, is_fullscreen);
        Debug.Log($"Is full screen - {is_fullscreen}");
    }

}
