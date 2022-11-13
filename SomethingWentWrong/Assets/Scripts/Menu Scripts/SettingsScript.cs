using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class SettingsScript : MonoBehaviour
{

    public GameObject Settings;
    [SerializeField] private bool isOpened = false;

    private void Awake()
    {
        Settings.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isOpened)
            ShowHideMenu();
    }
    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        Settings.GameObject().SetActive(isOpened);
    }

    public void SetVolume(double value)
    {
        
    }
    public void SetResolution(int value)
    {
        switch (value)
        {
            case 0: Screen.SetResolution(1280, 720, false); Debug.Log("heheheh"); break;
            case 1: Screen.SetResolution(1920, 1080, false); break;
            case 2: Screen.SetResolution(2560, 1440, false); break;
            case 3: Screen.SetResolution(3840, 2160, false); break;
                
        }
    }
}
