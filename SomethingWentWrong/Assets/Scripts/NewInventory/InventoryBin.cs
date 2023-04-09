using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBin : MonoBehaviour
{
    public void onBinClick()
    {
        if (GameManager.GM.InventoryManager.SelectedItem.itemData.TypeOfThisItem == ItemType.Weapon)
        {
            --GameManager.GM.InventoryManager.AmmoCounter[GameManager.GM.InventoryManager.SelectedItem.itemData.itemName];
            GameManager.GM.InventoryManager.UpdateWeaponBar(GameManager.GM.InventoryManager.SelectedItem.itemData.itemName, GameManager.GM.InventoryManager.AmmoCounter[GameManager.GM.InventoryManager.SelectedItem.itemData.itemName]);
        }
        GameManager.GM.InventoryManager.deleteItem();
    }
}
