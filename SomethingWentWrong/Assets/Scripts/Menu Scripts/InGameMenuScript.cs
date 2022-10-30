using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameMenuScript : MonoBehaviour
{
    public GameObject InGameMenuPanel;
    [SerializeField]
    private bool isOpened = false;
    
    private void Awake()
    {
        InGameMenuPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowHideMenu();
    }
    
    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        InGameMenuPanel.GameObject().SetActive(isOpened);
    }
}

