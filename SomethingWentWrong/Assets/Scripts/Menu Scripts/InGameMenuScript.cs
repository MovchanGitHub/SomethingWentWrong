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
    [SerializeField] public bool isOpened;
    [SerializeField] public bool isMainMenu;

    
    private void Awake()
    {
        InGameMenuPanel.SetActive(isOpened);
        Background.SetActive(isOpened);
        DeathScreen.SetActive(false);
        WinScreen.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isMainMenu)
            ShowHideMenu();
    }
    
    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        InGameMenuPanel.GameObject().SetActive(isOpened);
        Background.GameObject().SetActive(isOpened);
    }
}

