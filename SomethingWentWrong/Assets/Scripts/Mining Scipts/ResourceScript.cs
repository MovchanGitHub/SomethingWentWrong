using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour, IDamagable
{
    [SerializeField] private float lootDropBarrier = 5;
    [SerializeField] private float hp;
    private float currentDamage;
    private float Health
    {
        set
        {
            if ((int)(((hp - value + currentDamage) / lootDropBarrier) ) >= 1)
            {
                DropItem((int)((hp - value + currentDamage) / lootDropBarrier) * dropCount);
                currentDamage = (hp - value + currentDamage) - lootDropBarrier * (int)((hp - value + currentDamage) / lootDropBarrier);
            }
            else
            {
                currentDamage += hp - value;
            }
            Debug.Log(currentDamage);
            hp = value;
        }
        get
        {
            return hp;
        }
    }

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
        Health -=  damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void DropItem(int dropCount)
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
