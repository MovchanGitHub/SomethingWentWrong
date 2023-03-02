using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNewInventory : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.GM.InventoryManager != null)
        {
            GameManager.GM.InventoryManager.standartItemGrid = gameObject.GetComponentInChildren<ItemGrid>();
            GameManager.GM.InventoryManager.standartItemGrid = gameObject.GetComponentInChildren<ItemGrid>();
            GameManager.GM.InventoryManager.canvasTransform = gameObject.GetComponent<RectTransform>();
            GameManager.GM.InventoryManager.gameObject.GetComponent<InventoryHighlight>().highlighter = gameObject.transform.GetChild(1).GetComponent<RectTransform>();
        }
    }
}
