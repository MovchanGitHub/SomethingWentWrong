using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance { get; private set; }
    [SerializeField] private GameObject[] inventoryCells;
    [SerializeField] private GameObject emptyCell;
    private InventoryCell emptyCellCode;

    [SerializeField] private GameObject ContextMenu;
    private float contextMenuWidthDiv2;
    private float contextMenuHeightDiv2;
    [SerializeField] private GameObject tipPanel;
    //[SerializeField] private GameObject SurvivalManager;
    private SurvivalManager SurvivalManagerCode;
    public GameObject BombSpawner;
    public GameObject BulletSpawner;
    public int bulletsAmount = 0;
    public int bombsAmount = 0;

    public GameObject InventoryPanel;
    private GameObject AlreadyChosenCell = null;
    private GameObject ChosenCellExtra = null;
    private Transform playerTransform;

    public bool isOpened;
    private bool shiftPressed;
    public bool canBeOpened = true;

    private GameObject CurrentCellRef;
    private GameObject tempCell;
    private GameObject onMouseObject;
    private BombLogic BombSpawnerCode;
    private Bullet BulletSpawnerCode;

    [SerializeField] private GameObject StandartBorsch;
    [SerializeField] private GameObject StandartEWR;
    [SerializeField] private GameObject StandartBeluga;
    [SerializeField] private GameObject StandartGematogen;

    public InGameMenuScript pause;

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
        BombSpawnerCode = BombSpawner.GetComponent<BombLogic>();
        BulletSpawnerCode = BulletSpawner.GetComponent<Bullet>();
        emptyCellCode = emptyCell.GetComponent<InventoryCell>();
        contextMenuWidthDiv2 = ContextMenu.GetComponent<RectTransform>().rect.width / 2;
        contextMenuHeightDiv2 = ContextMenu.GetComponent<RectTransform>().rect.height / 2;
        SurvivalManagerCode = SurvivalManager.Instance;

        CountOneTimeWeapon(BombLogic.bombName, BombSpawner.GetComponent<BombLogic>().SetAmountBombs);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !pause.isPaused)
        {
            isOpened = !isOpened;
            InventoryPanel.SetActive(isOpened);
            IsometricPlayerMovementController.Instance.IsAbleToMove = !isOpened;
            GameManagerScript.instance.isUIOpened = !GameManagerScript.instance.isUIOpened;
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

    public void SetDefault()
    {
        foreach (var cell in inventoryCells)
        {
            MakeCellEmpty(cell.GetComponent<InventoryCell>());
            UpdateCounterText(cell);
        }

        AddItem(StandartBorsch.GetComponent<PickUpScript>().itemToInventory);
        AddItem(StandartEWR.GetComponent<PickUpScript>().itemToInventory);
        AddItem(StandartBeluga.GetComponent<PickUpScript>().itemToInventory);
        AddItem(StandartGematogen.GetComponent<PickUpScript>().itemToInventory);
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
        InventoryCell currentCellCode = CurrentCell.GetComponent<InventoryCell>();

        //Åñëè ïåðâûé îáúåêò (ÿ÷åéêó) äëÿ Swap-à åù¸ íå âûáðàëè
        if (AlreadyChosenCell == null && currentCellCode.item.TypeOfThisItem != ItemType.NoItem)
        {
            //Ïðè íàæàòèè íà ïðàâóþ êíîïêó ìûøè âûñâå÷èâàåòñÿ êîíòåêñòíîå ìåíþ
            if (Input.GetMouseButtonUp(1))
            {
                ChosenCellExtra = CurrentCell;
                ShowContextMenu();
            }
            else
            {
                AlreadyChosenCell = Instantiate(CurrentCell);
                InventoryCell alreadyChosenCell = AlreadyChosenCell.GetComponent<InventoryCell>();
                CurrentCellRef = CurrentCell;
                tempCell = AlreadyChosenCell.transform.gameObject;

                if (shiftPressed)
                {
                    alreadyChosenCell.amount = 1;
                    currentCellCode.amount -= 1;
                    if (currentCellCode.amount < 1)
                    {
                        MakeCellEmpty(currentCellCode);
                    }
                }
                else
                {
                    MakeCellEmpty(currentCellCode);
                }

                onMouseObject = Instantiate(alreadyChosenCell.item.dragAndDropElement, transform);
                if (alreadyChosenCell.amount > 1)
                {
                    onMouseObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + alreadyChosenCell.amount;
                }
                onMouseObject.transform.position = Input.mousePosition;
            }
        }
        //Åñëè ïåðâûé îáúåêò (ÿ÷åéêó) äëÿ Swap-à óæå âûáðàëè
        else if (AlreadyChosenCell != null)
        {
            InventoryCell alreadyChosenCell = AlreadyChosenCell.GetComponent<InventoryCell>();
            InventoryCell сurrentCellRefCode = CurrentCellRef.GetComponent<InventoryCell>();
            InventoryCell tempCellCode = tempCell.GetComponent<InventoryCell>();

            if (alreadyChosenCell.item == currentCellCode.item && currentCellCode.amount <= currentCellCode.item.maximumAmount)
            {
                currentCellCode.amount += alreadyChosenCell.amount;
                int tempAmount = 0;

                if (currentCellCode.amount > currentCellCode.item.maximumAmount)
                {

                    tempAmount = currentCellCode.amount;
                    currentCellCode.amount = currentCellCode.item.maximumAmount;

                    сurrentCellRefCode.amount -= currentCellCode.amount - tempAmount;
                    сurrentCellRefCode.item = tempCellCode.item;
                    сurrentCellRefCode.icon.GetComponent<Image>().sprite = tempCellCode.item.image;
                    UpdateCounterText(CurrentCellRef);
                }

                alreadyChosenCell.amount -= currentCellCode.amount - tempAmount;
                alreadyChosenCell.item = tempCellCode.item;
                alreadyChosenCell.icon.GetComponent<Image>().sprite = tempCellCode.item.image;
            }
            else if (currentCellCode.item.TypeOfThisItem != ItemType.NoItem)
            {
                GameObject temporary = Instantiate(AlreadyChosenCell);
                InventoryCell temporaryCode = temporary.GetComponent<InventoryCell>();

                alreadyChosenCell.item = currentCellCode.item;
                alreadyChosenCell.amount = currentCellCode.amount;
                alreadyChosenCell.icon.GetComponent<Image>().sprite = currentCellCode.item.image;

                сurrentCellRefCode.item = temporaryCode.item;
                сurrentCellRefCode.amount += temporaryCode.amount;
                сurrentCellRefCode.icon.GetComponent<Image>().sprite = temporaryCode.item.image;
                UpdateCounterText(CurrentCellRef);

                Destroy(temporary);
            }
            else
            {
                GameObject temporary = Instantiate(AlreadyChosenCell);
                InventoryCell temporaryCode = temporary.GetComponent<InventoryCell>();

                alreadyChosenCell.item = currentCellCode.item;
                alreadyChosenCell.amount = currentCellCode.amount;
                alreadyChosenCell.icon.GetComponent<Image>().sprite = currentCellCode.item.image;

                currentCellCode.item = temporaryCode.item;
                currentCellCode.amount = temporaryCode.amount;
                currentCellCode.icon.GetComponent<Image>().sprite = temporaryCode.item.image;
                Destroy(temporary);
            }

            Destroy(onMouseObject);
            AlreadyChosenCell = null;
        }
        UpdateCounterText(CurrentCell);
    }


    //Âûáðàñûâàíèå ïðåäìåòîâ èç èíâåíòàðÿ
    public void OnDropZoneSpaceClick()
    {

        if (AlreadyChosenCell != null)
        {
            InventoryCell AlreadyChosenCellCode = AlreadyChosenCell.GetComponent<InventoryCell>();
            GameObject dropObject = Instantiate(AlreadyChosenCellCode.item.dropObject);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dropObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0);

            AlreadyChosenCellCode.amount -= 1;
            if (AlreadyChosenCellCode.amount > 1)
            {
                onMouseObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + AlreadyChosenCellCode.amount;
            }
            else
            {
                onMouseObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            }

            if (AlreadyChosenCellCode.amount < 1)
            {
                Destroy(onMouseObject);
                AlreadyChosenCell = null;
            }
        }


    }


    public void AddItem(ItemsBase newItem)
    {
        GameObject cellToAdd = FindCellToAdd(newItem);
        InventoryCell cellToAddCode = cellToAdd.GetComponent<InventoryCell>();
        if (cellToAddCode.item.TypeOfThisItem == ItemType.NoItem)
        {
            cellToAddCode.item = newItem;
            cellToAddCode.amount = 1;
            cellToAddCode.icon.GetComponent<Image>().sprite = newItem.image;
        }
        else
        {
            cellToAddCode.amount += 1;
        }
        
        if (newItem.name == BombLogic.bombName)
            bombsAmount++;
        if (newItem.name == BulletLogic.bulletName)
            bulletsAmount++;
            
        UpdateCounterText(cellToAdd);
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
        //InventoryCell cellCode = cell.GetComponent<InventoryCell>();
        cell.item = emptyCellCode.item;
        cell.amount = emptyCellCode.amount;
        cell.icon.GetComponent<Image>().sprite = emptyCellCode.item.image;
    }

    private void UpdateCounterText(GameObject cell)
    {
        if (cell.GetComponent<InventoryCell>().amount <= 1)
        {
            cell.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            cell.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + cell.GetComponent<InventoryCell>().amount;
        }
    }

    private void ShowContextMenu()
    {
        ContextMenu.transform.position = new Vector2(Input.mousePosition.x + contextMenuWidthDiv2, Input.mousePosition.y - contextMenuHeightDiv2);
        ContextMenu.SetActive(true);
    }

    public void UseItem(GameObject Cell) 
    {
        if (Cell == ContextMenu)
            Cell = ChosenCellExtra;
        InventoryCell CellCode = Cell.GetComponent<InventoryCell>();
        if ((!Input.GetMouseButtonUp(1)) && (CellCode.item.TypeOfThisItem == ItemType.Food))
        {
            ItemTypeFood temporary = CellCode.item as ItemTypeFood;
            SurvivalManagerCode.ReplenishHunger(temporary.satiationEffect);
            SurvivalManagerCode.ReplenishThirst(temporary.slakingOfThirstEffect);
            SurvivalManagerCode.ReplenishAnoxaemia(temporary.oxygenRecovery);
        }

        CellCode.amount--;
        if (CellCode.amount <= 0)
            DeleteItem(Cell);
        
        UpdateCounterText(Cell);
    }

    public void DeleteItem(GameObject Cell)
    {
        if (Cell == ContextMenu)
            Cell = ChosenCellExtra;
        InventoryCell cellCode = Cell.GetComponent<InventoryCell>();
        if (!Input.GetMouseButtonUp(1))
        {
            cellCode.item = emptyCellCode.item;
            cellCode.amount = emptyCellCode.amount;
            cellCode.icon.GetComponent<Image>().sprite = emptyCellCode.item.image;
        }
    }

    public void ShowTipPanel(GameObject CurrentCell)
    {
        StartCoroutine(CoroutineShowTipPanel(CurrentCell));
    }

    IEnumerator CoroutineShowTipPanel(GameObject CurrentCell)
    {
        //Debug.Log("Start");
        yield return new WaitForSeconds(1.0f);
        //Debug.Log("End");
        if (CurrentCell.GetComponent<InventoryCell>().item.TypeOfThisItem == ItemType.Food)
        {
            ItemTypeFood temporary = CurrentCell.GetComponent<InventoryCell>().item as ItemTypeFood;

            if (temporary.satiationEffect > 0)
                tipPanel.transform.GetChild(0).GetComponent<Text>().color = Color.green;
            else if (temporary.satiationEffect < 0)
                tipPanel.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            tipPanel.transform.GetChild(0).GetComponent<Text>().text = temporary.satiationEffect.ToString();

            if (temporary.slakingOfThirstEffect > 0)
                tipPanel.transform.GetChild(1).GetComponent<Text>().color = Color.green;
            else if (temporary.slakingOfThirstEffect < 0)
                tipPanel.transform.GetChild(1).GetComponent<Text>().color = Color.red;
            tipPanel.transform.GetChild(1).GetComponent<Text>().text = temporary.slakingOfThirstEffect.ToString();

            if (temporary.oxygenRecovery > 0)
                tipPanel.transform.GetChild(2).GetComponent<Text>().color = Color.green;
            else if (temporary.oxygenRecovery < 0)
                tipPanel.transform.GetChild(2).GetComponent<Text>().color = Color.red;
            tipPanel.transform.GetChild(2).GetComponent<Text>().text = temporary.oxygenRecovery.ToString();

            if (temporary.healEffect > 0)
                tipPanel.transform.GetChild(3).GetComponent<Text>().color = Color.green;
            else if (temporary.healEffect < 0)
                tipPanel.transform.GetChild(3).GetComponent<Text>().color = Color.red;
            tipPanel.transform.GetChild(3).GetComponent<Text>().text = temporary.healEffect.ToString();

            tipPanel.SetActive(true);
        }
    }

    public void OnCursorExit()
    {
        //Debug.Log("Exit");

        tipPanel.SetActive(false);
        tipPanel.transform.GetChild(0).GetComponent<Text>().text = "0";
        tipPanel.transform.GetChild(0).GetComponent<Text>().color = Color.black;
        tipPanel.transform.GetChild(1).GetComponent<Text>().text = "0";
        tipPanel.transform.GetChild(1).GetComponent<Text>().color = Color.black;
        tipPanel.transform.GetChild(2).GetComponent<Text>().text = "0";
        tipPanel.transform.GetChild(2).GetComponent<Text>().color = Color.black;
        tipPanel.transform.GetChild(3).GetComponent<Text>().text = "0";
        tipPanel.transform.GetChild(3).GetComponent<Text>().color = Color.black;
        StopCoroutine("CoroutineShowTipPanel");
    }

    public void CountOneTimeWeapon(string nameOfWeapon, System.Action<int> setter)
    {
        int newAmount = bombsAmount;
        for (int i = 0; i < 16; i++)
        {
            if (inventoryCells[i].GetComponent<InventoryCell>().item.name == nameOfWeapon)
                newAmount += inventoryCells[i].GetComponent<InventoryCell>().amount;
        }
        bombsAmount = newAmount;
    }


    public void UseOneTimeWeapon(string nameOfWeapon)
    {
        for (int i = 0; i < 16; i++)
        {
            if (inventoryCells[i].GetComponent<InventoryCell>().item.name == nameOfWeapon)
            {
                UseItem(inventoryCells[i]);
                return;
            }
        }
    }
}
