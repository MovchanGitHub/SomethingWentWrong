using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryPanel;
    private GameObject AlreadyChosenCell = null;
    private bool isOpened;

    private void Start()
    {
        isOpened = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpened = !isOpened;
            InventoryPanel.SetActive(isOpened);
            IsometricPlayerMovementController.IsAbleToMove = !IsometricPlayerMovementController.IsAbleToMove;
        }
    }

    //Перемещение предметов по инвентарю
    public void OnCellClick(GameObject CurrentCell)
    {
        //Если первый объект (ячейку) для Swap-а ещё не выбрали
        if (AlreadyChosenCell == null)
        {
            if (CurrentCell.GetComponent<InventoryCell>().item.TypeOfThisItem != ItemType.NoItem)
            {
                AlreadyChosenCell = CurrentCell;
            }
        }
        //Если первый объект (ячейку) для Swap-а уже выбрали
        else
        {
            GameObject temporary = Instantiate(AlreadyChosenCell);

            AlreadyChosenCell.GetComponent<InventoryCell>().item = CurrentCell.GetComponent<InventoryCell>().item;
            AlreadyChosenCell.GetComponent<InventoryCell>().amount = CurrentCell.GetComponent<InventoryCell>().amount;
            AlreadyChosenCell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = CurrentCell.GetComponent<InventoryCell>().item.image;

            CurrentCell.GetComponent<InventoryCell>().item = temporary.GetComponent<InventoryCell>().item;
            CurrentCell.GetComponent<InventoryCell>().amount = temporary.GetComponent<InventoryCell>().amount;
            CurrentCell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = temporary.GetComponent<InventoryCell>().item.image;

            Destroy(temporary);
            AlreadyChosenCell = null;
        }
    }

    //Выбрасывание предметов из инвентаря
    public void OnDropZoneSpaceClick()
    {
        if (AlreadyChosenCell != null)
        {
            AlreadyChosenCell.GetComponent<InventoryCell>().item = AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/EmtyCell.asset", typeof(ItemsBase)) as ItemsBase;
            AlreadyChosenCell.GetComponent<InventoryCell>().amount = 0;
            AlreadyChosenCell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = AlreadyChosenCell.GetComponent<InventoryCell>().item.image;
            AlreadyChosenCell = null;
        }
    }

    public void ShowDescription()
    {

    }
}
