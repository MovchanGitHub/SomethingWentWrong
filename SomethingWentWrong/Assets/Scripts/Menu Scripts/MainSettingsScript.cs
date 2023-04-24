using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainSettingsScript : MonoBehaviour
{

    public GameObject Settings;
    public GameObject Backgound;
    [SerializeField] private bool isOpened;
    [SerializeField] public double musicVolume;
    [SerializeField] public double soundsVolume;
    [SerializeField] public bool isFullScreen;

    private void Awake()
    {
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape) && isOpened)
        // {
        //     ShowHideSettings();
        //     GetComponentInParent<MainMenuScript>().ShowHideMenu();
        // }
    }
    public void ShowHideSettings()
    {
        isOpened = !isOpened;
        // Settings.GameObject().layer = isOpened ? 0 : -1;
        Settings.GameObject().SetActive(isOpened);
        Backgound.GameObject().SetActive(isOpened);
    }

    public void SetMusicVolume(double value)
    {
        musicVolume = value;
    }
    public void SetSoundsVolume(double value)
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
