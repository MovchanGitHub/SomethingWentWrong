using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NotesManager : MonoBehaviour
{
    public CreaturesBase creature;
    private TMPro.TextMeshProUGUI nameHeader;
    private Image icon;

    public string NameHeader {get {return nameHeader.text;}}

    [HideInInspector] public int hp;

    [HideInInspector] public ItemType lootType;
    [HideInInspector] public Sprite lootSprite;
    [HideInInspector] public int lootAmount;
    [HideInInspector] public int hungerReplenishment;
    [HideInInspector] public int thirstReplenishment;
    [HideInInspector] public int oxigenReplenishment;

    [HideInInspector] public int damage;
    [HideInInspector] public float speed;

    private void Start()
    {
        if (creature.typeOfThisCreature == creatureType.Plant)
        {
            ResourceScript resourceScript = creature.creaturePrefab.GetComponent<ResourceScript>();
            hp = resourceScript.MaxHP;
            lootSprite = resourceScript.Drop.GetComponentInChildren<SpriteRenderer>().sprite;
            lootAmount = resourceScript.TimesToDrop;
            lootType = resourceScript.Drop.GetComponent<PickUpScript>().itemToInventory.TypeOfThisItem;
            if (lootType == ItemType.Food)
            {
                
                ItemTypeFood dropItem = resourceScript.Drop.GetComponent<PickUpScript>().itemToInventory as ItemTypeFood;
                hungerReplenishment = dropItem?.satiationEffect ?? 0;
                thirstReplenishment = dropItem?.slakingOfThirstEffect ?? 0;
                oxigenReplenishment = dropItem?.oxygenRecovery ?? 0;
            }
        }
        else if (creature.typeOfThisCreature == creatureType.Enemy)
        {
            hp = creature.creaturePrefab.GetComponentInChildren<EnemyDamagable>().MaxHP;
            damage = creature.creaturePrefab.GetComponentInChildren<EnemyAttack>().Damage;
            speed = creature.creaturePrefab.GetComponent<EnemyMovement>().speed;
        }

    }

    public void InitializeNote()
    {
        nameHeader = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        icon = transform.GetChild(1).GetComponent<Image>();

        if (creature.isOpenedInEcnyclopedia)
        {
            OpenUpInfoInNote();
        }
        else
        {
            nameHeader.text = "Неизвестно";
            icon.sprite = creature.imageUnknown;
            GetComponent<Button>().interactable = false;
        }
    }

    public void OpenUpInfoInNote()
    {
        nameHeader.text = creature.name;
        icon.sprite = creature.imageSmall;
        GetComponent<Button>().interactable = true;
    }


    public void OnSelect()
    {
        GameManager.GM.UI.Encyclopedia.EncyclopediaScript.OpenExtraInfo(gameObject);
    }
}
