using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGMUI : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject aboutGame;
    [SerializeField] GameObject playerHealthSlider;
    [SerializeField] GameObject rocketHealthSlider;
    [SerializeField] GameObject healthBar;

    [SerializeField] GameObject skillsMenu;

    [SerializeField] WeaponsBarScript weaponsBar;

    [SerializeField] EncyclopediaManager encyclopedia;

    private void Awake()
    {
        UIminiGM = this;
    }

    public static MiniGMUI UIminiGM { get; private set; }

    public GameObject PauseMenu { get { return pauseMenu; } }
    public GameObject PlayerHealthSlider { get { return playerHealthSlider; } }
    public GameObject RocketHealthSlider { get { return rocketHealthSlider; } }
    public GameObject HealthBar { get { return healthBar; } }
    public GameObject SettingsMenu { get { return settingsMenu; } }
    public GameObject EndScreen { get { return endScreen; } }
    public GameObject ControlsMenu { get { return controlsMenu; } }
    public GameObject AboutGame { get { return aboutGame; } }

    public GameObject SkillsMenu { get { return skillsMenu; } }
    public WeaponsBarScript WeaponsBarScript { get { return weaponsBar; } }

    public EncyclopediaManager Encyclopedia { get { return encyclopedia; } }
}
