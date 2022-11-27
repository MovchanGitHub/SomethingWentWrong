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

    [SerializeField] private GameObject ContextMenu;
    [SerializeField] private GameObject tipPanel;
    [SerializeField] private GameObject SurvivalManager;
    [SerializeField] private GameObject BombSpawner;
    [SerializeField] private GameObject BulletSpawner;

    public GameObject InventoryPanel;
    private GameObject AlreadyChosenCell = null;
    private GameObject ChosenCellExtra = null;
    private Transform playerTransform;

    public bool isOpened;
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

        CountOneTimeWeapon(PlayerBombSpawnerScript.bombName, BombSpawner.GetComponent<PlayerBombSpawnerScript>().SetAmountBombs);
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
                ShowContextMenu();
            }
            else
            {
                AlreadyChosenCell = Instantiate(CurrentCell);
                CurrentCellRef = CurrentCell;
                tempCell = AlreadyChosenCell.transform.gameObject;

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

                onMouseObject = Instantiate(AlreadyChosenCell.GetComponent<InventoryCell>().item.dragAndDropElement, transform);
                if (AlreadyChosenCell.GetComponent<InventoryCell>().amount > 1)
                {
                    onMouseObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + AlreadyChosenCell.GetComponent<InventoryCell>().amount;
                }
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
                    UpdateCounterText(CurrentCellRef);
                }

                alreadyChosenCell.amount -= currentCell.amount - tempAmount;
                alreadyChosenCell.item = tempCell.GetComponent<InventoryCell>().item;
                alreadyChosenCell.icon.GetComponent<Image>().sprite = tempCell.GetComponent<InventoryCell>().item.image;
            }
            else if (currentCell.item.TypeOfThisItem != ItemType.NoItem)
            {
                GameObject temporary = Instantiate(AlreadyChosenCell);

                alreadyChosenCell.item = currentCell.item;
                alreadyChosenCell.amount = currentCell.amount;
                alreadyChosenCell.icon.GetComponent<Image>().sprite = currentCell.item.image;

                CurrentCellRef.GetComponent<InventoryCell>().item = temporary.GetComponent<InventoryCell>().item;
                CurrentCellRef.GetComponent<InventoryCell>().amount += temporary.GetComponent<InventoryCell>().amount;
                CurrentCellRef.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = temporary.GetComponent<InventoryCell>().item.image;
                UpdateCounterText(CurrentCellRef);

                Destroy(temporary);
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
        UpdateCounterText(CurrentCell);
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
            if (AlreadyChosenCell.GetComponent<InventoryCell>().amount > 1)
            {
                onMouseObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + AlreadyChosenCell.GetComponent<InventoryCell>().amount;
            }
            else
            {
                onMouseObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            }

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
        
        if (newItem.name == PlayerBombSpawnerScript.bombName)
            BombSpawner.GetComponent<PlayerBombSpawnerScript>().SetAmountBombs(BombSpawner.GetComponent<PlayerBombSpawnerScript>().GetAmountBombs() + 1);
        if (newItem.name == Bullet.bulletName)
            BulletSpawner.GetComponent<Bullet>().SetAmountBullets(BulletSpawner.GetComponent<Bullet>().GetAmountBullets() + 1);
            
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
        cell.GetComponent<InventoryCell>().item = emptyCell.GetComponent<InventoryCell>().item;
        cell.GetComponent<InventoryCell>().amount = emptyCell.GetComponent<InventoryCell>().amount;
        cell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = emptyCell.GetComponent<InventoryCell>().item.image;
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
        ContextMenu.transform.position = new Vector2(Input.mousePosition.x + ContextMenu.GetComponent<RectTransform>().rect.width / 2, Input.mousePosition.y - ContextMenu.GetComponent<RectTransform>().rect.height / 2);
        ContextMenu.SetActive(true);
    }

    public void UseItem(GameObject Cell) 
    {
        if((!Input.GetMouseButtonUp(1)) && (Cell.GetComponent<InventoryCell>().item.TypeOfThisItem == ItemType.Food))
        {
            ItemTypeFood temporary = Cell.GetComponent<InventoryCell>().item as ItemTypeFood;
            SurvivalManager.GetComponent<SurvivalManager>().ReplenishHunger(temporary.satiationEffect);
            SurvivalManager.GetComponent<SurvivalManager>().ReplenishThirst(temporary.slakingOfThirstEffect);
        }
        
        Cell.GetComponent<InventoryCell>().amount--;
        if (Cell.GetComponent<InventoryCell>().amount <= 0)
            DeleteItem(Cell);
        
        UpdateCounterText(Cell);
    }

    public void DeleteItem(GameObject Cell)
    {
        if (!Input.GetMouseButtonUp(1))
        {
            Cell.GetComponent<InventoryCell>().item = AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Items/EmtyCell.asset", typeof(ItemsBase)) as ItemsBase;
            Cell.GetComponent<InventoryCell>().amount = 0;
            Cell.GetComponent<InventoryCell>().icon.GetComponent<Image>().sprite = Cell.GetComponent<InventoryCell>().item.image;
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

            //tipPanel.transform.GetChild(2).GetComponent<Text>().text = temporary.oxigenEffect.ToString();

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
        tipPanel.transform.GetChild(1).GetComponent<Text>().text = "0";
        tipPanel.transform.GetChild(2).GetComponent<Text>().text = "0";
        tipPanel.transform.GetChild(3).GetComponent<Text>().text = "0";
        StopCoroutine("CoroutineShowTipPanel");
    }

    public void CountOneTimeWeapon(string nameOfWeapon, System.Action<int> setter)
    {
        int newAmount = BombSpawner.GetComponent<PlayerBombSpawnerScript>().GetAmountBombs();
        for (int i = 0; i < 16; i++)
        {
            if (inventoryCells[i].GetComponent<InventoryCell>().item.name == nameOfWeapon)
                newAmount += inventoryCells[i].GetComponent<InventoryCell>().amount;
        }
        BombSpawner.GetComponent<PlayerBombSpawnerScript>().SetAmountBombs(newAmount);
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
