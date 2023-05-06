using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum creatureType {Unknown, Enemy, Plant};

[CreateAssetMenu(fileName = "Some creature", menuName = "Creatures/Creature", order = 51)]
public class CreaturesBase : ScriptableObject
{
    public string name;
    public string description;
    public creatureType typeOfThisCreature;
    public GameObject creaturePrefab;
    public Sprite imageSmall;
    public Sprite imageBig;
    public Sprite imageUnknown;
    public bool isOpenedInEcnyclopedia;
    public string noName;
}
