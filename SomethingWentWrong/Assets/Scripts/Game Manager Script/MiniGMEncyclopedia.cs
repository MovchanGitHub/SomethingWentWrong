using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGMEncyclopedia : MonoBehaviour
{
    [SerializeField] private EncyclopediaManager encyclopediaScript;

    [SerializeField] private GameObject plantsTab;
    [SerializeField] private GameObject enemiesTab;
    [SerializeField] private GameObject loreTab;
    //[SerializeField] private GameObject plantsTabHeader;
    //[SerializeField] private GameObject enemiesTabHeader;
    [SerializeField] private GameObject extraInfoPlantPanel;
    [SerializeField] private GameObject extraInfoEnemyPanel;
    [SerializeField] private GameObject extraInfoLorePanel;
    //[SerializeField] private GameObject newNoteNotification;

    private void Awake()
    {
        EncyclopediaMiniGM = this;
    }
    public static MiniGMEncyclopedia EncyclopediaMiniGM { get; private set; }

    public EncyclopediaManager EncyclopediaScript { get { return encyclopediaScript; } }
    public GameObject PlantsTab { get { return plantsTab; } }
    public GameObject EnemiesTab { get { return enemiesTab; } }
    public GameObject LoreTab { get { return loreTab; } }
    public GameObject ExtraInfoPlantPanel { get { return extraInfoPlantPanel; } }
    public GameObject ExtraInfoEnemyPanel { get { return extraInfoEnemyPanel; } }
    public GameObject ExtraInfoLorePanel { get { return extraInfoLorePanel; } }
    //public GameObject PlantsTabHeader { get { return plantsTabHeader; } }
    //public GameObject EnemiesTabHeader { get { return enemiesTabHeader; } }
}
