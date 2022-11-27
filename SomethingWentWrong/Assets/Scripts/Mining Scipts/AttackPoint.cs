using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int damage = 5;
    public LayerMask damagableLayers;

    [SerializeField] private float attackRate = 2f;
    private float attackTimer = 0f;

    void Awake()
    {
        startPosition = transform.localPosition;
    }

    void Update()
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

    private void Attack()
    {
        //Запуск анимации нужен тут


        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, attackRange, damagableLayers);

        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.GetComponent<IDamagable>() != null)
            {
                hitObject.GetComponent<IDamagable>().GetDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
