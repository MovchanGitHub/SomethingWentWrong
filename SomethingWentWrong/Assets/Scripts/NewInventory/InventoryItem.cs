using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemsBase itemData;
    public int onGridPositionX;
    public int onGridPositionY;

    internal void Set(ItemsBase itemsBase)
    {
        itemData = itemsBase;

        transform.GetComponent<Image>().sprite = itemData.image;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileWidth;
        size.y = itemData.height * ItemGrid.tileHeight;

        GetComponent<RectTransform>().sizeDelta = size;
    }
}
