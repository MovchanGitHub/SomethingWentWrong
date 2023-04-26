using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using static GameManager;
using UnityEngine.EventSystems;

public class EncyclopediaManager : MonoBehaviour
{
    private InputSystem inputSystem;

    //[SerializeField] private GameObject mainPanel;//
    //[SerializeField] private GameObject panelWithExtraInfo;//
    //[SerializeField] private GameObject extraInfoEnemyPanel;
    //[SerializeField] private GameObject extraInfoPlantPanel;
    //[SerializeField] private GameObject iconOfSpecialAbility;//
    //[SerializeField] private GameObject LootIcon;
    //[SerializeField] private GameObject newNoteNotification;
    //[SerializeField] private GameObject plantsTab;
    //[SerializeField] private GameObject enemiesTab;
    //[SerializeField] private GameObject plantsTabHeader;
    //[SerializeField] private GameObject enemiesTabHeader;

    private Image extraInfoPlantImage;
    private TMPro.TextMeshProUGUI extraInfoPlantHpValue;
    private TMPro.TextMeshProUGUI extraInfoPlantName;
    private TMPro.TextMeshProUGUI extraInfoPlantDescription;
    private Image extraInfoPlantLootIcon;
    private TMPro.TextMeshProUGUI extraInfoPlantLootValue;

    private Image extraInfoEnemyImage;
    private TMPro.TextMeshProUGUI extraInfoEnemyHpValue;
    private TMPro.TextMeshProUGUI extraInfoEnemyDamageValue;
    private TMPro.TextMeshProUGUI extraInfoEnemySpeedValue;
    private TMPro.TextMeshProUGUI extraInfoEnemyName;
    private TMPro.TextMeshProUGUI extraInfoEnemyDescription;

    private Dictionary<string, GameObject> notes;

    //private Color32 selectedTab;
    //private Color32 nonSelectedTab;

    //[SerializeField] private List<NotesManager> enemiesNotes;
    //[SerializeField] private List<NotesManager> plantsNotes;


    private bool isOpened;

    private void Awake()
    {
        isOpened = false;
        notes = new Dictionary<string, GameObject>();
        //selectedTab = new Color32(124, 192, 0, 255);
        //nonSelectedTab = new Color32(89, 137, 0, 255);
    }

    private void Start()
    {
        inputSystem = GM.InputSystem;

        extraInfoPlantImage = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(0).GetComponent<Image>();
        extraInfoPlantHpValue = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoPlantName = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoPlantDescription = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoPlantLootIcon = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(6).GetComponent<Image>();
        extraInfoPlantLootValue = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(7).GetComponent<TMPro.TextMeshProUGUI>();

        extraInfoEnemyImage = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(0).GetComponent<Image>();
        extraInfoEnemyHpValue = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemyDamageValue = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemySpeedValue = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemyName = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(7).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemyDescription = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(8).GetComponent<TMPro.TextMeshProUGUI>();

        InitializeEncyclopedia();
    }

    private void InitializeEncyclopedia()
    {
        Transform enemiesNotesMask = GM.UI.Encyclopedia.EnemiesTab.transform.GetChild(0);
        int childrenCount = enemiesNotesMask.childCount;
        for (int i = 0; i < childrenCount; i++)
        {
            notes.Add(enemiesNotesMask.GetChild(i).GetComponent<NotesManager>().creature.name, enemiesNotesMask.GetChild(i).gameObject);
            enemiesNotesMask.GetChild(i).GetComponent<NotesManager>().InitializeNote();
        }
        Transform plantsNotesMask = GM.UI.Encyclopedia.PlantsTab.transform.GetChild(0);
        childrenCount = plantsNotesMask.childCount;
        for (int i = 0; i < childrenCount; i++)
        {
            notes.Add(plantsNotesMask.GetChild(i).GetComponent<NotesManager>().creature.name, plantsNotesMask.GetChild(i).gameObject);
            plantsNotesMask.GetChild(i).GetComponent<NotesManager>().InitializeNote();
        }
    }

    public void OpenNewCreature(CreaturesBase openedCreature)
    {
        openedCreature.isOpenedInEcnyclopedia = true;
        //Debug.Log(openedCreature.name);
        //Debug.Log(notes[openedCreature.name]);
        NotesManager curNoteCode = notes[openedCreature.name].GetComponent<NotesManager>();
        curNoteCode.OpenUpInfoInNote();
        //ShowNewNoteNotification(openedCreature);
    }

