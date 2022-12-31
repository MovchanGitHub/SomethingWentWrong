using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public GameObject screen;
    public bool isOpened;
    public InGameMenuScript pause;
    public GameObject[] windows;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (isOpened)
            {
                HideWinScreen();
                pause.PauseGame(false);
            }
            else
            {
                ShowWinScreen();
                pause.PauseGame(true);
            }
        }
    }

    public void HideWinScreen()
    {
        isOpened = false;
        screen.SetActive(false);
    }
    public void ShowWinScreen()
    {
        foreach (var window in windows)
        {
            window.SetActive(false);
        }
        isOpened = true;
        screen.SetActive(true);
    }
}
