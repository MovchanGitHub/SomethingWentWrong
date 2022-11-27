using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed = 6f;
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

    public void StartMove(Vector3 to, float speed)
    {
        StartCoroutine(MoveDropRoutine(to, speed));
    }

    public IEnumerator MoveDropRoutine(Vector3 to, float speed)
    {
        Vector3 from = transform.position;
        float distance = Vector3.Distance(from, to);
        float rate = speed / distance;

        for (float t = 0; t < 1; t += rate * Time.deltaTime)
        {
            transform.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        transform.position = to;
    }
}
