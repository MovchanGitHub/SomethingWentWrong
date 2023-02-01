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
    [SerializeField] private GameObject notesOfCreatures;
    private static int amountOfNotes;
    [SerializeField] private GameObject iconOfSpecialAbility;
    [SerializeField] private GameObject LootIcon;
    [SerializeField] private CreaturesBase noCreature;
    [SerializeField] private GameObject newNoteNotification;
    [SerializeField] private GameObject plantTab;
    [SerializeField] private GameObject enemiesTab;

    private bool isOpened;

    private void Start()
    {
        isOpened = false;
        amountOfNotes = notesOfCreatures.transform.childCount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenCloseEncyclopedia();
            
        }
    }

    public void OpenNewCreature(CreaturesBase openedCreature)
    {
        Debug.Log("start");
        //Debug.Log(amountOfNotes);
        //Debug.Log(openedCreature);
        for (int i = 0; i < amountOfNotes; i++)
        {
            //Debug.Log(notesOfCreatures.transform.GetChild(i).GetComponent<NotesManager>().creature);
            Debug.Log(noCreature);
            if (notesOfCreatures.transform.GetChild(i).GetComponent<NotesManager>().creature == noCreature)
            {
                Debug.Log("Contact");
                GameObject currentNote = notesOfCreatures.transform.GetChild(i).gameObject;
                NotesManager notesManagerCode = currentNote.GetComponent<NotesManager>();
                notesManagerCode.creature = openedCreature;
                notesManagerCode.nameHeader.GetComponent<Text>().text = openedCreature.name;
                notesManagerCode.icon.GetComponent<Image>().sprite = openedCreature.imageSmall;

                ShowNewNoteNotification(openedCreature);
                //MonBehaviour.StartCoroutine(ShowNewNoteNotification(openedCreature));


                openedCreature.isOpenedInEcnyclopedia = true;
                break;
            }
        }
    }

    public void OpenExtraInfo(GameObject ChosenNote)
    {
        //Second time on the same Note?
        if (panelWithExtraInfo.activeSelf == true && panelWithExtraInfo.transform.GetChild(3).gameObject.GetComponent<Text>().text == ChosenNote.GetComponent<NotesManager>().creature.name)
        {
            panelWithExtraInfo.SetActive(false);
        }
        //Common case - opening the extraInfo
        else
        {
            panelWithExtraInfo.SetActive(true);

            for (int i = 0; i < extraInfoEnemyPanel.transform.GetChild(1).GetChild(2).childCount; i++)
            {
                Destroy(extraInfoEnemyPanel.transform.GetChild(1).GetChild(2).GetChild(i).gameObject);
            }
            for (int i = 0; i < extraInfoPlantPanel.transform.GetChild(1).childCount; i++)
            {
                Destroy(extraInfoPlantPanel.transform.GetChild(1).GetChild(i).gameObject);
            }

            if (ChosenNote.GetComponent<NotesManager>().creature.typeOfThisCreature == creatureType.Enemy)
            {
                ShowEnemyStats(ChosenNote);
            }
            else
            {
                ShowPlantStats(ChosenNote);
            }

            panelWithExtraInfo.transform.GetChild(3).gameObject.GetComponent<Text>().text = ChosenNote.GetComponent<NotesManager>().creature.name;
            panelWithExtraInfo.transform.GetChild(4).gameObject.GetComponent<Text>().text = ChosenNote.GetComponent<NotesManager>().creature.description;
            panelWithExtraInfo.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ChosenNote.GetComponent<NotesManager>().creature.imageBig;
        }
    }



    private void OpenCloseEncyclopedia()
    {
        isOpened = !isOpened;
        mainPanel.SetActive(isOpened);
        panelWithExtraInfo.SetActive(false);
        IsometricPlayerMovementController.IsAbleToMove = !IsometricPlayerMovementController.IsAbleToMove;
    }


    private void ShowPlantStats (GameObject ChosenNote)
    {
        extraInfoPlantPanel.SetActive(true);
        extraInfoEnemyPanel.SetActive(false);
        CreatureTypePlant temporary = ChosenNote.GetComponent<NotesManager>().creature as CreatureTypePlant;
        for (int i = 0; i < temporary.lootSprites.Count; i++)
        {
            GameObject tempObject = Instantiate(LootIcon);
            tempObject.GetComponent<Image>().sprite = temporary.lootSprites[i];
            tempObject.transform.GetChild(0).GetComponent<Text>().text = temporary.lootAmount[i].ToString();
            tempObject.transform.SetParent(extraInfoPlantPanel.transform.GetChild(1));
        }
    }
    private void ShowEnemyStats(GameObject ChosenNote)
    {
        extraInfoEnemyPanel.SetActive(true);
        extraInfoPlantPanel.SetActive(false);
        //Debug.Log(ChosenNote.GetComponent<NotesManager>().creature.GetType());
        CreatureTypeEnemy temporary = ChosenNote.GetComponent<NotesManager>().creature as CreatureTypeEnemy;
        extraInfoEnemyPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = temporary.healthPoints.ToString();
        extraInfoEnemyPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Text>().text = temporary.damagePoints.ToString();
        for (int i = 0; i < temporary.listOfSpecialAbilities.Count; i++)
        {
            GameObject tempObject = Instantiate(iconOfSpecialAbility);
            tempObject.GetComponent<Image>().sprite = temporary.listOfSpecialAbilities[i];
            tempObject.transform.SetParent(extraInfoEnemyPanel.transform.GetChild(1).GetChild(2));

        }
    }

    private void ShowNewNoteNotification(CreaturesBase openedCreature)
    {
        newNoteNotification.transform.GetChild(1).GetComponent<Image>().sprite = openedCreature.imageSmall;
        newNoteNotification.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = openedCreature.name;
        newNoteNotification.SetActive(true);
        newNoteNotification.GetComponent<Animator>().Play("EncyclopediaNotificatonShowUp");
    }

    public void OpenPlantsTab()
    {
        plantTab.SetActive(true);
        enemiesTab.SetActive(false);
    }

    public void OpenEnemiesTab()
    {
        plantTab.SetActive(false);
        enemiesTab.SetActive(true);
    }
}