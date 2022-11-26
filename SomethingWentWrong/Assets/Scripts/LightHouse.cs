using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHouse : MonoBehaviour, IDamagable
{
    [SerializeField] private float hp;

    public float HP()
    {
        return hp;
    }

    public void GetDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("GameOver!!!");

        Destroy(gameObject);
    }

}
