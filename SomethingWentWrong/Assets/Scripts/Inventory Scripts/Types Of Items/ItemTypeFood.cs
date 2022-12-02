using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Some Food", menuName = "Items/Food Item", order = 52)]
public class ItemTypeFood : ItemsBase
{
    public int satiationEffect;
    public int slakingOfThirstEffect;
    public int healEffect;
    public int oxygenRecovery;
    
    private void Start()
    {
        TypeOfThisItem = ItemType.Food;
    }
}
