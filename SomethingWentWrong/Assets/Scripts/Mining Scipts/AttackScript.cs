using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int damage = 5;
    public LayerMask damagableLayers;

    [SerializeField] private float attackRate = 2f;
    private float attackTimer = 0f;

    private void Update()
    {
        if (Time.time >= attackTimer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                attackTimer = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        //Запуск анимации нужен тут

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damagableLayers);

        foreach (Collider2D hitObject in hitObjects)
        {
            hitObject.GetComponent<Damagable>().doDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
