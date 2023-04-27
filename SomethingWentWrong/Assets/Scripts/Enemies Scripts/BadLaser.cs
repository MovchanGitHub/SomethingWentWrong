using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadLaser : MonoBehaviour
{
    private LaserEnemyAttack attackLogic;

    private void Start()
    {
        attackLogic = transform.parent.GetComponentInChildren<LaserEnemyAttack>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackLogic.damagedEntities.Contains(collision.gameObject))
        {
            IDamagable hit = collision.transform.GetComponentInChildren<IDamagable>();
            if (hit != null)
            {
                hit.GetDamage(attackLogic);
                attackLogic.damagedEntities.Add(collision.gameObject);
            }
        }
    }
}
