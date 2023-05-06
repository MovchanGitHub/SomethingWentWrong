using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
    public TextMeshProUGUI maxScoreTitle;
    public TextMeshProUGUI currentScore;
    public DayNightCycle dayNightCycle;
    GameObject[] windows;

    private int maxScore;
    public int MaxScore
    {
        get => maxScore;
        set
        {
            maxScore = value;
            SaveScore();
            isNewScoreAchieved = true;
        }
    }
    private string scorePath;
    private bool isNewScoreAchieved = false;

    
    private void Start() {
        endScreen = GM.UI.EndScreen;
        windows = new[] { GM.UI.PauseMenu, GM.UI.SettingsMenu, GM.UI.ControlsMenu, GM.UI.SkillsMenu };
        newScoreTitle.gameObject.SetActive(false);
        scorePath = Application.persistentDataPath + "/score.gamesave";
        LoadScore();
    }

    public void HideDeathScreen() {
        isOpened = false;
        endScreen.SetActive(false);
    }
    
    public void ShowDeathScreen(string message) {
        if (!GM.IsTutorial)
        {
            var days = dayNightCycle.DayCount;
            currentScore.text = $"Прожито: {days}";
            foreach (var window in windows)
                window.SetActive(false);
        
            if (isNewScoreAchieved)
            {
                newScoreTitle.gameObject.SetActive(true);
            }
        
            maxScoreTitle.text = $"Рекорд: {maxScore}";
        }
        else
        {
            GM.Tutorial.PopupSystem.CurrentPopupObject.SetActive(false);
        }

        isOpened = true;
        if (message == "Вы прошли обучение!\nно какой ценой...")
        {
            GM.Tutorial.endGameButtons[0].SetActive(false);
            GM.Tutorial.endGameButtons[1].SetActive(true);
            title.color = TutorialWinColor;
        }
        title.text = message;
        endScreen.SetActive(true);
    }

    private void LoadScore()
    {
        if (File.Exists(scorePath)) {
            var bf = new BinaryFormatter();
            var fs = new FileStream(scorePath, FileMode.Open);
            var save = (ScoreSave)bf.Deserialize(fs);
            maxScore = save.score;
            fs.Close();
        }
    }

    public void SaveScore()
    {
        var bf = new BinaryFormatter();
        var fs = new FileStream(scorePath, FileMode.Create);
        var save = new ScoreSave(maxScore);
        bf.Serialize(fs, save);
        fs.Close();
    }
}

[System.Serializable]
public class ScoreSave {
    public int score;
    
    public ScoreSave(int score) {
        this.score = score;
    }
}
