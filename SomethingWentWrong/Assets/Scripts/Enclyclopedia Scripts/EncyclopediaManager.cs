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

    Image backgrounds;
    Queue<Coroutine> ShadingAnims = new Queue<Coroutine>();
    Queue<Coroutine> DeShadingAnims = new Queue<Coroutine>();
    Queue<Coroutine> OpeningAnims = new Queue<Coroutine>();
    //Dictionary<openingAnims, Queue<Coroutine>> = new Dictionary<openingAnims, Queue<Coroutine>> 
    Queue<Coroutine> ClosingAnims = new Queue<Coroutine>();

    Coroutine newNoteCoroutine;
    private Queue<CreaturesBase> notificationsToShowUp;
    private Image notificationImage;
    private TMPro.TextMeshProUGUI notificationHeader;
    [SerializeField] TMPro.TMP_ColorGradient firstGradient;
    [SerializeField] TMPro.TMP_ColorGradient secondGradient;

    //private Color32 selectedTab;
    //private Color32 nonSelectedTab;

    //[SerializeField] private List<NotesManager> enemiesNotes;
    //[SerializeField] private List<NotesManager> plantsNotes;


    private bool isOpened;

    private void Awake()
    {
        isOpened = false;
        notes = new Dictionary<string, GameObject>();
        backgrounds = GetComponent<Image>();
        notificationsToShowUp = new Queue<CreaturesBase>();
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

        notificationImage = GM.UI.Encyclopedia.NewNoteNotification.transform.GetChild(2).GetComponent<Image>();
        notificationHeader =  GM.UI.Encyclopedia.NewNoteNotification.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();

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
        if (GM.IsTutorial)
            return;
        openedCreature.isOpenedInEcnyclopedia = true;
        NotesManager curNoteCode = notes[openedCreature.name].GetComponent<NotesManager>();
        curNoteCode.OpenUpInfoInNote();
        notificationsToShowUp.Enqueue(openedCreature);
        Debug.Log(notificationsToShowUp.Count);
        Debug.Log(newNoteCoroutine);
        if (newNoteCoroutine == null)
            newNoteCoroutine = StartCoroutine(ShowNewNotification());
    }

    public void OpenExtraInfo(GameObject ChosenNote)
    {
        if (GM.UI.Encyclopedia.PlantsTab.activeSelf)
            OpenPlants();
        else
            OpenEnemy();

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
        GM.UI.Encyclopedia.ExtraInfoLorePanel.SetActive(false);
    }

    public void CloseLoreNote() => GM.UI.Encyclopedia.ExtraInfoLorePanel.SetActive(false);


    public void OpenCloseEncyclopedia(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (GM.IsTutorial)
            return;
        isOpened = !isOpened;
        //Time.timeScale = isOpened ? 0f : 1f;
        //GetComponent<Image>().enabled = isOpened;
        //transform.GetChild(0).gameObject.SetActive(isOpened);
        //HideExtraInfo();
        //if (isOpened)
        //    inputSystem.BlockPlayerInputs();
        //else
        //    inputSystem.UnblockPlayerInputs();
        if (isOpened)
        {
            if (newNoteCoroutine != null)
            {
                StopCoroutine(newNoteCoroutine);
                StartCoroutine(AnimateClosingElement(GM.UI.Encyclopedia.NewNoteNotification));
                newNoteCoroutine = null;
            }
            Time.timeScale = 0f;
            ShadingAnims.Enqueue(StartCoroutine(ShadeBackground()));
            OpeningAnims.Enqueue(StartCoroutine(AnimateOpeningElement(transform.GetChild(0).gameObject)));
            inputSystem.BlockPlayerInputs();
        }
        else
        {
            Time.timeScale = 1f;
            DeShadingAnims.Enqueue(StartCoroutine(DeShadeBackground()));
            HideExtraInfo();
            ClosingAnims.Enqueue(StartCoroutine(AnimateClosingElement(transform.GetChild(0).gameObject)));
            inputSystem.UnblockPlayerInputs();
        }
    }

    private IEnumerator ShadeBackground()
    {
        if (DeShadingAnims.Count != 0)
            StopCoroutine(DeShadingAnims.Dequeue());
        //backgrounds.enabled = true;
        while (backgrounds.color.a <= 0.75f)
        {
            backgrounds.color = new Color(backgrounds.color.r, backgrounds.color.g, backgrounds.color.b, backgrounds.color.a + 0.06f);
            //Debug.Log(backgrounds.color.a);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    private IEnumerator DeShadeBackground()
    {
        if (ShadingAnims.Count != 0)
            StopCoroutine(ShadingAnims.Dequeue());
        while (backgrounds.color.a > 0f)
        {
            backgrounds.color = new Color(backgrounds.color.r, backgrounds.color.g, backgrounds.color.b, backgrounds.color.a - 0.01f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        //backgrounds.enabled = false;
    }

    private IEnumerator AnimateOpeningElement(GameObject element)
    {
        if (ClosingAnims.Count != 0)
            StopCoroutine(ClosingAnims.Dequeue());
        element.SetActive(true);
        while (element.transform.localScale.x <= 1)
        {
            element.transform.localScale += new Vector3(0.025f, 0.025f, 0);
            //Debug.Log(element.transform.localScale.x);
            yield return new WaitForSecondsRealtime(0.005f);
            //Debug.Log(element.transform.localScale.x <= 1);
        }
    }
    private IEnumerator AnimateClosingElement(GameObject element)
    {
        if (OpeningAnims.Count != 0)
            StopCoroutine(OpeningAnims.Dequeue());
        while (element.transform.localScale.x > 0)
        {
            element.transform.localScale -= new Vector3(0.025f, 0.025f, 0);
            yield return new WaitForSecondsRealtime(0.005f);
        }
        element.SetActive(false);
    }

    private IEnumerator ShowNewNotification()
    {
        notificationImage.gameObject.SetActive(true);
        notificationHeader.gameObject.SetActive(true);
        GM.UI.Encyclopedia.NewNoteNotification.SetActive(true);
        while (GM.UI.Encyclopedia.NewNoteNotification.transform.localScale.x <= 1)
        {
            GM.UI.Encyclopedia.NewNoteNotification.transform.localScale += new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.03f);
        }
        notificationImage.gameObject.SetActive(true);
        notificationHeader.gameObject.SetActive(true);
        while (notificationsToShowUp.Count != 0)
        {
            CreaturesBase curCreature = notificationsToShowUp.Dequeue();
            notificationImage.sprite = curCreature.imageSmall;
            for (int i = 0; i < 4; i++)
            {
                notificationHeader.colorGradientPreset = firstGradient;
                yield return new WaitForSeconds(0.5f);
                notificationHeader.colorGradientPreset = secondGradient;
                yield return new WaitForSeconds(0.5f);
            }
        }
        while (GM.UI.Encyclopedia.NewNoteNotification.transform.localScale.x >= 0.01)
        {
            GM.UI.Encyclopedia.NewNoteNotification.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.03f);
        }
        notificationImage.gameObject.SetActive(false);
        notificationHeader.gameObject.SetActive(false);
        GM.UI.Encyclopedia.NewNoteNotification.SetActive(false);
        newNoteCoroutine = null;
    }

    //private void ShowNewNoteNotification(CreaturesBase openedCreature)
    //{
    //    GameObject notification = Instantiate(newNoteNotification, transform);
    //    notification.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = openedCreature.name;
    //    notification.transform.GetComponentsInChildren<Image>()[1].sprite = openedCreature.imageSmall;
    //}

    public void OpenPlantsTab()
    {
        GM.UI.Encyclopedia.PlantsTab.SetActive(true);
        GM.UI.Encyclopedia.LoreTab.SetActive(false);
        GM.UI.Encyclopedia.ExtraInfoLorePanel.SetActive(false);
        GM.UI.Encyclopedia.EnemiesTab.SetActive(false);
        GM.UI.Encyclopedia.ExtraInfoEnemyPanel.SetActive(false);
    }

    public void OpenEnemiesTab()
    {
        GM.UI.Encyclopedia.PlantsTab.SetActive(false);
        GM.UI.Encyclopedia.ExtraInfoPlantPanel.SetActive(false);
        GM.UI.Encyclopedia.LoreTab.SetActive(false);
        GM.UI.Encyclopedia.ExtraInfoLorePanel.SetActive(false);
        GM.UI.Encyclopedia.EnemiesTab.SetActive(true);
    }

    public void OpenLoreTab()
    {

        GM.UI.Encyclopedia.PlantsTab.SetActive(false);
        GM.UI.Encyclopedia.ExtraInfoPlantPanel.SetActive(false);
        GM.UI.Encyclopedia.LoreTab.SetActive(true);
        GM.UI.Encyclopedia.EnemiesTab.SetActive(false);
        GM.UI.Encyclopedia.ExtraInfoEnemyPanel.SetActive(false);
    }
}