using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EncyclopediaManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject panelWithExtraInfo;
    [SerializeField] private GameObject extraInfoEnemyPanel;
    [SerializeField] private GameObject extraInfoPlantPanel;
    [SerializeField] private GameObject iconOfSpecialAbility;
    [SerializeField] private GameObject LootIcon;
    [SerializeField] private GameObject newNoteNotification;
    [SerializeField] private GameObject plantsTab;
    [SerializeField] private GameObject enemiesTab;
    [SerializeField] private GameObject plantsTabHeader;
    [SerializeField] private GameObject enemiesTabHeader;

    private Vector2 newNoteNotificationInitialPos;

    private Color32 selectedTab;
    private Color32 nonSelectedTab;

    [SerializeField] private List<NotesManager> enemiesNotes;
    [SerializeField] private List<NotesManager> plantsNotes;

    private static EncyclopediaManager instance;

    private bool isOpened;

    public static EncyclopediaManager Instance
    {
        get 
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isOpened = false;
        InitializeEncyclopedia();
        selectedTab = new Color32(89, 137, 0, 255);
        nonSelectedTab = new Color32(124, 192, 0, 255);
        newNoteNotificationInitialPos = transform.TransformPoint(GetComponent<RectTransform>().rect.xMax - newNoteNotification.GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.yMax + newNoteNotification.GetComponent<RectTransform>().rect.height / 2, 0);
        Debug.Log(newNoteNotification.GetComponent<RectTransform>().rect.height / 2);
        Debug.Log(-newNoteNotification.GetComponent<RectTransform>().rect.width);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenCloseEncyclopedia();
        }
        
    }

    private void InitializeEncyclopedia()
    {
        foreach (var notes in new List<List<NotesManager>> { enemiesNotes, plantsNotes })
            foreach (var note in notes)
            {
                if (note.creature.isOpenedInEcnyclopedia)
                {
                    //Debug.Log("1");
                    note.nameHeader.GetComponent<Text>().text = note.creature.name;
                    note.icon.GetComponent<Image>().sprite = note.creature.imageSmall;
                }
                else
                {
                    //Debug.Log("2");
                    note.nameHeader.GetComponent<Text>().text = "Unknown";
                    note.icon.GetComponent<Image>().sprite = note.creature.imageUnknown;
                }
            }
    }

    public void OpenNewCreature(CreaturesBase openedCreature)
    {
        Debug.Log("start");
        openedCreature.isOpenedInEcnyclopedia = true;
        GameObject notesWhereToSearch;
        int amountOfNotes;
        if (openedCreature.typeOfThisCreature == creatureType.Enemy)
        {
            notesWhereToSearch = enemiesTab;
            amountOfNotes = enemiesNotes.Count;
        }
        else
        {
            notesWhereToSearch = plantsTab;
            amountOfNotes = plantsNotes.Count;
        }

        string openedCreatureName = openedCreature.name;
        for (int i = 0; i < amountOfNotes; i++)
        {
            //Debug.Log(notesOfCreatures.transform.GetChild(i).GetComponent<NotesManager>().creature);
            if (notesWhereToSearch.transform.GetChild(i).GetComponent<NotesManager>().creature.name == openedCreatureName)
            {
                NotesManager notesManagerCode =  notesWhereToSearch.transform.GetChild(i).GetComponent<NotesManager>();
                Debug.Log(notesManagerCode.creature.name);
                Debug.Log(i);
                notesManagerCode.nameHeader.GetComponent<Text>().text = openedCreature.name;
                Debug.Log(notesManagerCode.nameHeader.GetComponent<Text>().text);
                Debug.Log(openedCreature.name);
                notesManagerCode.icon.GetComponent<Image>().sprite = openedCreature.imageSmall;

                ShowNewNoteNotification(openedCreature);
                break;
            }
        }
    }

    public void OpenExtraInfo(GameObject ChosenNote)
    {
        //Second time on the same Note?
        if (panelWithExtraInfo.activeSelf == true && panelWithExtraInfo.transform.GetChild(3).gameObject.GetComponent<Text>().text == ChosenNote.GetComponent<NotesManager>().nameHeader.GetComponent<Text>().text)
        {
            panelWithExtraInfo.SetActive(false);
        }
        //Common case - opening the extraInfo
        else
        {
            panelWithExtraInfo.SetActive(true);
            //for (int i = 0; i < extraInfoEnemyPanel.transform.GetChild(1).GetChild(2).childCount; i++)
            //{
            //    Destroy(extraInfoEnemyPanel.transform.GetChild(1).GetChild(2).GetChild(i).gameObject);
            //}
            //for (int i = 0; i < extraInfoPlantPanel.transform.GetChild(1).childCount; i++)
            //{
            //    Destroy(extraInfoPlantPanel.transform.GetChild(1).GetChild(i).gameObject);
            //}
            CreaturesBase curCreature = ChosenNote.GetComponent<NotesManager>().creature;
            if (curCreature.isOpenedInEcnyclopedia)
            {
                if (curCreature.typeOfThisCreature == creatureType.Enemy)
                {
                    ShowEnemyStats(ChosenNote);
                }
                else
                {
                    ShowPlantStats(ChosenNote);
                }

                panelWithExtraInfo.transform.GetChild(3).gameObject.GetComponent<Text>().text = curCreature.name;
                panelWithExtraInfo.transform.GetChild(4).gameObject.GetComponent<Text>().text = curCreature.description;
                panelWithExtraInfo.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = curCreature.imageBig;
            }
            else
            {
                extraInfoPlantPanel.SetActive(false);
                extraInfoEnemyPanel.SetActive(false);
                panelWithExtraInfo.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Unknown";
                panelWithExtraInfo.transform.GetChild(4).gameObject.GetComponent<Text>().text = "???";
                panelWithExtraInfo.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = curCreature.imageUnknown;
            }
        }
    }



    private void OpenCloseEncyclopedia()
    {
        isOpened = !isOpened;
        mainPanel.SetActive(isOpened);
        panelWithExtraInfo.SetActive(false);
        IsometricPlayerMovementController.IsAbleToMove = !isOpened;
    }


    private void ShowPlantStats (GameObject ChosenNote)
    {
        extraInfoPlantPanel.SetActive(true);
        extraInfoEnemyPanel.SetActive(false);
        GameObject lootPanel = extraInfoPlantPanel.transform.GetChild(1).gameObject;
        for (int i = 0; i < lootPanel.transform.childCount; i++)
        {
            Destroy(lootPanel.transform.GetChild(i).gameObject);
        }
        CreatureTypePlant temporary = ChosenNote.GetComponent<NotesManager>().creature as CreatureTypePlant;
        for (int i = 0; i < temporary.lootSprites.Count; i++)
        {
            GameObject tempObject = Instantiate(LootIcon);
            tempObject.GetComponent<Image>().sprite = temporary.lootSprites[i];
            tempObject.transform.GetChild(0).GetComponent<Text>().text = temporary.lootAmount[i].ToString();
            tempObject.transform.SetParent(lootPanel.transform);
        }
    }
    private void ShowEnemyStats(GameObject ChosenNote)
    {
        extraInfoEnemyPanel.SetActive(true);
        extraInfoPlantPanel.SetActive(false);
        GameObject specialAbilitiesPanel = extraInfoEnemyPanel.transform.GetChild(1).GetChild(2).gameObject;
        for (int i = 0; i < specialAbilitiesPanel.transform.childCount; i++)
        {
            Destroy(extraInfoEnemyPanel.transform.GetChild(1).GetChild(2).GetChild(i).gameObject);
        }
        CreatureTypeEnemy temporary = ChosenNote.GetComponent<NotesManager>().creature as CreatureTypeEnemy;
        extraInfoEnemyPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = temporary.healthPoints.ToString();
        extraInfoEnemyPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Text>().text = temporary.damagePoints.ToString();
        for (int i = 0; i < temporary.listOfSpecialAbilities.Count; i++)
        {
            GameObject tempObject = Instantiate(iconOfSpecialAbility);
            tempObject.GetComponent<Image>().sprite = temporary.listOfSpecialAbilities[i];
            tempObject.transform.SetParent(specialAbilitiesPanel.transform);

        }
    }

    private void ShowNewNoteNotification(CreaturesBase openedCreature)
    {
        GameObject notification = Instantiate(newNoteNotification, transform);
        notification.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = openedCreature.name;
        notification.transform.GetComponentsInChildren<Image>()[1].sprite = openedCreature.imageSmall;
    }

    public void OpenPlantsTab()
    {
        plantsTab.SetActive(true);
        plantsTabHeader.GetComponent<Image>().color = selectedTab;
        enemiesTab.SetActive(false);
        enemiesTabHeader.GetComponent<Image>().color = nonSelectedTab;
    }

    public void OpenEnemiesTab()
    {
        plantsTab.SetActive(false);
        plantsTabHeader.GetComponent<Image>().color = nonSelectedTab;
        enemiesTab.SetActive(true);
        enemiesTabHeader.GetComponent<Image>().color = selectedTab;
    }
}