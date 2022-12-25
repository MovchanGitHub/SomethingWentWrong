using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public ItemGrid standartItemGrid;
    [HideInInspector] private ItemGrid selectedItemGrid;
    public ItemGrid SelectedItemGrid { 
        get => selectedItemGrid; 
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.setParent(value);
        }
    }

    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemsBase> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    Vector2Int oldPosition;
    InventoryHighlight inventoryHighlight;
    InventoryItem itemToHighlight;
    private bool isCanvasActive = false;

    public static InventoryController instance { get; private set; }

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }

        Destroy(this.gameObject);
    }

    private void Update()
    {
        dragItemIcon();

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            isCanvasActive = !isCanvasActive;
            canvasTransform.gameObject.SetActive(isCanvasActive);
        }

        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            insertRandomItem();
        }
        */

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        handleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            onPressLeftMouseButton();
        }
    }

    private void handleHighlight()
    {
        Vector2Int posOnGrid = getTileGridPosition();
        if (oldPosition == posOnGrid)
        {
            return;
        }
        oldPosition = posOnGrid;

        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.getItem(posOnGrid.x, posOnGrid.y);
            if (itemToHighlight)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.setPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.boundaryCheck(posOnGrid.x, posOnGrid.y, selectedItem.itemData.width, selectedItem.itemData.height));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.setPosition(selectedItemGrid, selectedItem, posOnGrid.x, posOnGrid.y);
        }
    }

    /*
    private void createRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
    }
    */

    private void createItem(ItemsBase item)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        inventoryItem.Set(item);
    }

    /*
    private void insertRandomItem()
    {
        createRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        insertItem(itemToInsert);
    }
    */

    public void insertItem(ItemsBase item)
    {
        createItem(item);
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;

        Vector2Int? posOnGrid = SelectedItemGrid.findSpaceForItem(itemToInsert);

        if (posOnGrid == null)
        {
            Destroy(itemToInsert.gameObject);
            selectedItem = null;
            return;
        }

        selectedItemGrid.placeItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    private void onPressLeftMouseButton()
    {
        Vector2Int tileGridPosition = getTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            DropItem(tileGridPosition);
        }
    }

    private Vector2Int getTileGridPosition()
    {
        Vector2 pos = Input.mousePosition;

        if (selectedItem != null)
        {
            pos.x -= (selectedItem.itemData.width - 1) * ItemGrid.tileWidth / 2;
            pos.y += (selectedItem.itemData.height - 1) * ItemGrid.tileHeight / 2;
        }

        return selectedItemGrid.getTileGridPosition(pos);
    }

    private void DropItem(Vector2Int tileGridPosition)
    {
        bool isDropComplete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (isDropComplete)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void dragItemIcon()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
}

