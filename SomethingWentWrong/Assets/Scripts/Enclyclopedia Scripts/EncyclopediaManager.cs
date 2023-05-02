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
    private TMPro.TextMeshProUGUI extraInfoPlantHungerValue;
    private Image extraInfoPlantHungerImage;
    private TMPro.TextMeshProUGUI extraInfoPlantThirstValue;
    private Image extraInfoPlantThirstImage;
    private TMPro.TextMeshProUGUI extraInfoPlantOxigenValue;
    private Image extraInfoPlantOxigenImage;

    private Image extraInfoEnemyImage;
    private TMPro.TextMeshProUGUI extraInfoEnemyHpValue;
    private TMPro.TextMeshProUGUI extraInfoEnemyDamageValue;
    private TMPro.TextMeshProUGUI extraInfoEnemySpeedValue;
    private TMPro.TextMeshProUGUI extraInfoEnemyName;
    private TMPro.TextMeshProUGUI extraInfoEnemyDescription;

    private Dictionary<string, GameObject> notes;

    private ScrollRect scrollRectPlants;
    private ScrollRect scrollRectLore;

    private Image backgrounds;
    public Coroutine coroutineToStop = null;
    private Coroutine shadeCoroutineToStop = null;
    private GameObject[] allElemsToClose;
    //Queue<Coroutine> ShadingAnims = new Queue<Coroutine>();
    //Queue<Coroutine> DeShadingAnims = new Queue<Coroutine>();
    //Queue<Coroutine> OpeningAnims = new Queue<Coroutine>();
    ////Dictionary<openingAnims, Queue<Coroutine>> = new Dictionary<openingAnims, Queue<Coroutine>> 
    //Queue<Coroutine> ClosingAnims = new Queue<Coroutine>();

    Coroutine newNoteCoroutine;
    private Queue<CreaturesBase> notificationsToShowUp;
    private Image notificationImage;
    private TMPro.TextMeshProUGUI notificationHeader;
    [SerializeField] TMPro.TMP_ColorGradient firstGradient;
    [SerializeField] TMPro.TMP_ColorGradient secondGradient;

    [SerializeField] TMPro.TMP_ColorGradient PositiveGradient;
    [SerializeField] TMPro.TMP_ColorGradient NegativeGradient;

    //private Color32 selectedTab;
    //private Color32 nonSelectedTab;

    //[SerializeField] private List<NotesManager> enemiesNotes;
    //[SerializeField] private List<NotesManager> plantsNotes;


    [HideInInspector] public bool isOpened;
    public AudioClip[] Notes;
    public AudioClip Open;
    public AudioSource NotesSource;

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
        extraInfoPlantHungerValue = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(9).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoPlantHungerImage = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(8).GetComponent<Image>();
        extraInfoPlantThirstValue = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(11).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoPlantThirstImage = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(10).GetComponent<Image>();
        extraInfoPlantOxigenValue = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(13).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoPlantOxigenImage = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(12).GetComponent<Image>();

        extraInfoEnemyImage = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(0).GetComponent<Image>();
        extraInfoEnemyHpValue = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemyDamageValue = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemySpeedValue = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemyName = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(7).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemyDescription = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(8).GetComponent<TMPro.TextMeshProUGUI>();

        notificationImage = GM.UI.Encyclopedia.NewNoteNotification.transform.GetChild(2).GetComponent<Image>();
        notificationHeader = GM.UI.Encyclopedia.NewNoteNotification.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();

        scrollRectPlants = GM.UI.Encyclopedia.PlantsTab.GetComponent<ScrollRect>();
        scrollRectLore = GM.UI.Encyclopedia.LoreTab.GetComponent<ScrollRect>();

        allElemsToClose = new GameObject[] { transform.GetChild(0).gameObject,
                                             GM.UI.Encyclopedia.PlantsTab,
                                             GM.UI.Encyclopedia.ExtraInfoPlantPanel,
                                             GM.UI.Encyclopedia.EnemiesTab,
                                             GM.UI.Encyclopedia.ExtraInfoEnemyPanel,
                                             GM.UI.Encyclopedia.LoreTab,
                                             GM.UI.Encyclopedia.ExtraInfoLorePanel};

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
            CreatureTypePlant curCreature = curNotesManager.creature as CreatureTypePlant;
            if (GM.UI.Encyclopedia.ExtraInfoPlantPanel.activeSelf && extraInfoPlantName.text == curNotesManager.NameHeader)
            {
                coroutineToStop = StartCoroutine(AnimateClosingElement(GM.UI.Encyclopedia.ExtraInfoPlantPanel));
                return;
            }
            extraInfoPlantName.text = curCreature.name;
            extraInfoPlantDescription.text = curCreature.description;
            extraInfoPlantImage.sprite = curCreature.imageBig;
            extraInfoPlantHpValue.text = curNotesManager.hp.ToString();

            extraInfoPlantLootIcon.sprite = curNotesManager.lootSprite;
            extraInfoPlantLootValue.text = curNotesManager.lootAmount.ToString();

            if (curNotesManager.lootType == ItemType.Food)
            {
                extraInfoPlantHungerValue.gameObject.SetActive(true);
                extraInfoPlantHungerImage.gameObject.SetActive(true);
                extraInfoPlantThirstValue.gameObject.SetActive(true);
                extraInfoPlantThirstImage.gameObject.SetActive(true);
                extraInfoPlantOxigenValue.gameObject.SetActive(true);
                extraInfoPlantOxigenImage.gameObject.SetActive(true);

                extraInfoPlantHungerValue.text = curNotesManager.hungerReplenishment.ToString();
                SetGradientForValue(curNotesManager.hungerReplenishment, extraInfoPlantHungerValue);
                extraInfoPlantThirstValue.text = curNotesManager.thirstReplenishment.ToString();
                SetGradientForValue(curNotesManager.thirstReplenishment, extraInfoPlantThirstValue);
                extraInfoPlantOxigenValue.text = curNotesManager.oxigenReplenishment.ToString();
                SetGradientForValue(curNotesManager.oxigenReplenishment, extraInfoPlantOxigenValue);
            }
            else
            {
                extraInfoPlantHungerValue.gameObject.SetActive(false);
                extraInfoPlantHungerImage.gameObject.SetActive(false);
                extraInfoPlantThirstValue.gameObject.SetActive(false);
                extraInfoPlantThirstImage.gameObject.SetActive(false);
                extraInfoPlantOxigenValue.gameObject.SetActive(false);
                extraInfoPlantOxigenImage.gameObject.SetActive(false);
            }

            coroutineToStop = StartCoroutine(AnimateOpeningElement(GM.UI.Encyclopedia.ExtraInfoPlantPanel));
        }

        void OpenEnemy()
        {
            NotesManager curNotesManager = ChosenNote.GetComponent<NotesManager>();
            CreaturesBase curCreature = ChosenNote.GetComponent<NotesManager>().creature;
            if (GM.UI.Encyclopedia.ExtraInfoEnemyPanel.activeSelf && extraInfoEnemyName.text == curNotesManager.NameHeader)
            {
                coroutineToStop = StartCoroutine(AnimateClosingElement(GM.UI.Encyclopedia.ExtraInfoEnemyPanel));
                return;
            }
            extraInfoEnemyName.text = curCreature.name;
            extraInfoEnemyDescription.text = curCreature.description;
            extraInfoEnemyImage.sprite = curCreature.imageBig;
            extraInfoEnemyHpValue.text = curNotesManager.hp.ToString();

            extraInfoEnemyDamageValue.text = curNotesManager.damage.ToString();
            extraInfoEnemySpeedValue.text = curNotesManager.speed.ToString();

            coroutineToStop = StartCoroutine(AnimateOpeningElement(GM.UI.Encyclopedia.ExtraInfoEnemyPanel));
        }

        void SetGradientForValue(int value, TMPro.TextMeshProUGUI textToPaint)
        {
            if (value < 0)
                textToPaint.colorGradientPreset = NegativeGradient;
            else if (value > 0)
                textToPaint.colorGradientPreset = PositiveGradient;
            else
                textToPaint.colorGradientPreset = firstGradient;
        }
    }

    public void HideExtraInfo()
    {
        GM.UI.Encyclopedia.ExtraInfoPlantPanel.SetActive(false);
        GM.UI.Encyclopedia.ExtraInfoEnemyPanel.SetActive(false);
        GM.UI.Encyclopedia.ExtraInfoLorePanel.SetActive(false);
    }

    public void CloseLoreNote() => coroutineToStop = StartCoroutine(AnimateClosingElement(GM.UI.Encyclopedia.ExtraInfoLorePanel));


    public void OpenCloseEncyclopedia(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (GM.IsTutorial)
            return;
        isOpened = !isOpened;
        if (isOpened)
        {
            NotesSource.volume = 1;
            NotesSource.clip = Open;
            NotesSource.pitch = 1;
            NotesSource.Play();
            if (newNoteCoroutine != null)
            {
                StopCoroutine(newNoteCoroutine);
                StartCoroutine(AnimateClosingElement(GM.UI.Encyclopedia.NewNoteNotification));
                newNoteCoroutine = null;
            }
            Time.timeScale = 0f;
            foreach (GameObject elem in allElemsToClose)
                elem.transform.localScale = Vector3.zero;
            shadeCoroutineToStop = StartCoroutine(ShadeBackground());
            coroutineToStop = StartCoroutine(AnimateOpeningElement(transform.GetChild(0).gameObject));
            inputSystem.BlockPlayerInputs();
        }
        else
        {
            NotesSource.volume = 0;
            Time.timeScale = 1f;
            shadeCoroutineToStop = StartCoroutine(DeShadeBackground());
            HideExtraInfo();
            coroutineToStop = StartCoroutine(AnimateOpenCloseMultipleElement(allElemsToClose));
            inputSystem.UnblockPlayerInputs();
        }
    }

    private IEnumerator ShadeBackground()
    {
        if (shadeCoroutineToStop != null)
            StopCoroutine(shadeCoroutineToStop);
        while (backgrounds.color.a <= 0.75f)
        {
            backgrounds.color = new Color(backgrounds.color.r, backgrounds.color.g, backgrounds.color.b, backgrounds.color.a + 0.03f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        shadeCoroutineToStop = null;
    }
    private IEnumerator DeShadeBackground()
    {
        if (shadeCoroutineToStop != null)
            StopCoroutine(shadeCoroutineToStop);
        while (backgrounds.color.a > 0f)
        {
            backgrounds.color = new Color(backgrounds.color.r, backgrounds.color.g, backgrounds.color.b, backgrounds.color.a - 0.01f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        shadeCoroutineToStop = null;
    }

    public IEnumerator AnimateOpeningElement(GameObject element)
    {
        if (coroutineToStop != null)
            StopCoroutine(coroutineToStop);
        element.SetActive(true);
        while (element.transform.localScale.x <= 1)
        {
            element.transform.localScale += new Vector3(0.025f, 0.025f, 0);
            yield return new WaitForSecondsRealtime(0.005f);
        }
        coroutineToStop = null;
    }

    //public IEnumerator AnimateOpeningElement(GameObject element, ScrollRect scrollRect)
    //{
    //    if (coroutineToStop != null)
    //        StopCoroutine(coroutineToStop);
    //    element.SetActive(true);
    //    while (element.transform.localScale.x <= 1)
    //    {
    //        element.transform.localScale += new Vector3(0.025f, 0.025f, 0);
    //        yield return new WaitForSecondsRealtime(0.005f);
    //    }
    //    scrollRect.verticalNormalizedPosition = 0;
    //    coroutineToStop = null;
    //}

    private IEnumerator AnimateOpenCloseMultipleElement(GameObject[] elementsToOpen, GameObject[] elementsToClose)
    {
        if (coroutineToStop != null)
            StopCoroutine(coroutineToStop);
        foreach (GameObject elem in elementsToOpen)
            elem.SetActive(true);
        elementsToOpen[0].transform.GetChild(0).gameObject.SetActive(false);
        while (elementsToOpen[0].transform.localScale.x <= 1)
        {
            foreach (GameObject elem in elementsToOpen)
                elem.transform.localScale += new Vector3(0.025f, 0.025f, 0);
            foreach (GameObject elem in elementsToClose)
                if (elem.transform.localScale.x > 0)
                    elem.transform.localScale -= new Vector3(0.025f, 0.025f, 0);
            yield return new WaitForSecondsRealtime(0.005f);
        }
        foreach (GameObject elem in elementsToClose)
            elem.SetActive(false);
        elementsToOpen[0].transform.GetChild(0).gameObject.SetActive(true);
        coroutineToStop = null;
    }
    private IEnumerator AnimateOpenCloseMultipleElement(GameObject[] elementsToOpen, GameObject[] elementsToClose, ScrollRect scrollRect)
    {
        if (coroutineToStop != null)
            StopCoroutine(coroutineToStop);
        foreach (GameObject elem in elementsToOpen)
            elem.SetActive(true);
        elementsToOpen[0].transform.GetChild(0).gameObject.SetActive(false);
        while (elementsToOpen[0].transform.localScale.x <= 1)
        {
            foreach (GameObject elem in elementsToOpen)
                elem.transform.localScale += new Vector3(0.025f, 0.025f, 0);
            foreach (GameObject elem in elementsToClose)
                if (elem.transform.localScale.x > 0)
                    elem.transform.localScale -= new Vector3(0.025f, 0.025f, 0);
            yield return new WaitForSecondsRealtime(0.005f);
        }
        foreach (GameObject elem in elementsToClose)
            elem.SetActive(false);
        elementsToOpen[0].transform.GetChild(0).gameObject.SetActive(true);
        scrollRect.verticalNormalizedPosition = 1;
        coroutineToStop = null;
    }
    private IEnumerator AnimateOpenCloseMultipleElement(GameObject[] elementsToClose)
    {
        if (coroutineToStop != null)
            StopCoroutine(coroutineToStop);
        while (elementsToClose[0].transform.localScale.x > 0)
        {
            foreach (GameObject elem in elementsToClose)
                if (elem.transform.localScale.x > 0)
                    elem.transform.localScale -= new Vector3(0.025f, 0.025f, 0);
            yield return new WaitForSecondsRealtime(0.005f);
        }
        foreach (GameObject elem in elementsToClose)
            elem.SetActive(false);
        coroutineToStop = null;
    }
    private IEnumerator AnimateClosingElement(GameObject element)
    {
        if (coroutineToStop != null)
            StopCoroutine(coroutineToStop);
        while (element.transform.localScale.x > 0)
        {
            element.transform.localScale -= new Vector3(0.025f, 0.025f, 0);
            yield return new WaitForSecondsRealtime(0.005f);
        }
        element.SetActive(false);
        coroutineToStop = null;
    }

    private IEnumerator ShowNewNotification()
    {
        NotesSource.volume = 1;
        NotesSource.clip = Notes[Random.Range(0, Notes.Length)];
        NotesSource.pitch = 1 + Random.Range(-0.15f, 0.15f);
        NotesSource.Play();
        notificationImage.gameObject.SetActive(false);
        notificationHeader.gameObject.SetActive(false);
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
        coroutineToStop = StartCoroutine(AnimateOpenCloseMultipleElement(new GameObject[] { GM.UI.Encyclopedia.PlantsTab },
                                        new GameObject[] { GM.UI.Encyclopedia.LoreTab,
                                                           GM.UI.Encyclopedia.ExtraInfoLorePanel,
                                                           GM.UI.Encyclopedia.EnemiesTab,
                                                           GM.UI.Encyclopedia.ExtraInfoEnemyPanel}, scrollRectPlants));
        //GM.UI.Encyclopedia.PlantsTab.SetActive(true);
        //GM.UI.Encyclopedia.LoreTab.SetActive(false);
        //GM.UI.Encyclopedia.ExtraInfoLorePanel.SetActive(false);
        //GM.UI.Encyclopedia.EnemiesTab.SetActive(false);
        //GM.UI.Encyclopedia.ExtraInfoEnemyPanel.SetActive(false);
    }

    public void OpenEnemiesTab()
    {
        coroutineToStop = StartCoroutine(AnimateOpenCloseMultipleElement(new GameObject[] { GM.UI.Encyclopedia.EnemiesTab },
                                        new GameObject[] { GM.UI.Encyclopedia.LoreTab,
                                                           GM.UI.Encyclopedia.ExtraInfoLorePanel,
                                                           GM.UI.Encyclopedia.PlantsTab,
                                                           GM.UI.Encyclopedia.ExtraInfoPlantPanel}));
        //    GM.UI.Encyclopedia.PlantsTab.SetActive(false);
        //    GM.UI.Encyclopedia.ExtraInfoPlantPanel.SetActive(false);
        //    GM.UI.Encyclopedia.LoreTab.SetActive(false);
        //    GM.UI.Encyclopedia.ExtraInfoLorePanel.SetActive(false);
        //    GM.UI.Encyclopedia.EnemiesTab.SetActive(true);
    }

    public void OpenLoreTab()
    {
        coroutineToStop = StartCoroutine(AnimateOpenCloseMultipleElement(new GameObject[] { GM.UI.Encyclopedia.LoreTab },
                                        new GameObject[] { GM.UI.Encyclopedia.EnemiesTab,
                                                           GM.UI.Encyclopedia.ExtraInfoEnemyPanel,
                                                           GM.UI.Encyclopedia.PlantsTab,
                                                           GM.UI.Encyclopedia.ExtraInfoPlantPanel}, scrollRectLore));
        //GM.UI.Encyclopedia.PlantsTab.SetActive(false);
        //GM.UI.Encyclopedia.ExtraInfoPlantPanel.SetActive(false);
        //GM.UI.Encyclopedia.LoreTab.SetActive(true);
        //GM.UI.Encyclopedia.EnemiesTab.SetActive(false);
        //GM.UI.Encyclopedia.ExtraInfoEnemyPanel.SetActive(false);
    }
}
