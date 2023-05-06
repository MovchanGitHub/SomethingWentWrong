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
    [SerializeField] GameObject playerHealthIncreaseSlider;
    [SerializeField] GameObject rocketHealthSlider;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject pointer;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] GameObject skillsMenu;
    [SerializeField] GameObject confirmExit;
    [SerializeField] GameObject endScreenMenuExit;
    [SerializeField] GameObject endScreenExit;

    [SerializeField] WeaponsBarScript weaponsBar;
    [SerializeField] Buttons buttons;
    [SerializeField] InGameMenuScript menuScript;
    [SerializeField] SettingsScript settings;
    [SerializeField] SkillsScript skillsScript;
    [SerializeField] EndScreen endScreenScript;
    [SerializeField] MiniGMEncyclopedia encyclopedia;

    private void Awake()
    {
        UIminiGM = this;
    }

    public static MiniGMUI UIminiGM { get; private set; }

    public GameObject PauseMenu { get { return pauseMenu; } }
    public GameObject PlayerHealthSlider { get { return playerHealthSlider; } }
    public GameObject PlayerHealthIncreaseSlider { get { return playerHealthIncreaseSlider; } }
    public GameObject RocketHealthSlider { get { return rocketHealthSlider; } }
    public GameObject HealthBar { get { return healthBar; } }
    public GameObject SettingsMenu { get { return settingsMenu; } }
    public GameObject EndScreen { get { return endScreen; } }
    public GameObject ControlsMenu { get { return controlsMenu; } }
    public GameObject AboutGame { get { return aboutGame; } }
    public GameObject Pointer { get { return pointer; } }
    public GameObject LoadingScreen { get { return loadingScreen; } }
    public GameObject ConfirmExit { get { return confirmExit; } }
    public GameObject EndScreenMenuExit { get { return endScreenMenuExit; } }
    public GameObject EndScreenExit { get { return endScreenExit; } }

    public GameObject SkillsMenu { get { return skillsMenu; } }
    public WeaponsBarScript WeaponsBarScript { get { return weaponsBar; } }
    public Buttons Buttons { get { return buttons; } }
    public SettingsScript SettingsScript { get { return settings; } }
    public SkillsScript SkillsScript { get { return skillsScript; } }
    public EndScreen EndScreenScript { get { return endScreenScript; } }
    public InGameMenuScript InGameMenuScript  { get { return menuScript; } }
    public MiniGMEncyclopedia Encyclopedia { get { return encyclopedia; } }
}