    public void OpenExtraInfo(GameObject ChosenNote)
    {
        if (GM.UI.Encyclopedia.PlantsTab.activeSelf)
            OpenPlants();
        else
            OpenEnemy();
        //StartCoroutine(DeselectNote());



        //IEnumerator DeselectNote()
        //{
        //    yield return new WaitForEndOfFrame();
        //    EventSystem.current.SetSelectedGameObject(null);
        //}

        void OpenPlants()
        {

            NotesManager curNotesManager = ChosenNote.GetComponent<NotesManager>();
            CreatureTypePlant curCreature = ChosenNote.GetComponent<NotesManager>().creature as CreatureTypePlant;
            if (GM.UI.Encyclopedia.ExtraInfoPlantPanel.activeSelf && extraInfoPlantName.text == curNotesManager.NameHeader)
            {
                GM.UI.Encyclopedia.ExtraInfoPlantPanel.SetActive(false);
                return;
            }
            extraInfoPlantName.text = curCreature.name;
            extraInfoPlantDescription.text = curCreature.description;
            extraInfoPlantImage.sprite = curCreature.imageBig;
            extraInfoPlantHpValue.text = curNotesManager.hp.ToString();

            extraInfoPlantLootIcon.sprite = curNotesManager.lootSprite;
            extraInfoPlantLootValue.text = curNotesManager.lootAmount.ToString();

            GM.UI.Encyclopedia.ExtraInfoPlantPanel.SetActive(true);
        }

        void OpenEnemy()
        {
            NotesManager curNotesManager = ChosenNote.GetComponent<NotesManager>();
            CreaturesBase curCreature = ChosenNote.GetComponent<NotesManager>().creature;
            if (GM.UI.Encyclopedia.ExtraInfoEnemyPanel.activeSelf && extraInfoEnemyName.text == curNotesManager.NameHeader)
            {
                GM.UI.Encyclopedia.ExtraInfoEnemyPanel.SetActive(false);
                return;
            }
            extraInfoEnemyName.text = curCreature.name;
            extraInfoEnemyDescription.text = curCreature.description;
            extraInfoEnemyImage.sprite = curCreature.imageBig;
            extraInfoEnemyHpValue.text = curNotesManager.hp.ToString();

            extraInfoEnemyDamageValue.text = curNotesManager.damage.ToString();
            extraInfoEnemySpeedValue.text = curNotesManager.speed.ToString();

            GM.UI.Encyclopedia.ExtraInfoEnemyPanel.SetActive(true);
        }
    }

    public void HideExtraInfo()
    {
        GM.UI.Encyclopedia.ExtraInfoPlantPanel.SetActive(false);
        GM.UI.Encyclopedia.ExtraInfoEnemyPanel.SetActive(false);
    }



    public void OpenCloseEncyclopedia(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isOpened = !isOpened;
        GetComponent<Image>().enabled = isOpened;               //redo after getting parent for EncyclopediaPanel
        transform.GetChild(0).gameObject.SetActive(isOpened);
        //transform.GetChild(1).gameObject.SetActive(isOpened);
        HideExtraInfo();
        if (isOpened)
            inputSystem.BlockPlayerInputs();
        else
            inputSystem.UnblockPlayerInputs();

        //GameManager.GM.PlayerMovement.IsAbleToMove = !isOpened;
    }


    //public void ShowPlantStats (GameObject ChosenNote)
    //{
    //    extraInfoPlantPanel.SetActive(true);
    //    extraInfoEnemyPanel.SetActive(false);
    //    GameObject lootPanel = extraInfoPlantPanel.transform.GetChild(1).gameObject;
    //    for (int i = 0; i < lootPanel.transform.childCount; i++)
    //    {
    //        Destroy(lootPanel.transform.GetChild(i).gameObject);
    //    }
    //    CreatureTypePlant temporary = ChosenNote.GetComponent<NotesManager>().creature as CreatureTypePlant;
    //    for (int i = 0; i < temporary.lootSprites.Count; i++)
    //    {
    //        GameObject tempObject = Instantiate(LootIcon);
    //        tempObject.GetComponent<Image>().sprite = temporary.lootSprites[i];
    //        tempObject.transform.GetChild(0).GetComponent<Text>().text = temporary.lootAmount[i].ToString();
    //        tempObject.transform.SetParent(lootPanel.transform);
    //    }
    //}
    //public void ShowEnemyStats(GameObject ChosenNote)
    //{
    //    extraInfoEnemyPanel.SetActive(true);
    //    extraInfoPlantPanel.SetActive(false);
    //    GameObject specialAbilitiesPanel = extraInfoEnemyPanel.transform.GetChild(1).GetChild(2).gameObject;
    //    for (int i = 0; i < specialAbilitiesPanel.transform.childCount; i++)
    //    {
    //        Destroy(extraInfoEnemyPanel.transform.GetChild(1).GetChild(2).GetChild(i).gameObject);
    //    }
    //    CreatureTypeEnemy temporary = ChosenNote.GetComponent<NotesManager>().creature as CreatureTypeEnemy;
    //    extraInfoEnemyPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = temporary.healthPoints.ToString();
    //    extraInfoEnemyPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Text>().text = temporary.damagePoints.ToString();
    //    for (int i = 0; i < temporary.listOfSpecialAbilities.Count; i++)
    //    {
    //        GameObject tempObject = Instantiate(iconOfSpecialAbility);
    //        tempObject.GetComponent<Image>().sprite = temporary.listOfSpecialAbilities[i];
    //        tempObject.transform.SetParent(specialAbilitiesPanel.transform);

    //    }
    //}

    //private void ShowNewNoteNotification(CreaturesBase openedCreature)
    //{
    //    GameObject notification = Instantiate(newNoteNotification, transform);
    //    notification.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = openedCreature.name;
    //    notification.transform.GetComponentsInChildren<Image>()[1].sprite = openedCreature.imageSmall;
    //}

    public void OpenPlantsTab()
    {
        GM.UI.Encyclopedia.PlantsTab.SetActive(true);
        //plantsTabHeader.GetComponent<Image>().color = selectedTab;
        GM.UI.Encyclopedia.EnemiesTab.SetActive(false);
        GM.UI.Encyclopedia.ExtraInfoEnemyPanel.SetActive(false);
        //enemiesTabHeader.GetComponent<Image>().color = nonSelectedTab;
    }

    public void OpenEnemiesTab()
    {
        GM.UI.Encyclopedia.PlantsTab.SetActive(false);
        //plantsTabHeader.GetComponent<Image>().color = nonSelectedTab;
        GM.UI.Encyclopedia.EnemiesTab.SetActive(true);
        GM.UI.Encyclopedia.ExtraInfoPlantPanel.SetActive(false);
        //enemiesTabHeader.GetComponent<Image>().color = selectedTab;
    }
}