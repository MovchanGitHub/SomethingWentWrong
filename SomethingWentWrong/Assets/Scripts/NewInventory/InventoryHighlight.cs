using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);
    }

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * ItemGrid.tileWidth;
        size.y = targetItem.itemData.height * ItemGrid.tileHeight;
        highlighter.sizeDelta = size;
    }    

    public void setPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        setParent(targetGrid);
        Vector2 pos = targetGrid.calcPosOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);
        highlighter.localPosition = pos;
    }

    public void setParent(ItemGrid targetGrid)
    {
        if (targetGrid == null)
        {
            return;
        }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void setPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        Vector2 pos = targetGrid.calcPosOnGrid(targetItem, posX, posY);
        highlighter.localPosition = pos;
    }
}
