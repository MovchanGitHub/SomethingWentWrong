using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed = 4f;
    [SerializeField] float pickUpDistance = 2.5f;
    [SerializeField] float despawnTime = 10f;
    [SerializeField] ItemsBase itemToInventory;

    private void Start()
    {
        player = GameManagerScript.instance.player.transform;
    }

    private void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime < 0)
        {
            Destroy(gameObject);
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pickUpDistance && !InventoryManager.instance.IsInventoryFull())
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (distance < 0.1f)
            {
                PickUp();
            }
        }

    }

    private void PickUp()
    {
        InventoryManager.instance.AddItem(itemToInventory);
        Destroy(gameObject);
    }
}
