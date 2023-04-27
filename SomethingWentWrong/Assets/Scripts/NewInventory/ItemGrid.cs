using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGrid : MonoBehaviour
{
    [HideInInspector] public static float tileWidth = 24;
    [HideInInspector] public static float tileHeight = 24;

    [HideInInspector] public static float tileSpriteWidth = 24;
    [HideInInspector] public static float tileSpriteHeight = 24;

    RectTransform rectTransform;
    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    InventoryItem[,] inventoryItemSlots;

    [HideInInspector] int gridSizeWidth = 10;
    [HideInInspector] int gridSizeHeight = 5;

    private (int,int) reservedPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        InitializeGrid(gridSizeWidth, gridSizeHeight);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void InitializeGrid(int width, int height)
    {
        inventoryItemSlots = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSpriteWidth, height * tileSpriteHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int getTileGridPosition(Vector2 mousePosition, Vector2 inventoryCanvasScale, Vector2 UICanvasScale)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileWidth / inventoryCanvasScale.x / UICanvasScale.x);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileHeight / inventoryCanvasScale.y/ UICanvasScale.y);

        /*
        if (tileGridPosition.x > gridSizeWidth - 1)
        {
            tileGridPosition.x = gridSizeWidth - 1;
        }
        if (tileGridPosition.y > gridSizeHeight - 1)
        {
            tileGridPosition.y = gridSizeHeight - 1;
        }
        if (tileGridPosition.x < 0)
            tileGridPosition.x = 0;
        if (tileGridPosition.y < 0)
            tileGridPosition.y = 0;
        */

        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        cleanGridRef(inventoryItem);

        if (!boundaryCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height))
        {
            for (int i = reservedPosition.Item1; i < reservedPosition.Item1 + inventoryItem.itemData.width; i++)
            {
                for (int j = reservedPosition.Item2; j < reservedPosition.Item2 + inventoryItem.itemData.height; j++)
                {
                    inventoryItemSlots[i, j] = inventoryItem;
                }
            }
            return false;
        }
        
        if (!overlapCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height, ref overlapItem))
        {
            for (int i = reservedPosition.Item1; i < reservedPosition.Item1 + inventoryItem.itemData.width; i++)
            {
                for (int j = reservedPosition.Item2; j < reservedPosition.Item2 + inventoryItem.itemData.height; j++)
                {
                    inventoryItemSlots[i, j] = inventoryItem;
                }
            }
            overlapItem = null;
            return false;
        }

        /*
        if (overlapItem)
        {
            cleanGridRef(overlapItem);
        }
        */
        reservedPosition = (-1, -1);
        placeItem(inventoryItem, posX, posY);

        return true;
    }

    internal void ReturnItem(InventoryItem item)
    {
        placeItem(item, reservedPosition.Item1, reservedPosition.Item2);
        reservedPosition = (-1, -1);
    }

    internal InventoryItem getItem(int x, int y)
    {
        if (positionCheck(x, y))
        {
            return inventoryItemSlots[x, y];
        }
        else
        {
            return null;
        }
    }

    public void clearGrid()
    {
        for (int i = 0; i < gridSizeWidth; i++)
        {
            for (int j = 0; j < gridSizeHeight; j++)
            {
                if (inventoryItemSlots[i, j])
                {
                    Destroy(inventoryItemSlots[i, j].gameObject);
                    cleanGridRef(inventoryItemSlots[i, j]);
                }
            }
        }
    }

    public Vector2 calcPosOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 pos = new Vector2();
        pos.x = posX * tileSpriteWidth + tileSpriteWidth * inventoryItem.itemData.width / 2;
        pos.y = -(posY * tileSpriteHeight + tileSpriteHeight * inventoryItem.itemData.height / 2);
        return pos;
    }

    public InventoryItem checkItem(int x, int y)
    {
        InventoryItem item = null;
        if (positionCheck(x, y))
            item = inventoryItemSlots[x, y];

        return item;
    }

    public InventoryItem LeftMousePickUp(int x, int y)
    {
        InventoryItem item = null;
        if (positionCheck(x, y))
            item = inventoryItemSlots[x, y];

        if (item == null)
        {
            return null;
        }

        reservedPosition = (item.onGridPositionX, item.onGridPositionY);

        //cleanGridRef(item);

        return item;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem item = null;
        if (positionCheck(x, y))
            item = inventoryItemSlots[x, y];

        if (item == null)
        {
            return null;
        }

        cleanGridRef(item);

        return item;
    }

    public Vector2Int? findSpaceForItem(InventoryItem itemToInsert)
    {
        for (int y = 0; y < gridSizeHeight - itemToInsert.itemData.height + 1; y++)
        {
            for (int x = 0; x < gridSizeWidth - itemToInsert.itemData.width + 1; x++)
            {
                if (checkAvailableSpace(x, y, itemToInsert.itemData.width, itemToInsert.itemData.height))
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return null;
    }

    public void placeItem(InventoryItem itemToInsert, int posX, int posY)
    {
        RectTransform rectTransform = itemToInsert.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < itemToInsert.itemData.width; x++)
        {
            for (int y = 0; y < itemToInsert.itemData.height; y++)
            {
                inventoryItemSlots[posX + x, posY + y] = itemToInsert;
            }
        }

        itemToInsert.onGridPositionX = posX;
        itemToInsert.onGridPositionY = posY;
        Vector2 pos = calcPosOnGrid(itemToInsert, posX, posY);

        rectTransform.localPosition = pos;
    }

    public void cleanGridRef(InventoryItem item)
    {
        for (int i = 0; i < item.itemData.width; i++)
        {
            for (int j = 0; j < item.itemData.height; j++)
            {
                inventoryItemSlots[item.onGridPositionX + i, item.onGridPositionY + j] = null;
            }
        }
    }

    bool positionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
        {
            return false;
        }

        if (posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    public bool boundaryCheck(int posX, int posY, int width, int height)
    {
        if (positionCheck(posX, posY) == false)
        {
            return false;
        }

        posX += width - 1;
        posY += height - 1;

        if (positionCheck(posX, posY) == false)
        {
            return false;
        }

        return true;
    }

    private bool overlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                string itemName = "null";
                if (inventoryItemSlots[posX + x, posY + y])
                {
                    itemName = inventoryItemSlots[posX + x, posY + y].itemData.itemName;
                }
                if (inventoryItemSlots[posX + x, posY + y] != null)
                {
                    return false;
                    /*
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlots[posX + x, posY + y];
                    }
                    */
                }
                /*
                else
                {
                    if (overlapItem != inventoryItemSlots[posX + x, posY + y])
                    {
                        return false;
                    }
                }
                */
            }
        }

        return true;
    }

    private bool checkAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlots[posX + x, posY + y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool checkAmmo(ItemsBase ammoType)
    {
        for (int i = 0; i < gridSizeWidth; i++)
        {
            for (int j = 0; j < gridSizeHeight; j++)
            {
                if (inventoryItemSlots[i,j] && inventoryItemSlots[i,j].itemData == ammoType)
                {
                    Destroy(inventoryItemSlots[i, j].gameObject);
                    cleanGridRef(inventoryItemSlots[i, j]);
                    var ammo = --GameManager.GM.InventoryManager.AmmoCounter[ammoType.itemName];
                    GameManager.GM.InventoryManager.UpdateWeaponBar(ammoType.itemName, ammo);
                    return true;
                }
            }
        }
        return false;
    }
}

