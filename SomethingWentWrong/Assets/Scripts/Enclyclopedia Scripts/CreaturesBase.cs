using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum creatureType {Unknown, Enemy, Plant};

public class CreaturesBase : ScriptableObject
{
    public string name;
    public string description;
    public creatureType typeOfThisCreature;
    public Sprite imageSmall;
    public Sprite imageBig;
    public bool isOpenedInEcnyclopedia;


    //start ??????????? ?? MonoBehaviour
    //private void Start()
    //{
    //    isOpenedInEcnyclopedia = false;
    //    Debug.Log(name);
    //}
}
