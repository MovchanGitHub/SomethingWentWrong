using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour, IDamagable
{
    [SerializeField] private float hp;
    [SerializeField] private GameObject drop;
    [SerializeField] private int dropCount = 1;
    [SerializeField] private float spread = 2f;
    [SerializeField] private float dropSpeed = 5f;

    public float HP()
    {
        return hp;
    }

    public void GetDamage(float damage)
    {
        hp -= damage;
        DropItem();
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void DropItem()
    {
        int amountOfDrop = dropCount;
        while (amountOfDrop > 0)
        {
            amountOfDrop--;
            Vector3 pos = transform.position;
            pos.x += spread * UnityEngine.Random.value - spread / 2;
            pos.y += spread * UnityEngine.Random.value - spread / 2;
            GameObject dropObject = Instantiate(drop);
            dropObject.transform.position = transform.position;
            dropObject.GetComponent<PickUpScript>().StartMove(pos, dropSpeed);
        }
    }
}
