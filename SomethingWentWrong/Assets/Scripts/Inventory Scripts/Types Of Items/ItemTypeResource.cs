using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Some Resource", menuName = "Items/Resource Item", order = 52)]
public class ItemTypeResource : ItemsBase
{

    private void Start()
    {
        TypeOfThisItem = ItemType.Resource;
    }
}

