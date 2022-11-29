using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class SettingsScript : MonoBehaviour
{

    public GameObject Settings;
    public GameObject Backgound;
    [SerializeField] private bool isOpened;
    [SerializeField] public int musicVolume;
    [SerializeField] public int soundsVolume;
    [SerializeField] public bool isFullScreen;

    private void Awake()
    {
        Settings.SetActive(false);
        Backgound.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isOpened)
        {
            ShowHideSettings();
            GetComponentInParent<InGameMenuScript>().ShowHideMenu();
        }
    }
    public void ShowHideSettings()
    {
        isOpened = !isOpened;
        // Time.timeScale = isOpened ? 0 : 1;
        Settings.GameObject().SetActive(isOpened);
        Backgound.GameObject().SetActive(isOpened);
        
    }

    public void SetMusicVolume(int value)
    {
        musicVolume = value;
    }
    public void SetSoundsVolume(int value)
    {
        soundsVolume = value;
    }
    public void SetResolution(int value)
    {
        switch (value)
        {
            case 0: Screen.SetResolution(1280, 720, Screen.fullScreen); break;
            case 1: Screen.SetResolution(1920, 1080, Screen.fullScreen); break;
            case 2: Screen.SetResolution(2560, 1440, Screen.fullScreen); break;
            case 3: Screen.SetResolution(3840, 2160, Screen.fullScreen); break;
                
        }
    }

    public void SetFullscreen(bool is_fullscreen)
    {
        Screen.SetResolution(Screen.width, Screen.height, is_fullscreen);
        Debug.Log($"Is full screen - {is_fullscreen}");
    }

}
