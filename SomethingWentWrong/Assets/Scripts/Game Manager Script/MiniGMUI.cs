using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGMUI : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject controlsMenu;

    [SerializeField] GameObject skillsMenu;

    [SerializeField] WeaponsBarScript weaponsBar;

    [SerializeField] EncyclopediaManager encyclopedia;

    private void Awake()
    {
        UIminiGM = this;
    }

    public static MiniGMUI UIminiGM { get; private set; }

    public GameObject PauseMenu { get { return pauseMenu; } }
    public GameObject SettingsMenu { get { return settingsMenu; } }
    public GameObject EndScreen { get { return endScreen; } }
    public GameObject ControlsMenu { get { return controlsMenu; } }

    public GameObject SkillsMenu { get { return skillsMenu; } }
    public WeaponsBarScript WeaponsBarScript { get { return weaponsBar; } }

    public EncyclopediaManager Encyclopedia { get { return encyclopedia; } }
}
