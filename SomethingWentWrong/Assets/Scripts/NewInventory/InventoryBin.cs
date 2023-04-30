using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryBin : MonoBehaviour
{

    private bool isDeleting = false;

    [SerializeField] private float speed = 100;

    public Image itemToDelete;

    public void onBinClick()
    {
        if (GameManager.GM.InventoryManager.SelectedItem == null)
        {
            return;
        }
        
        if (GameManager.GM.IsTutorial && GameManager.GM.Tutorial.checkForErasing)
            GameManager.GM.Tutorial.InventoryEraseSomething();

        if (GameManager.GM.InventoryManager.SelectedItem.itemData.TypeOfThisItem == ItemType.Weapon && GameManager.GM.InventoryManager.SelectedItem.itemData.itemName != "spaceTrash")
        {
            --GameManager.GM.InventoryManager.AmmoCounter[GameManager.GM.InventoryManager.SelectedItem.itemData.itemName];
            GameManager.GM.InventoryManager.UpdateWeaponBar(GameManager.GM.InventoryManager.SelectedItem.itemData.itemName, GameManager.GM.InventoryManager.AmmoCounter[GameManager.GM.InventoryManager.SelectedItem.itemData.itemName]);
        }
        itemToDelete.transform.position = Mouse.current.position.ReadValue();
        itemToDelete.sprite = GameManager.GM.InventoryManager.SelectedItem.itemData.image;
        itemToDelete.transform.localScale = new Vector3(GameManager.GM.InventoryManager.SelectedItem.itemData.width, GameManager.GM.InventoryManager.SelectedItem.itemData.height, itemToDelete.transform.localScale.z);
        itemToDelete.gameObject.SetActive(true);
        isDeleting = true;
        GameManager.GM.InventoryManager.deleteItem();
    }

    private void Update()
    {
        if (!isDeleting)
        {
            return;
        }

        Vector3 direction = transform.position - itemToDelete.transform.position;
        direction = Quaternion.Euler(0, 0, 75) * direction;
        float distanceThisFrame = speed * Time.deltaTime;

        itemToDelete.transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        itemToDelete.transform.localScale = new Vector3(itemToDelete.transform.localScale.x - 0.5f * Time.deltaTime, itemToDelete.transform.localScale.y - 0.5f * Time.deltaTime, itemToDelete.transform.localScale.z);

        if (Vector2.Distance(transform.position, itemToDelete.transform.position) < 2)
        {
            itemToDelete.gameObject.SetActive(false);
            isDeleting = false;
            if (GameManager.GM.InventoryManager.SelectedItem == null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
