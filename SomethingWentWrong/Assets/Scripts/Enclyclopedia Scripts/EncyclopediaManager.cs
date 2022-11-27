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
    //[SerializeField] private GameObject extraInfoPlantPanel;
    [SerializeField] private GameObject notesOfCreatures;
    [SerializeField] private GameObject iconOfSpecialAbility;

    private bool isOpened;

    private void Start()
    {
        isOpened = false;
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
        //Debug.Log("Start");
        // for (int i = 0; i < notesOfCreatures.transform.childCount; i++)
        // {
        //     //Debug.Log(i);
        //     if (notesOfCreatures.transform.GetChild(i).GetComponent<NotesManager>().creature == AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Creatures/Some unknown thing.asset", typeof(CreaturesBase)))
        //     {
        //         //Debug.Log("Contact");
        //         GameObject CurrentNote = notesOfCreatures.transform.GetChild(i).gameObject;
        //         CurrentNote.GetComponent<NotesManager>().creature = openedCreature;
        //         CurrentNote.GetComponent<NotesManager>().nameHeader.GetComponent<Text>().text = openedCreature.name;
        //         CurrentNote.GetComponent<NotesManager>().icon.GetComponent<Image>().sprite = openedCreature.imageSmall;
        //         break;
        //     }
        // }
    }

    public void OpenExtraInfo(GameObject ChosenNote)
    {
        //Second time on the same Note?
        if (panelWithExtraInfo.activeSelf == true && panelWithExtraInfo.transform.GetChild(2).gameObject.GetComponent<Text>().text == ChosenNote.GetComponent<NotesManager>().creature.name)
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

            if (ChosenNote.GetComponent<NotesManager>().creature.typeOfThisCreature == creatureType.Enemy)
            {
                ShowEnemyStats(ChosenNote);
            }
            else
            {
                extraInfoEnemyPanel.SetActive(false);
            }

            panelWithExtraInfo.transform.GetChild(2).gameObject.GetComponent<Text>().text = ChosenNote.GetComponent<NotesManager>().creature.name;
            panelWithExtraInfo.transform.GetChild(3).gameObject.GetComponent<Text>().text = ChosenNote.GetComponent<NotesManager>().creature.description;
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

    private void ShowEnemyStats(GameObject ChosenNote)
    {
        extraInfoEnemyPanel.SetActive(true);
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

}