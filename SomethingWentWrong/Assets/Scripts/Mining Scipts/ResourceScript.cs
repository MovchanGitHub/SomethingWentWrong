using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour, Damagable
{
    [SerializeField] private int hp;
    [SerializeField] private GameObject drop;
    [SerializeField] private float spread = 2f;
    [SerializeField] private float dropSpeed = 5f;

    public void doDamage(int damage)
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
        Vector3 pos = transform.position;
        pos.x += spread * UnityEngine.Random.value - spread / 2;
        pos.y += spread * UnityEngine.Random.value - spread / 2;
        GameObject dropObject = Instantiate(drop);
        dropObject.transform.position = transform.position;
        StartCoroutine(dropObject.GetComponent<PickUpScript>().MoveDropRoutine(pos, dropSpeed));
    }
}
