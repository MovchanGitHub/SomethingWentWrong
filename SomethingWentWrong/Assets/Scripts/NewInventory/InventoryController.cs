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
    public InventoryItem SelectedItem
    {
        get => selectedItem;
    }
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

    private Dictionary<string, int> ammoCounter;

    public Dictionary<string, int> AmmoCounter
    {
        get => ammoCounter;
        set => ammoCounter = value;
    }

    private SurvivalBar survivalBarScript;
    bool wasShownIncreasment = false;

    [SerializeField] private ItemsBase[] startItems;


    public void UpdateWeaponBar(string key, int value)
    {
        switch (key)
        {
            case "crystal": GM.UI.WeaponsBarScript.ammoCount1.text = value.ToString();break;
            case "bomb": GM.UI.WeaponsBarScript.ammoCount2.text = value.ToString(); break;
            case "bullet": GM.UI.WeaponsBarScript.ammoCount3.text = value.ToString(); break;
            default: Debug.LogError(key); break;
        }
    }

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Start()
    {
        inputSystem = GM.InputSystem;

        ammoCounter = new Dictionary<string, int>{ { "bullet", 0}, { "bomb", 0}, { "crystal", 0} };

        survivalBarScript = GM.UI.GetComponentInChildren<SurvivalBar>();

        /*
        foreach (var item in startItems)
        {
            insertItem(item);
        }
        */
    }

    public void OpenCloseInventory(InputAction.CallbackContext context)
    {
        if (GM.PlayerMovement.isActiveAndEnabled)
        {
            isCanvasActive = !isCanvasActive;
        }

        if (!isCanvasActive)
        {
            removeEffects();
            if (selectedItem != null)
            {
                //insertItem(selectedItem.itemData);
                SelectedItemGrid.ReturnItem(selectedItem);
                //Destroy(selectedItem.gameObject);
                selectedItem = null;
            }
        }

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

        if (Mouse.current.leftButton.wasPressedThisFrame && isCanvasActive)
        {
            onPressLeftMouseButton();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame && isCanvasActive)
        {
            onPressRightMouseButton();
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
                if (isCanvasActive && itemToHighlight.itemData.TypeOfThisItem == ItemType.Food)
                {
                    survivalBarScript.ShowEffectsFromFood(itemToHighlight.itemData as ItemTypeFood);
                    wasShownIncreasment = true;
                }
                else
                {
                    removeEffects();
                }
            }
            else
            {
                inventoryHighlight.Show(false);
                removeEffects();
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.boundaryCheck(posOnGrid.x, posOnGrid.y, selectedItem.itemData.width, selectedItem.itemData.height));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.setPosition(selectedItemGrid, selectedItem, posOnGrid.x, posOnGrid.y);
        }
    }

    private void removeEffects()
    {
        if (wasShownIncreasment)
        {
            survivalBarScript.RemoveEffectsFromFood();
            wasShownIncreasment = false;
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

        return !(posOnGrid == null);
    }

    public void insertItem(ItemsBase item)
    {
        createItem(item);
        InventoryItem itemToInsert = newItem;
        newItem = null;

        itemToInsert.transform.localScale = Vector2.one; //scale update
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

        return selectedItemGrid.getTileGridPosition(pos, canvasTransform.localScale, canvasTransform.parent.localScale);
    }

    private void DropItem(Vector2Int tileGridPosition)
    {
        bool isDropComplete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (isDropComplete)
        {
            /*
            if (selectedItem.itemData.TypeOfThisItem == ItemType.Weapon)
            {
                ++ammoCounter[selectedItem.itemData.itemName];
                UpdateWeaponBar(selectedItem.itemData.itemName, ammoCounter[selectedItem.itemData.itemName]);
            }
            */
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
        selectedItem = selectedItemGrid.LeftMousePickUp(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            removeEffects();
            if (selectedItem.itemData.TypeOfThisItem == ItemType.Weapon)
            {
                --ammoCounter[selectedItem.itemData.itemName];
                UpdateWeaponBar(selectedItem.itemData.itemName, ammoCounter[selectedItem.itemData.itemName]);
            }
            */
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
        removeEffects();
        InventoryItem itemToCheck = SelectedItemGrid.checkItem(tileGridPosition.x, tileGridPosition.y);
        if (itemToCheck != null && itemToCheck.itemData.TypeOfThisItem == ItemType.Weapon)
        {
            return;
        }

        InventoryItem item = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (item != null)
        {
            ItemTypeFood itemToUse = item.itemData as ItemTypeFood;
            GM.SurvivalManager.ReplenishHunger(itemToUse.satiationEffect);
            GM.SurvivalManager.ReplenishThirst(itemToUse.slakingOfThirstEffect);
            GM.SurvivalManager.ReplenishAnoxaemia(itemToUse.oxygenRecovery);
            GM.PlayerMovement.GetComponentInChildren<PlayerDamagable>().HP += itemToUse.healEffect;
            Destroy(item.gameObject);
            inventoryHighlight.Show(false);
            SelectedItemGrid.cleanGridRef(item);
        }
    }

    public void changeScale(int newScale)
    {
        gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
        ItemGrid.tileWidth = ItemGrid.tileSpriteWidth * newScale;
        ItemGrid.tileHeight = ItemGrid.tileSpriteHeight * newScale;
    }

    public void deleteItem()
    {
        if (selectedItem != null)
        {
            Destroy(selectedItem.gameObject);
            selectedItem = null;
        }
    }
}

