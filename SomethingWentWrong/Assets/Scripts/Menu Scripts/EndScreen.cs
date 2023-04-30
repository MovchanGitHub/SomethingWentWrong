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
    public TextMeshProUGUI maxScore;
    public DayNightCycle dayNightCycle;
    GameObject[] windows;

    [SerializeField] private int MaxScore;
    private string scorePath;

    
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
            foreach (var window in windows)
                window.SetActive(false);
        
            if (MaxScore < days) {
                MaxScore = days;
                newScoreTitle.gameObject.SetActive(true);
                SaveScore();
            }
        
            maxScore.text = $"Рекорд: {MaxScore}";
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
            MaxScore = save.score;
            fs.Close();
        }
    }

    private void SaveScore()
    {
        var bf = new BinaryFormatter();
        var fs = new FileStream(scorePath, FileMode.Create);
        var save = new ScoreSave(MaxScore);
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
