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
        GameManager.GM.InventoryManager.GetComponent<InventoryHighlight>().highlighter = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }
}
