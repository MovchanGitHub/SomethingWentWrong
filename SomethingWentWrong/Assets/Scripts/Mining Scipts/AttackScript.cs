using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    private WeaponType type = WeaponType.Fists;
    
    public WeaponType Type { get { return type; } }

    private int damage = 5;

    public int Damage { get { return damage; } }
    
    
    // AttackScript's unique methods
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
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
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damagableLayers);

        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.GetComponent<IDamagable>() != null)
            {
                hitObject.GetComponent<IDamagable>().GetDamage(this);
            }
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
