using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGMUI : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject controlsMenu;

    [SerializeField] GameObject skillsMenu;

    [SerializeField] EncyclopediaManager encyclopedia;

    private void Awake()
    {
        UIminiGM = this;
    }

    public static MiniGMUI UIminiGM { get; private set; }

    public GameObject PauseMenu { get { return pauseMenu; } }
    public GameObject SettingsMenu { get { return settingsMenu; } }
    public GameObject DeathScreen { get { return deathScreen; } }
    public GameObject WinScreen { get { return winScreen; } }
    public GameObject ControlsMenu { get { return controlsMenu; } }

    public GameObject SkillsMenu { get { return skillsMenu; } }

    public EncyclopediaManager Encyclopedia { get { return encyclopedia; } }
}
