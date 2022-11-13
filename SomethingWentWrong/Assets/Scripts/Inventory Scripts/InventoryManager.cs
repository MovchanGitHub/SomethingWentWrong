using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance { get; private set; }
    [SerializeField] private GameObject[] inventoryCells;
    [SerializeField] private GameObject emptyCell;

    private Sprite tempCellImage;
    [SerializeField] private GameObject ContextMenu;
    [SerializeField] private GameObject SurvivalManager;

    public GameObject InventoryPanel;
    private GameObject AlreadyChosenCell = null;
    private GameObject ChosenCellExtra = null;
    private bool isOpened;
    private GameObject onMouseObject;
    private Transform playerTransform;

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

        playerTransform = GameManagerScript.instance.player.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpened = !isOpened;
            InventoryPanel.SetActive(isOpened);
            IsometricPlayerMovementController.IsAbleToMove = !IsometricPlayerMovementController.IsAbleToMove;
            if (AlreadyChosenCell != null)
            {
                AlreadyChosenCell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = tempCellImage;
            }
            AlreadyChosenCell = null;
            if (onMouseObject != null)
            {
                Destroy(onMouseObject);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (ContextMenu.activeSelf == true)
            {
                StartCoroutine(CorountineClosingContextMenu());
            }
        }
    }

    IEnumerator CorountineClosingContextMenu()
    {
        yield return new WaitForSeconds(0.2f);
        ChosenCellExtra = null;
        ContextMenu.SetActive(false);
    }

    //Ïåðåìåùåíèå ïðåäìåòîâ ïî èíâåíòàðþ
    public void OnCellClick(GameObject CurrentCell)
    {
        //Åñëè ïåðâûé îáúåêò (ÿ÷åéêó) äëÿ Swap-à åù¸ íå âûáðàëè
        if (AlreadyChosenCell == null)
        {
            if (CurrentCell.GetComponent<InventoryCell>().item.TypeOfThisItem != ItemType.NoItem)
            {

                //Ïðè íàæàòèè íà ïðàâóþ êíîïêó ìûøè âûñâå÷èâàåòñÿ êîíòåêñòíîå ìåíþ
                if (Input.GetMouseButtonUp(1))
                {
                    ChosenCellExtra = CurrentCell;
                    Debug.Log("ContextMenuHere");
                    ShowContextMenu(CurrentCell);
                }
                else
                {
                    AlreadyChosenCell = CurrentCell;
                    onMouseObject = Instantiate(AlreadyChosenCell.GetComponent<InventoryCell>().item.dragAndDropElement, transform);
                    onMouseObject.transform.position = Input.mousePosition;
                    tempCellImage = AlreadyChosenCell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite;
                    AlreadyChosenCell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = emptyCell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite;
                }
            }

         
        }
        //Åñëè ïåðâûé îáúåêò (ÿ÷åéêó) äëÿ Swap-à óæå âûáðàëè
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
            Destroy(onMouseObject);
            onMouseObject = null;
            AlreadyChosenCell = null;
        }
    }

    //Âûáðàñûâàíèå ïðåäìåòîâ èç èíâåíòàðÿ
    public void OnDropZoneSpaceClick()
    {

        if (AlreadyChosenCell != null)
        {
            GameObject dropObject = Instantiate(AlreadyChosenCell.GetComponent<InventoryCell>().item.dropObject);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dropObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0);

            AlreadyChosenCell.GetComponent<InventoryCell>().item = emptyCell.GetComponent<InventoryCell>().item;
            AlreadyChosenCell.GetComponent<InventoryCell>().amount = 0;
            AlreadyChosenCell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = emptyCell.GetComponent<InventoryCell>().item.image;
            Destroy(onMouseObject);
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

    private void ShowContextMenu(GameObject CurrentCell)
    {
        ContextMenu.transform.position = new Vector2(Input.mousePosition.x + ContextMenu.GetComponent<RectTransform>().rect.width / 2, Input.mousePosition.y - ContextMenu.GetComponent<RectTransform>().rect.height / 2);
        ContextMenu.SetActive(true);
    }

    public void UseItem() /*(GameObject CurrentCell)*/
    {
        if(ChosenCellExtra.GetComponent<InventoryCell>().item.TypeOfThisItem == ItemType.Food)
        {
            ItemTypeFood temporary = ChosenCellExtra.GetComponent<InventoryCell>().item as ItemTypeFood;
            SurvivalManager.GetComponent<SurvivalManager>().ReplenishHunger(temporary.satiationEffect);
            SurvivalManager.GetComponent<SurvivalManager>().ReplenishThirst(temporary.slakingOfThirstEffect);
            DeleteItem();
        }
    }

    public void DeleteItem()/*(GameObject CurrentCell)*/
    {
        ChosenCellExtra.GetComponent<InventoryCell>().item = AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Items/EmtyCell.asset", typeof(ItemsBase)) as ItemsBase;
        ChosenCellExtra.GetComponent<InventoryCell>().amount = 0;
        ChosenCellExtra.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = ChosenCellExtra.GetComponent<InventoryCell>().item.image;
    }
}
