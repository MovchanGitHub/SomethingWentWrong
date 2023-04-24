using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NotesManager : MonoBehaviour, ISelectHandler
{
    public CreaturesBase creature;
    private TMPro.TextMeshProUGUI nameHeader;
    private Image icon;

    public string NameHeader {get {return nameHeader.text;}}

    [HideInInspector] public int hp;

    [HideInInspector] public Sprite lootSprite;
    [HideInInspector] public int lootAmount;

    private void Awake()
    {
        nameHeader = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        icon = transform.GetChild(1).GetComponent<Image>();

        if (creature.typeOfThisCreature == creatureType.Plant)
        {
            ResourceScript resourceScript = creature.creaturePrefab.GetComponent<ResourceScript>();
            //hp = resourceScript.MaxHP;
            lootSprite = creature.creaturePrefab.GetComponentInChildren<SpriteRenderer>().sprite;
            lootAmount = resourceScript.DropCount;
        }

        if (creature.isOpenedInEcnyclopedia)
        {
            OpenUpInfoInNote();
        }
        else
        {
            nameHeader.text = "����������";
            icon.sprite = creature.imageUnknown;
        }
    }

    public void OpenUpInfoInNote()
    {
        nameHeader.text = creature.name;
        icon.sprite = creature.imageSmall;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //if (EventSystem.current.currentSelectedGameObject == gameObject)
        //{
        //    Debug.Log(EventSystem.current.currentSelectedGameObject);
        //    StartCoroutine(DeselectNote());
        //}
        //else
            GameManager.GM.UI.Encyclopedia.EncyclopediaScript.OpenExtraInfo(gameObject);
    }

    //private IEnumerator DeselectNote()
    //{
    //    yield return new WaitForEndOfFrame();
    //    EventSystem.current.SetSelectedGameObject(null);
    //    Debug.Log(EventSystem.current.currentSelectedGameObject);
    //}

    //public void OnDeselect(BaseEventData eventData)
    //{
    //    Debug.Log("Deselect Event");
    //    GameManager.GM.UI.Encyclopedia.EncyclopediaScript.HideExtraInfo();
    //}

    //private void Start()
    //{
    //    nameHeader.GetComponent<Text>().text = creature.name;
    //    icon.GetComponent<Image>().sprite = creature.imageSmall;
    //}

}
