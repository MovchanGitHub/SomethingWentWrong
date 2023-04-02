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
        pauseScript.ShowMenu();
        isOpened = false;
    }
    public void ShowSettings()
    {
        isOpened = true;
        settingsMenu.SetActive(true);
        pauseScript.HideMenu();
    }
    public void ShowHideSettings()
    {
        isOpened = !isOpened;
        settingsMenu.GameObject().SetActive(isOpened);
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
            case 0: { Screen.SetResolution(1280, 720, Screen.fullScreen); GameManager.GM.InventoryManager.changeScale(2); } break;
            case 1: {Screen.SetResolution(1920, 1080, Screen.fullScreen); GameManager.GM.InventoryManager.changeScale(3); } break;
            case 2: {Screen.SetResolution(2560, 1440, Screen.fullScreen); GameManager.GM.InventoryManager.changeScale(4); } break;
            case 3: {Screen.SetResolution(3840, 2160, Screen.fullScreen); GameManager.GM.InventoryManager.changeScale(6); } break;
                
        }
    }

    public void SetFullscreen(bool is_fullscreen)
    {
        Screen.SetResolution(Screen.width, Screen.height, is_fullscreen);
        Debug.Log($"Is full screen - {is_fullscreen}");
    }

}
