using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNewInventory : MonoBehaviour
{
    private void Start()
    {
        GameManager.GM.InventoryManager.standartItemGrid = gameObject.GetComponentInChildren<ItemGrid>();
        GameManager.GM.InventoryManager.standartItemGrid = gameObject.GetComponentInChildren<ItemGrid>();
        GameManager.GM.InventoryManager.canvasTransform = gameObject.GetComponent<RectTransform>();
        if (!GameManager.GM.InventoryManager.GetComponent<InventoryHighlight>().highlighter)
        {
            GameManager.GM.InventoryManager.GetComponent<InventoryHighlight>().highlighter = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        }
    }
}
