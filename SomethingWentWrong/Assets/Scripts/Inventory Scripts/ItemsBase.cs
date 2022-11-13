using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ItemType {NoItem, Weapon, Food, Resource};

public class ItemsBase : ScriptableObject
{
    public string itemName;
    public int maximumAmount;
    public string itemDescription;
    public ItemType TypeOfThisItem;
    public Sprite image;
    public GameObject dragAndDropElement;
    public GameObject dropObject;
}
