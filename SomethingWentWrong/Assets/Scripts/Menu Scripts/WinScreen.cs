using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class WinScreen : MonoBehaviour
{
    GameObject winScreen;
    public bool isOpened;
    GameObject[] windows;
    

    private void Start()
    {
        winScreen = GM.UI.WinScreen;
        windows = new GameObject[] { GM.UI.PauseMenu, GM.UI.SettingsMenu, GM.UI.ControlsMenu, GM.UI.SkillsMenu };
    }

    public void HideWinScreen()
    {
        isOpened = false;
        winScreen.SetActive(false);
    }
    public void ShowWinScreen()
    {
        foreach (var window in windows)
        {
            window.SetActive(false);
        }
        isOpened = true;
        winScreen.SetActive(true);
    }
}
