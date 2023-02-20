using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject screen;
    public bool isOpened;
    public TextMeshProUGUI title;
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
                ShowDeathScreen("Вы проиграли");
                pause.PauseGame(true);
            }
        }
    }

    public void HideDeathScreen()
    {
        isOpened = false;
        screen.SetActive(false);
    }
    public void ShowDeathScreen(string message)
    {
        foreach (var window in windows)
        {
            window.SetActive(false);
        }
        isOpened = true;
        title.text = message;
        screen.SetActive(true);
    }
}
