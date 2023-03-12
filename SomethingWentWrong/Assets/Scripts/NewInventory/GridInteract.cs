using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    ItemGrid itemGrid;
    private Vector2Int curPos;
    bool wasShownIncreasment = false;
    private SurvivalBar survivalBarScript;

    private void Awake()
    {
        itemGrid = GetComponent<ItemGrid>();
    }

    private void Start()
    {
        survivalBarScript = GameManager.GM.UI.GetComponentInChildren<SurvivalBar>();

        GameManager.GM.InventoryManager.SelectedItemGrid = GameManager.GM.InventoryManager.standartItemGrid;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.GM.InventoryManager.SelectedItemGrid = itemGrid;
        curPos = itemGrid.getTileGridPosition(eventData.position);
        if ((itemGrid.getItem(curPos.x, curPos.y)?.itemData.TypeOfThisItem ?? ItemType.NoItem) == ItemType.Food)
        {
            survivalBarScript.ShowIncreasmentFromFood(itemGrid.getItem(curPos.x, curPos.y).itemData as ItemTypeFood);
            wasShownIncreasment = true;
            Debug.Log(1);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.GM.InventoryManager.SelectedItemGrid = GameManager.GM.InventoryManager.standartItemGrid;
        if (wasShownIncreasment)
        {
            survivalBarScript.RemoveIncreasmentFromFood();
            Debug.Log(2);
            wasShownIncreasment = false;
        }
    }
}
