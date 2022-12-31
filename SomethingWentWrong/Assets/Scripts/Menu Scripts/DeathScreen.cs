using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject screen;
    public bool isOpened;
    public InGameMenuScript pause;
    public GameObject[] windows;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isOpened)
            {
                HideDeathScreen();
                pause.PauseGame(false);
            }
            else
            {
                ShowDeathScreen();
                pause.PauseGame(true);
            }
        }
    }

    public void HideDeathScreen()
    {
        isOpened = false;
        screen.SetActive(false);
    }
    public void ShowDeathScreen()
    {
        foreach (var window in windows)
        {
            window.SetActive(false);
        }
        isOpened = true;
        screen.SetActive(true);
    }
}
