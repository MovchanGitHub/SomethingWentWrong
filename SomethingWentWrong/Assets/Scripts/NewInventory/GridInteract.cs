using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    ItemGrid itemGrid;
    private Vector2Int curPos;

    private void Awake()
    {
        itemGrid = GetComponent<ItemGrid>();
    }

    private void Start()
    {
        GameManager.GM.InventoryManager.SelectedItemGrid = GameManager.GM.InventoryManager.standartItemGrid;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.GM.InventoryManager.SelectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.GM.InventoryManager.SelectedItemGrid = GameManager.GM.InventoryManager.standartItemGrid;
    }
}
