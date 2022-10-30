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

    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        Settings.GameObject().SetActive(isOpened);
    }
    
}
