using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int maxStack;

    public int currentStack;

    public string name;

}

public class RockItem : Item
{
    public RockItem()
    {
        maxStack = 10;
        currentStack = 1;
        name = "Rock";
    }
}
