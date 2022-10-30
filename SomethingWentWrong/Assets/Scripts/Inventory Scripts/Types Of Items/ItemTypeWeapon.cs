using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Some Weapon", menuName = "Items/Weapon Item", order = 52)]
public class ItemTypeWeapon : ItemsBase
{
    public int Damage;
    public int Range;

    private void Start()
    {
        TypeOfThisItem = ItemType.Weapon;
    }
}
