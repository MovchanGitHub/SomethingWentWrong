using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static GameManager;

public class EndScreen: MonoBehaviour
{

    public Color TutorialWinColor;
    private GameObject endScreen;
    [HideInInspector] public bool isOpened;
    public TextMeshProUGUI title;
    public TextMeshProUGUI newScoreTitle;
    public TextMeshProUGUI maxScore;
    public DayNightCycle dayNightCycle;
    GameObject[] windows;

    [SerializeField] private int MaxScore;

    private void Start() {
        endScreen = GM.UI.EndScreen;
        windows = new[] { GM.UI.PauseMenu, GM.UI.SettingsMenu, GM.UI.ControlsMenu, GM.UI.SkillsMenu };
        newScoreTitle.gameObject.SetActive(false);
    }

    public void HideDeathScreen() {
        isOpened = false;
        endScreen.SetActive(false);
    }
    
    public void ShowDeathScreen(string message) {
        if (!GM.IsTutorial)
        {
            var days = dayNightCycle.DayCount;
            foreach (var window in windows)
                window.SetActive(false);
        
            if (MaxScore < days) {
                MaxScore = days;
                newScoreTitle.gameObject.SetActive(true);
            }
        
            maxScore.text = $"Рекорд: {MaxScore}";
        }

        isOpened = true;
        if (message == "Вы прошли обучение!\nно какой ценой...")
            title.color = TutorialWinColor;
        title.text = message;
        endScreen.SetActive(true);
    }
}
