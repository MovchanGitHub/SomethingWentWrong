using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGMEncyclopedia : MonoBehaviour
{
    [SerializeField] private EncyclopediaManager encyclopediaScript;

    [SerializeField] private GameObject plantsTab;
    [SerializeField] private GameObject enemiesTab;
    [SerializeField] private GameObject plantsTabHeader;
    [SerializeField] private GameObject enemiesTabHeader;
    [SerializeField] private GameObject extraInfoPlantPanel;
    [SerializeField] private GameObject lootPrefab;
    //[SerializeField] private GameObject newNoteNotification;
    [SerializeField] private GameObject extraInfoEnemyPanel;

    private void Awake()
    {
        EncyclopediaMiniGM = this;
    }
    public static MiniGMEncyclopedia EncyclopediaMiniGM { get; private set; }

    public EncyclopediaManager EncyclopediaScript { get { return encyclopediaScript; } }
    public GameObject ExtraInfoEnemyPanel { get { return extraInfoEnemyPanel; } }
    public GameObject ExtraInfoPlantPanel { get { return extraInfoPlantPanel; } }
    public GameObject LootPrefab { get { return lootPrefab; } }
    public GameObject PlantsTab { get { return plantsTab; } }
    public GameObject EnemiesTab { get { return enemiesTab; } }
    public GameObject PlantsTabHeader { get { return plantsTabHeader; } }
    public GameObject EnemiesTabHeader { get { return enemiesTabHeader; } }
}
