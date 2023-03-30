using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBin : MonoBehaviour
{
    public void onBinClick()
    {
        GameManager.GM.InventoryManager.deleteItem();
    }
}
