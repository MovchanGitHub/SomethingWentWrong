using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static GameManager;

public class DeathScreen : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject deathScreen;
    public bool isOpened;
    TextMeshProUGUI title;
    GameObject[] windows;

    private void Start()
    {
        deathScreen = GM.UI.DeathScreen;
        title = deathScreen.GetComponentInChildren<TextMeshProUGUI>();
        windows = new GameObject[] { GM.UI.PauseMenu, GM.UI.SettingsMenu, GM.UI.ControlsMenu, GM.UI.SkillsMenu };
    }

    public void HideDeathScreen()
    {
        isOpened = false;
        deathScreen.SetActive(false);
    }
    public void ShowDeathScreen(string message)
    {
        foreach (var window in windows)
        {
            window.SetActive(false);
        }
        isOpened = true;
        title.text = message;
        deathScreen.SetActive(true);
    }
}
