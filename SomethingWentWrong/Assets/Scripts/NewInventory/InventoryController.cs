using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class InventoryController : MonoBehaviour
{
    private InputSystem inputSystem;

    public ItemGrid standartItemGrid;
    [SerializeField] private ItemGrid selectedItemGrid;
    public ItemGrid SelectedItemGrid {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.setParent(value);
        }
    }

    InventoryItem selectedItem;
    InventoryItem newItem;
    InventoryItem overlapItem;
    RectTransform newRectTransform;
    RectTransform rectTransform;

    [SerializeField] List<ItemsBase> items;
    [SerializeField] GameObject itemPrefab;
    public Transform canvasTransform;

    Vector2Int oldPosition;
    InventoryHighlight inventoryHighlight;
    InventoryItem itemToHighlight;
    [HideInInspector] public bool isCanvasActive = false;

    [HideInInspector] public bool canBeOpened;

    private Dictionary<string, int> ammoCounter;

    public Dictionary<string, int> AmmoCounter
    {
        get => ammoCounter;
    }
    
    
    private void UpdateWeaponBar(string key, int value)
    {
        switch (key)
        {
            case "Стрельника": GM.UI.WeaponsBarScript.ammoCount1.text = value.ToString(); break;
            case "Бомбалика": GM.UI.WeaponsBarScript.ammoCount2.text = value.ToString(); break;
            case "Тенносей": GM.UI.WeaponsBarScript.ammoCount3.text = value.ToString(); break;  
        }
    }

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Start()
    {
        inputSystem = GameManager.GM.InputSystem;

        canBeOpened = true;

        ammoCounter = new Dictionary<string, int>{ { "Стрельника", 0}, { "Бомбалика", 0}, { "Тенносей", 0} };
    }

    public void OpenCloseInventory(InputAction.CallbackContext context)
    {
        isCanvasActive = !isCanvasActive;
        canvasTransform.gameObject.SetActive(isCanvasActive);
        if (isCanvasActive)
        {
            inputSystem.BlockPlayerInputs();
        }
        else
        {
            inputSystem.UnblockPlayerInputs();
        }
    }

    private void Update()
    {
        dragItemIcon();

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        if (isCanvasActive)
        {
            handleHighlight();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            onPressLeftMouseButton();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            onPressRightMouseButton();
        }
        if (selectedItem != null)
        {
            //Debug.Log(selectedItem.itemData.itemName);
        }
    }

    public void activateInventory(bool isActive)
    {
        isCanvasActive = isActive;
        canvasTransform.gameObject.SetActive(isCanvasActive);
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

    private void createItem(ItemsBase item)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        newItem = inventoryItem;

        newRectTransform = inventoryItem.GetComponent<RectTransform>();
        newRectTransform.SetParent(canvasTransform);

        inventoryItem.Set(item);
    }

    public bool checkSpaceInInventory(ItemsBase item)
    {
        createItem(item);
        InventoryItem itemToInsert = newItem;
        newItem = null;

        Vector2Int? posOnGrid = SelectedItemGrid.findSpaceForItem(itemToInsert);
        Destroy(itemToInsert.gameObject);
        newItem = null;

        if (posOnGrid == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void insertItem(ItemsBase item)
    {
        createItem(item);
        InventoryItem itemToInsert = newItem;
        newItem = null;

        Vector2Int? posOnGrid = SelectedItemGrid.findSpaceForItem(itemToInsert);

        if (posOnGrid == null)
        {
            Destroy(itemToInsert.gameObject);
            newItem = null;
            return;
        }

        selectedItemGrid.placeItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
        if (item.TypeOfThisItem == ItemType.Weapon)
        {
            UpdateWeaponBar(item.itemName, ++ammoCounter[item.itemName]);
        }
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

    private void onPressRightMouseButton()
    {
        Vector2Int tileGridPosition = getTileGridPosition();

        if (selectedItem == null)
        {
            UseItem(tileGridPosition);
        }
    }

    private Vector2Int getTileGridPosition()
    {
        Vector2 pos = Mouse.current.position.ReadValue();

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
            rectTransform.SetAsLastSibling();
        }
    }

    private void dragItemIcon()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Mouse.current.position.ReadValue();
            rectTransform.SetAsLastSibling();
        }
    }

    private void UseItem(Vector2Int tileGridPosition)
    {
        InventoryItem item = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (item != null)
        {
            if (item.itemData.TypeOfThisItem == ItemType.Food)
            {
                if (item != null)
                {
                    ItemTypeFood itemToUse = item.itemData as ItemTypeFood;
                    GameManager.GM.SurvivalManager.ReplenishHunger(itemToUse.satiationEffect);
                    GameManager.GM.SurvivalManager.ReplenishThirst(itemToUse.slakingOfThirstEffect);
                    GameManager.GM.SurvivalManager.ReplenishAnoxaemia(itemToUse.oxygenRecovery);
                }
                Destroy(item.gameObject);
                SelectedItemGrid.cleanGridRef(item);
            }
            else 
            {
                SelectedItemGrid.PlaceItem(item, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
            }
        }
}

    public void changeScale(int newScale)
    {
        gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
        ItemGrid.tileWidth = ItemGrid.tileSpriteWidth * newScale;
        ItemGrid.tileHeight = ItemGrid.tileSpriteHeight * newScale;
    }
}

