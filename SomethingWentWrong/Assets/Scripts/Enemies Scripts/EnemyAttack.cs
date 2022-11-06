using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public string resourceTag;
    public LayerMask damagableLayers;
    private EnemyMovement enemyLogic;
    public float coolDown;

    private void Awake()
    {
        enemyLogic = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == resourceTag)
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == resourceTag)
        {
            enemyLogic.canMove = true;
        }
    }

    private IEnumerator Attack()
    {
        enemyLogic.canMove = false;
        while (!enemyLogic.canMove)
        {
            yield return new WaitForSeconds(coolDown);
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, 1f, damagableLayers);

            foreach (Collider2D hitObject in hitObjects)
            {
                hitObject.GetComponent<DamagableScript.Damagable>().doDamage(5);
            } 
        }
    }
}
