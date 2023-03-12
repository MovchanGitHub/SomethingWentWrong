using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    private WeaponType type = WeaponType.Enemy;
    
    public WeaponType Type { get { return type; } }

    private int damage = 5;

    public int Damage { get { return damage; } }
    
    
    // EnemyAttack's unique values
    public string plantTag;
    public string playerTag;
    public string buildingTag;
    public LayerMask damagableLayers;
    private EnemyMovement enemyLogic;
    public float coolDown;
    
    private void Awake()
    {
        enemyLogic = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == plantTag || col.tag == playerTag || col.tag == buildingTag)
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == plantTag || other.tag == playerTag || other.tag == buildingTag)
        {
            enemyLogic.canMove = true;
        }
    }

    private IEnumerator Attack()
    {
        enemyLogic.canMove = false;
        while (!enemyLogic.canMove)
        {
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, 1.5f, damagableLayers);

            foreach (Collider2D hitObject in hitObjects)
            {
                if (hitObject.GetComponent<IDamagable>() != null)
                {
                    hitObject.GetComponent<IDamagable>().GetDamage(this);
                }
            }
            yield return new WaitForSeconds(coolDown);
        }
    }
}
