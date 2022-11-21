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

    [SerializeField] private GameObject ContextMenu;
    [SerializeField] private GameObject tipPanel;
    [SerializeField] private GameObject SurvivalManager;

    public GameObject InventoryPanel;
    private GameObject AlreadyChosenCell = null;
    private GameObject ChosenCellExtra = null;
    private Transform playerTransform;

    private bool isOpened;
    private bool shiftPressed;

    private GameObject CurrentCellRef;
    private GameObject tempCell;
    private GameObject onMouseObject;


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
                MakeCellEmpty(CurrentCellRef.GetComponent<InventoryCell>());
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftPressed = true;
        }
        else
        {
            shiftPressed = false;
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
        InventoryCell currentCell = CurrentCell.GetComponent<InventoryCell>();

        //Åñëè ïåðâûé îáúåêò (ÿ÷åéêó) äëÿ Swap-à åù¸ íå âûáðàëè
        if (AlreadyChosenCell == null && currentCell.item.TypeOfThisItem != ItemType.NoItem)
        {
            //Ïðè íàæàòèè íà ïðàâóþ êíîïêó ìûøè âûñâå÷èâàåòñÿ êîíòåêñòíîå ìåíþ
            if (Input.GetMouseButtonUp(1))
            {
                ChosenCellExtra = CurrentCell;
                ShowContextMenu(CurrentCell);
            }
            else
            {
                AlreadyChosenCell = Instantiate(CurrentCell);
                CurrentCellRef = CurrentCell;

                if (shiftPressed)
                {
                    AlreadyChosenCell.GetComponent<InventoryCell>().amount = 1;
                    currentCell.amount -= 1;
                    if (currentCell.amount < 1)
                    {
                        MakeCellEmpty(currentCell);
                    }
                }
                else
                {
                    MakeCellEmpty(currentCell);
                }

                tempCell = AlreadyChosenCell.transform.gameObject;
                onMouseObject = Instantiate(AlreadyChosenCell.GetComponent<InventoryCell>().item.dragAndDropElement, transform);
                onMouseObject.transform.position = Input.mousePosition;
            }
        }
        //Åñëè ïåðâûé îáúåêò (ÿ÷åéêó) äëÿ Swap-à óæå âûáðàëè
        else if (AlreadyChosenCell != null)
        {
            InventoryCell alreadyChosenCell = AlreadyChosenCell.GetComponent<InventoryCell>();

            if (alreadyChosenCell.item == currentCell.item && currentCell.amount <= currentCell.item.maximumAmount)
            {
                currentCell.amount += alreadyChosenCell.amount;
                int tempAmount = 0;

                if (currentCell.amount > currentCell.item.maximumAmount)
                {

                    tempAmount = CurrentCell.GetComponent<InventoryCell>().amount;
                    currentCell.amount = currentCell.item.maximumAmount;

                    CurrentCellRef.GetComponent<InventoryCell>().amount -= currentCell.amount - tempAmount;
                    CurrentCellRef.GetComponent<InventoryCell>().item = tempCell.GetComponent<InventoryCell>().item;
                    CurrentCellRef.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = tempCell.GetComponent<InventoryCell>().item.image;

                }

                alreadyChosenCell.amount -= currentCell.amount - tempAmount;
                alreadyChosenCell.item = tempCell.GetComponent<InventoryCell>().item;
                alreadyChosenCell.icon.GetComponent<Image>().sprite = tempCell.GetComponent<InventoryCell>().item.image;
            }
            else
            {
                GameObject temporary = Instantiate(AlreadyChosenCell);

                alreadyChosenCell.item = currentCell.item;
                alreadyChosenCell.amount = currentCell.amount;
                alreadyChosenCell.icon.GetComponent<Image>().sprite = currentCell.item.image;

                currentCell.item = temporary.GetComponent<InventoryCell>().item;
                currentCell.amount = temporary.GetComponent<InventoryCell>().amount;
                currentCell.icon.GetComponent<Image>().sprite = temporary.GetComponent<InventoryCell>().item.image;

                Destroy(temporary);
            }

            Destroy(onMouseObject);
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

            AlreadyChosenCell.GetComponent<InventoryCell>().amount -= 1;
            if (AlreadyChosenCell.GetComponent<InventoryCell>().amount < 1)
            {
                Destroy(onMouseObject);
                AlreadyChosenCell = null;
            }
        }


    }


    public void AddItem(ItemsBase newItem)
    {
        GameObject cellToAdd = FindCellToAdd(newItem);
        if (cellToAdd.GetComponent<InventoryCell>().item.TypeOfThisItem == ItemType.NoItem)
        {
            cellToAdd.GetComponent<InventoryCell>().item = newItem;
            cellToAdd.GetComponent<InventoryCell>().amount = 1;
            cellToAdd.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = newItem.image;
        }
        else
        {
            cellToAdd.GetComponent<InventoryCell>().amount += 1;
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


    private GameObject FindCellToAdd(ItemsBase newItem)
    {
        GameObject cellToAdd = Instantiate(emptyCell);
        foreach (GameObject cell in inventoryCells)
        {
            if (cell.GetComponent<InventoryCell>().item.TypeOfThisItem == ItemType.NoItem)
            {
                cellToAdd = cell;
                break;
            }
        }

        foreach (GameObject cell in inventoryCells)
        {
            if (cell.GetComponent<InventoryCell>().item == newItem && cell.GetComponent<InventoryCell>().amount < cell.GetComponent<InventoryCell>().item.maximumAmount)
            {
                cellToAdd = cell;
                break;
            }
        }

        return cellToAdd;
    }


    private void MakeCellEmpty(InventoryCell cell)
    {
        cell.GetComponent<InventoryCell>().item = emptyCell.GetComponent<InventoryCell>().item;
        cell.GetComponent<InventoryCell>().amount = emptyCell.GetComponent<InventoryCell>().amount;
        cell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = emptyCell.GetComponent<InventoryCell>().item.image;
    }

    private void ShowContextMenu()
    {
        ContextMenu.transform.position = new Vector2(Input.mousePosition.x + ContextMenu.GetComponent<RectTransform>().rect.width / 2, Input.mousePosition.y - ContextMenu.GetComponent<RectTransform>().rect.height / 2);
        ContextMenu.SetActive(true);
    }

    public void UseItem() 
    {
        if((!Input.GetMouseButtonUp(1)) && (ChosenCellExtra.GetComponent<InventoryCell>().item.TypeOfThisItem == ItemType.Food))
        {
            ItemTypeFood temporary = ChosenCellExtra.GetComponent<InventoryCell>().item as ItemTypeFood;
            SurvivalManager.GetComponent<SurvivalManager>().ReplenishHunger(temporary.satiationEffect);
            SurvivalManager.GetComponent<SurvivalManager>().ReplenishThirst(temporary.slakingOfThirstEffect);
            DeleteItem();
        }
    }

    public void DeleteItem()
    {
        if (!Input.GetMouseButtonUp(1))
        {
            ChosenCellExtra.GetComponent<InventoryCell>().item = AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Items/EmtyCell.asset", typeof(ItemsBase)) as ItemsBase;
            ChosenCellExtra.GetComponent<InventoryCell>().amount = 0;
            ChosenCellExtra.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = ChosenCellExtra.GetComponent<InventoryCell>().item.image;
        }
    }

    public void ShowTipPanel(GameObject CurrentCell)
    {
        StartCoroutine(CoroutineShowTipPanel(CurrentCell));
    }

    IEnumerator CoroutineShowTipPanel(GameObject CurrentCell)
    {
        Debug.Log("Start");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("End");
        if (CurrentCell.GetComponent<InventoryCell>().item.TypeOfThisItem == ItemType.Food)
        {
            ItemTypeFood temporary = CurrentCell.GetComponent<InventoryCell>().item as ItemTypeFood;
            tipPanel.transform.GetChild(0).GetComponent<Text>().text = temporary.satiationEffect.ToString();
            tipPanel.transform.GetChild(1).GetComponent<Text>().text = temporary.slakingOfThirstEffect.ToString();
            //tipPanel.transform.GetChild(2).GetComponent<Text>().text = temporary.oxigenEffect.ToString();
            tipPanel.transform.GetChild(3).GetComponent<Text>().text = temporary.healEffect.ToString();
            tipPanel.SetActive(true);
        }
    }

    public void OnCursorExit()
    {
        Debug.Log("Exit");

        tipPanel.SetActive(false);
        tipPanel.transform.GetChild(0).GetComponent<Text>().text = "0";
        tipPanel.transform.GetChild(1).GetComponent<Text>().text = "0";
        tipPanel.transform.GetChild(2).GetComponent<Text>().text = "0";
        tipPanel.transform.GetChild(3).GetComponent<Text>().text = "0";
        StopCoroutine("CoroutineShowTipPanel");
    }


}
