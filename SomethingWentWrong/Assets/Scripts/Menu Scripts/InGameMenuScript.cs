using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameMenuScript : MonoBehaviour
{
    public GameObject InGameMenuPanel;
    public GameObject Background;
    public GameObject DeathScreen;
    public GameObject WinScreen;
    public GameObject ControlKeysWindow;
    private bool isOpened;

    
    private void Awake()
    {
        InGameMenuPanel.SetActive(isOpened);
        Background.SetActive(isOpened);
        DeathScreen.SetActive(false);
        WinScreen.SetActive(false);
        ControlKeysWindow.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowHideMenu();
            if (ControlKeysWindow.activeSelf)
                ControlKeysWindow.SetActive(false);
        }
        
    }
    
    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        // Time.timeScale = isOpened ? 0 : 1;
        InGameMenuPanel.GameObject().SetActive(isOpened);
        Background.GameObject().SetActive(isOpened);
    }
}

