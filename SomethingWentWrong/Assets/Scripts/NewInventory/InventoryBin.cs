using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryBin : MonoBehaviour
{

    private bool isDeleting = false;

    private Vector2 startPosition;

    [SerializeField] private float speed = 100;

    public Image itemToDelete;

    private void Start()
    {
        startPosition = itemToDelete.transform.position;
    }

    public void onBinClick()
    {
        if (GameManager.GM.InventoryManager.SelectedItem.itemData.TypeOfThisItem == ItemType.Weapon)
        {
            --GameManager.GM.InventoryManager.AmmoCounter[GameManager.GM.InventoryManager.SelectedItem.itemData.itemName];
            GameManager.GM.InventoryManager.UpdateWeaponBar(GameManager.GM.InventoryManager.SelectedItem.itemData.itemName, GameManager.GM.InventoryManager.AmmoCounter[GameManager.GM.InventoryManager.SelectedItem.itemData.itemName]);
        }
        itemToDelete.transform.position = Mouse.current.position.ReadValue();
        itemToDelete.sprite = GameManager.GM.InventoryManager.SelectedItem.itemData.image;
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

        if (Vector2.Distance(transform.position, itemToDelete.transform.position) < 2)
        {
            itemToDelete.gameObject.SetActive(false);
            itemToDelete.transform.position = startPosition;
            isDeleting = false;
            if (GameManager.GM.InventoryManager.SelectedItem == null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
