using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    ItemGrid itemGrid;

    private void Awake()
    {
        itemGrid = GetComponent<ItemGrid>();
    }

    private void Start()
    {
        InventoryController.instance.SelectedItemGrid = InventoryController.instance.standartItemGrid;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryController.instance.SelectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryController.instance.SelectedItemGrid = InventoryController.instance.standartItemGrid;
    }
}
