using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EncyclopediaManager : MonoBehaviour
{
    [SerializeField] private static MonoBehaviour MonBehaviour;
    [SerializeField] private MonoBehaviour MonBehaviourNotStatic;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject panelWithExtraInfo;
    [SerializeField] private GameObject extraInfoEnemyPanel;
    [SerializeField] private GameObject extraInfoPlantPanel;
    [SerializeField] private GameObject notesOfCreatures;
    private static GameObject notesOfCreaturesStatic;
    private static int amountOfNotes;
    [SerializeField] private GameObject iconOfSpecialAbility;
    [SerializeField] private GameObject LootIcon;
    [SerializeField] private CreaturesBase noCreatureNotStatic;
    [SerializeField] private GameObject newNoteNotificationNotStatic;
    private static GameObject newNoteNotification;
    private static CreaturesBase noCreature;

    private bool isOpened;

    private void Start()
    {
        isOpened = false;
        notesOfCreaturesStatic = notesOfCreatures;
        amountOfNotes = notesOfCreatures.transform.childCount;
        noCreature = noCreatureNotStatic;
        newNoteNotification = newNoteNotificationNotStatic;
        MonBehaviour = MonBehaviourNotStatic;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenCloseEncyclopedia();
            
        }
    }

    public static void OpenNewCreature(CreaturesBase openedCreature)
    {
        Debug.Log("start");
        for (int i = 0; i < amountOfNotes; i++)
        {
            if (notesOfCreaturesStatic.transform.GetChild(i).GetComponent<NotesManager>().creature == noCreature)
            {
                Debug.Log("Contact");
                GameObject currentNote = notesOfCreaturesStatic.transform.GetChild(i).gameObject;
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

    private static void ShowNewNoteNotification(CreaturesBase openedCreature)
    {
        newNoteNotification.transform.GetChild(1).GetComponent<Image>().sprite = openedCreature.imageSmall;
        newNoteNotification.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = openedCreature.name;
        Debug.Log(newNoteNotification.transform.position);
        newNoteNotification.transform.position = new Vector2(952, 450);
        newNoteNotification.SetActive(true);
        MonBehaviour.StartCoroutine(WaitForNewNoteDissapear());
        //newNoteNotification.transform.position = Vector2.MoveTowards(new Vector2(1262, 450), new Vector2(952, 450), 0.0001f);
    }

    //static IEnumerator ShowNewNoteNotification(CreaturesBase openedCreature)
    //{
    //    Debug.Log('f');
    //    newNoteNotification.transform.GetChild(1).GetComponent<Image>().sprite = openedCreature.imageSmall;
    //    newNoteNotification.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = openedCreature.name;
    //    Debug.Log(newNoteNotification.transform.position);
    //    //newNoteNotification.transform.position = new Vector2(952, 450);
    //    newNoteNotification.SetActive(true);
    //    while (newNoteNotification.transform.position != new Vector3(952, 450))
    //    {
    //        newNoteNotification.transform.position = Vector2.MoveTowards(new Vector2(1262, 450), new Vector2(952, 450), 0.1f);
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    MonBehaviour.StartCoroutine(WaitForNewNoteDissapear());
    //    //newNoteNotification.transform.position = new Vector2(952, 450);
    //}

    static IEnumerator WaitForNewNoteDissapear()
    {
        Debug.Log("End");
        yield return new WaitForSeconds(4.0f);
        Debug.Log("End");
        newNoteNotification.SetActive(false);
    }

}