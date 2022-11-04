using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance { get; private set; }
    [SerializeField] private GameObject[] inventoryCells;

    public GameObject InventoryPanel;
    private GameObject AlreadyChosenCell = null;
    private bool isOpened;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }

        Destroy(this.gameObject);
    }

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

    //����������� ��������� �� ���������
    public void OnCellClick(GameObject CurrentCell)
    {
        //���� ������ ������ (������) ��� Swap-� ��� �� �������
        if (AlreadyChosenCell == null)
        {
            if (CurrentCell.GetComponent<InventoryCell>().item.TypeOfThisItem != ItemType.NoItem)
            {
                AlreadyChosenCell = CurrentCell;
            }
        }
        //���� ������ ������ (������) ��� Swap-� ��� �������
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

    //������������ ��������� �� ���������
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

    public void AddItem(ItemsBase newItem)
    {
        foreach (GameObject cell in inventoryCells)
        {
            if (cell.GetComponent<InventoryCell>().item.TypeOfThisItem == ItemType.NoItem)
            {
                cell.GetComponent<InventoryCell>().item = newItem;
                cell.GetComponent<InventoryCell>().amount = 0;
                cell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = newItem.image;
                return;
            }
        }
    }

    public bool IsInventoryFull()
    {
        foreach (GameObject cell in inventoryCells)
        {
            if (cell.GetComponent<InventoryCell>().item.TypeOfThisItem == ItemType.NoItem)
            {
                return false;
            }
        }
        return true;
    }

    public void ShowDescription()
    {

    }
}
