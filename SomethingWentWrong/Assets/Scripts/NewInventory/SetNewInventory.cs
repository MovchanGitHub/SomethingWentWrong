using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNewInventory : MonoBehaviour
{
    private void Awake()
    {
        if (InventoryController.instance != null)
        {
            InventoryController.instance.standartItemGrid = gameObject.GetComponentInChildren<ItemGrid>();
            InventoryController.instance.standartItemGrid = gameObject.GetComponentInChildren<ItemGrid>();
            InventoryController.instance.canvasTransform = gameObject.GetComponent<RectTransform>();
            InventoryController.instance.gameObject.GetComponent<InventoryHighlight>().highlighter = gameObject.transform.GetChild(1).GetComponent<RectTransform>();
        }
    }
}
