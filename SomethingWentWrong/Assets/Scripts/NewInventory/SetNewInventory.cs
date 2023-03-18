using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNewInventory : MonoBehaviour
{
    [SerializeField] private ItemGrid grid;

    private void Start()
    {
        GameManager.GM.InventoryManager.standartItemGrid = gameObject.GetComponentInChildren<ItemGrid>();
        GameManager.GM.InventoryManager.standartItemGrid = gameObject.GetComponentInChildren<ItemGrid>();
        GameManager.GM.InventoryManager.canvasTransform = gameObject.GetComponent<RectTransform>();
        GameManager.GM.InventoryManager.GetComponent<InventoryHighlight>().highlighter = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (ItemGrid.tileWidth != ItemGrid.tileSpriteWidth * gameObject.transform.localScale.x * GameManager.GM.UI.transform.localScale.x)
        {
            ItemGrid.tileWidth = ItemGrid.tileSpriteWidth * gameObject.transform.localScale.x * GameManager.GM.UI.transform.localScale.x;
        }

        if (ItemGrid.tileHeight != ItemGrid.tileSpriteHeight * gameObject.transform.localScale.y)
        {
            ItemGrid.tileHeight = ItemGrid.tileSpriteHeight * gameObject.transform.localScale.y * GameManager.GM.UI.transform.localScale.y;
        }
    }
}
