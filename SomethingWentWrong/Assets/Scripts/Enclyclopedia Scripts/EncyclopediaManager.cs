using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditor;
using static GameManager;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class EncyclopediaManager : MonoBehaviour
{
    private InputSystem inputSystem;

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
    private TMPro.TextMeshProUGUI extraInfoPlantHpRecoveryValue;
    private Image extraInfoPlantHpRecoveryImage;

    private Image extraInfoEnemyImage;
    private TMPro.TextMeshProUGUI extraInfoEnemyHpValue;
    private TMPro.TextMeshProUGUI extraInfoEnemyDamageValue;
    private TMPro.TextMeshProUGUI extraInfoEnemySpeedValue;
    private TMPro.TextMeshProUGUI extraInfoEnemyName;
    private TMPro.TextMeshProUGUI extraInfoEnemyDescription;

    public AspectRatioFitter aspectRatioFitter;

    private Dictionary<string, GameObject> notes;

    private ScrollRect scrollRectPlants;
    private ScrollRect scrollRectLore;

    private Image backgrounds;
    public Coroutine coroutineToStop = null;
    private Coroutine shadeCoroutineToStop = null;
    private GameObject[] allElemsToClose; 

    Coroutine newNoteCoroutine;
    private Queue<CreaturesBase> notificationsToShowUp;
    private Image notificationImage;
    private TMPro.TextMeshProUGUI notificationHeader;
    private CreaturesBase notificationCreature;
    [SerializeField] TMPro.TMP_ColorGradient firstGradient;
    [SerializeField] TMPro.TMP_ColorGradient secondGradient;

    [SerializeField] TMPro.TMP_ColorGradient PositiveGradient;
    [SerializeField] TMPro.TMP_ColorGradient NegativeGradient;


    [HideInInspector] public bool isOpened;
    public AudioClip[] Notes;
    public AudioClip Open;
    public AudioSource NotesSource;

    [SerializeField] private GameObject loreNotePrefab;
    [SerializeField] private CreaturesBase playerScriptableObject;

    string pathForSaves;
    Dictionary<string, bool> boolSaves = new Dictionary<string, bool>();
    BinaryFormatter formatter = new BinaryFormatter();

    private void Awake()
    {
        isOpened = false;
        notes = new Dictionary<string, GameObject>();
        backgrounds = GetComponent<Image>();
        aspectRatioFitter = transform.GetChild(0).GetComponent<AspectRatioFitter>();
        notificationsToShowUp = new Queue<CreaturesBase>();

        pathForSaves = Application.persistentDataPath + "/encyclopediaBools.gamesave";
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
        extraInfoPlantHpRecoveryValue = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(15).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoPlantHpRecoveryImage = GM.UI.Encyclopedia.ExtraInfoPlantPanel.transform.GetChild(14).GetComponent<Image>();

        extraInfoEnemyImage = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(0).GetComponent<Image>();
        extraInfoEnemyHpValue = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemyDamageValue = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemySpeedValue = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemyName = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(7).GetComponent<TMPro.TextMeshProUGUI>();
        extraInfoEnemyDescription = GM.UI.Encyclopedia.ExtraInfoEnemyPanel.transform.GetChild(8).GetComponent<TMPro.TextMeshProUGUI>();

        notificationImage = GM.UI.Encyclopedia.NewNoteNotification.transform.GetChild(2).GetComponent<Image>();
        notificationHeader = GM.UI.Encyclopedia.NewNoteNotification.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        notificationCreature = GM.UI.Encyclopedia.NewNoteNotification.GetComponent<NewNoteNotificationFastEncOpen>().openedCreature;


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
            var temp = enemiesNotesMask.GetChild(i).GetComponent<NotesManager>();
            notes.Add(temp.creature.name, enemiesNotesMask.GetChild(i).gameObject);
            boolSaves.Add(temp.creature.name, false);
            temp.InitializeNote();
        }
        Transform plantsNotesMask = GM.UI.Encyclopedia.PlantsTab.transform.GetChild(0);
        childrenCount = plantsNotesMask.childCount;
        for (int i = 0; i < childrenCount; i++)
        {
            var temp = plantsNotesMask.GetChild(i).GetComponent<NotesManager>();
            notes.Add(temp.creature.name, plantsNotesMask.GetChild(i).gameObject);
            boolSaves.Add(temp.creature.name, false);
            temp.InitializeNote();
        }

        if (File.Exists(pathForSaves))
            LoadBoolInfo();
        else
            SaveBoolInfo();

        //Debug.Log(boolSaves["ты"]);
        if (boolSaves.ContainsKey("ты") && boolSaves["ты"])
            OpenPlayerInEncyclopedia();
    }

    public void OpenNewCreature(CreaturesBase openedCreature)
    {
        if (GM.IsTutorial)
            return;
        boolSaves[openedCreature.name] = true;
        SaveBoolInfo();
        openedCreature.isOpenedInEcnyclopedia = true;
        NotesManager curNoteCode = notes[openedCreature.name].GetComponent<NotesManager>();
        curNoteCode.OpenUpInfoInNote();
        notificationsToShowUp.Enqueue(openedCreature);
        if (newNoteCoroutine == null)
            newNoteCoroutine = StartCoroutine(ShowNewNotification());
    }

    public IEnumerator GoToNewCreatureCoroutine (CreaturesBase openedCreature)
    {
        yield return null;
        //OpenCloseEncyclopedia();
        //while (coroutineToStop != null)
        //{
        //    Debug.Log(55);
        //    yield return new WaitForSecondsRealtime(0.2f);
        //    Debug.Log(coroutineToStop);
        //}
        //Debug.Log(0);
        //yield return new WaitUntil(() => coroutineToStop == null);
        //Debug.Log(1);
        //OpenPlantsTab();
        //yield return new WaitUntil(() => coroutineToStop == null);
        //Debug.Log(2);
        //OpenExtraInfo(notes[openedCreature.name]);
        //Debug.Log(3);
    }

    public void OpenExtraInfo(GameObject ChosenNote)
    {
        if (GM.UI.Encyclopedia.PlantsTab.activeSelf)
            OpenPlants();
        else if (GM.UI.Encyclopedia.EnemiesTab.activeSelf)
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
                extraInfoPlantHpRecoveryValue.gameObject.SetActive(true);
                extraInfoPlantHpRecoveryImage.gameObject.SetActive(true);

                string temp;
                if (curNotesManager.hungerReplenishment > 0)
                    temp = "+" + curNotesManager.hungerReplenishment.ToString();
                else
                    temp = curNotesManager.hungerReplenishment.ToString();
                extraInfoPlantHungerValue.text = temp;
                //SetGradientForValue(curNotesManager.hungerReplenishment, extraInfoPlantHungerValue);
                if (curNotesManager.thirstReplenishment > 0)
                    temp = "+" + curNotesManager.thirstReplenishment.ToString();
                else
                    temp = curNotesManager.thirstReplenishment.ToString();
                extraInfoPlantThirstValue.text = temp;
                //SetGradientForValue(curNotesManager.thirstReplenishment, extraInfoPlantThirstValue);
                if (curNotesManager.oxigenReplenishment > 0)
                    temp = "+" + curNotesManager.oxigenReplenishment.ToString();
                else
                    temp = curNotesManager.oxigenReplenishment.ToString();
                extraInfoPlantOxigenValue.text = temp;
                //SetGradientForValue(curNotesManager.oxigenReplenishment, extraInfoPlantOxigenValue);
                if (curNotesManager.hpReplenishment > 0)
                    temp = "+" + curNotesManager.hpReplenishment.ToString();
                else
                    temp = curNotesManager.hpReplenishment.ToString();
                extraInfoPlantHpRecoveryValue.text = temp;
            }
            else
            {
                extraInfoPlantHungerValue.gameObject.SetActive(false);
                extraInfoPlantHungerImage.gameObject.SetActive(false);
                extraInfoPlantThirstValue.gameObject.SetActive(false);
                extraInfoPlantThirstImage.gameObject.SetActive(false);
                extraInfoPlantOxigenValue.gameObject.SetActive(false);
                extraInfoPlantOxigenImage.gameObject.SetActive(false);
                extraInfoPlantHpRecoveryValue.gameObject.SetActive(false);
                extraInfoPlantHpRecoveryImage.gameObject.SetActive(false);
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

    public void CloseLoreNote() => coroutineToStop = StartCoroutine(AnimateOpenCloseMultipleElement(new GameObject[] { GM.UI.Encyclopedia.LoreTab}, new GameObject[] { GM.UI.Encyclopedia.ExtraInfoLorePanel }));


    public void OpenCloseEncyclopedia(UnityEngine.InputSystem.InputAction.CallbackContext context = default)
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
            element.transform.localScale += new Vector3(0.04f, 0.04f, 0);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        coroutineToStop = null;
    }


    public IEnumerator AnimateOpenCloseMultipleElement(GameObject[] elementsToOpen, GameObject[] elementsToClose)
    {
        if (coroutineToStop != null)
            StopCoroutine(coroutineToStop);
        foreach (GameObject elem in elementsToOpen)
            elem.SetActive(true);
        elementsToOpen[0].transform.GetChild(0).gameObject.SetActive(false);
        while (elementsToOpen[0].transform.localScale.x <= 1)
        {
            foreach (GameObject elem in elementsToOpen)
                elem.transform.localScale += new Vector3(0.04f, 0.04f, 0);
            foreach (GameObject elem in elementsToClose)
                if (elem.transform.localScale.x > 0)
                    elem.transform.localScale -= new Vector3(0.04f, 0.04f, 0);
            yield return new WaitForSecondsRealtime(0.01f);
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
                elem.transform.localScale += new Vector3(0.04f, 0.04f, 0);
            foreach (GameObject elem in elementsToClose)
                if (elem.transform.localScale.x > 0)
                    elem.transform.localScale -= new Vector3(0.04f, 0.04f, 0);
            yield return new WaitForSecondsRealtime(0.01f);
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
                    elem.transform.localScale -= new Vector3(0.04f, 0.04f, 0);
            yield return new WaitForSecondsRealtime(0.01f);
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
            element.transform.localScale -= new Vector3(0.04f, 0.04f, 0);
            yield return new WaitForSecondsRealtime(0.01f);
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
            notificationCreature = curCreature;
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

    public void OpenPlantsTab()
    {
        coroutineToStop = StartCoroutine(AnimateOpenCloseMultipleElement(new GameObject[] { GM.UI.Encyclopedia.PlantsTab },
                                        new GameObject[] { GM.UI.Encyclopedia.LoreTab,
                                                           GM.UI.Encyclopedia.ExtraInfoLorePanel,
                                                           GM.UI.Encyclopedia.EnemiesTab,
                                                           GM.UI.Encyclopedia.ExtraInfoEnemyPanel}, scrollRectPlants));
    }

    public void OpenEnemiesTab()
    {
        coroutineToStop = StartCoroutine(AnimateOpenCloseMultipleElement(new GameObject[] { GM.UI.Encyclopedia.EnemiesTab },
                                        new GameObject[] { GM.UI.Encyclopedia.LoreTab,
                                                           GM.UI.Encyclopedia.ExtraInfoLorePanel,
                                                           GM.UI.Encyclopedia.PlantsTab,
                                                           GM.UI.Encyclopedia.ExtraInfoPlantPanel}));
    }

    public void OpenLoreTab()
    {
        coroutineToStop = StartCoroutine(AnimateOpenCloseMultipleElement(new GameObject[] { GM.UI.Encyclopedia.LoreTab },
                                        new GameObject[] { GM.UI.Encyclopedia.EnemiesTab,
                                                           GM.UI.Encyclopedia.ExtraInfoEnemyPanel,
                                                           GM.UI.Encyclopedia.PlantsTab,
                                                           GM.UI.Encyclopedia.ExtraInfoPlantPanel}, scrollRectLore));
    }


    public void OpenPlayerInEncyclopedia()
    {
        NotesManager newNoteSctipt = Instantiate(loreNotePrefab, GM.UI.Encyclopedia.EnemiesTab.transform.GetChild(0)).GetComponent<NotesManager>();
        newNoteSctipt.creature = playerScriptableObject;
        newNoteSctipt.creature.isOpenedInEcnyclopedia = true;
        if (!boolSaves.ContainsKey(newNoteSctipt.creature.name))
            boolSaves.Add(newNoteSctipt.creature.name, true);
        SaveBoolInfo();
        newNoteSctipt.InitializeNote();
    }

    private void SaveBoolInfo()
    {
        using (FileStream fs = new FileStream(pathForSaves, FileMode.Create))
        {
            formatter.Serialize(fs, boolSaves);
        }
    }

    private void LoadBoolInfo()
    {
        using (FileStream fs = new FileStream(pathForSaves, FileMode.Open))
        {
            boolSaves = (Dictionary<string, bool>) formatter.Deserialize(fs);
        }

        foreach(var note in notes.Values)
        {
            var noteScript = note.GetComponent<NotesManager>();
            noteScript.creature.isOpenedInEcnyclopedia = boolSaves[noteScript.creature.name];
            noteScript.InitializeNote();
        }
    }
}
