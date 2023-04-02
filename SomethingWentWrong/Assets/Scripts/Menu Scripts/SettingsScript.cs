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

    private void Awake()
    {
        pauseScript = GetComponent<InGameMenuScript>();
    }

    private void Start()
    {
        settingsMenu = GM.UI.SettingsMenu;
        settingsMenu.SetActive(false);
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
        switch (value)
        {
            case 0: { Screen.SetResolution(1280, 720, Screen.fullScreen); } break;
            case 1: {Screen.SetResolution(1920, 1080, Screen.fullScreen); } break;
            case 2: {Screen.SetResolution(2560, 1440, Screen.fullScreen); } break;
            case 3: {Screen.SetResolution(3840, 2160, Screen.fullScreen); } break;
                
        }
    }

    public void SetFullscreen(bool is_fullscreen)
    {
        Screen.SetResolution(Screen.width, Screen.height, is_fullscreen);
        Debug.Log($"Is full screen - {is_fullscreen}");
    }

}
