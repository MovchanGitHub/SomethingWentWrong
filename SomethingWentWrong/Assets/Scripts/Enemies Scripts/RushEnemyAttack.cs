using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemyAttack : EnemyAttack
{
    private float angle;
    private float rushStrength = 15;
    private float delay = 0.2f;
    private Rigidbody2D rigidBody2D;

    protected override void Awake()
    {
        base.Awake();
        rigidBody2D = GetComponentInParent<Rigidbody2D>();
    }

    protected override void Update()
    {
        distanceToTarget = Vector2.Distance(transform.position, enemyLogic.actualTarget.transform.position);
        direction = enemyLogic.actualTarget.transform.position - transform.position;
        if (distanceToTarget < triggerAttackDistance && enemyLogic.CanMove)
        {
            enemyLogic.CanMove = false;
            StartCoroutine(RushAttack(direction));
        }
    }

    private IEnumerator RushAttack(Vector2 direction)
    {
        if (isAttacking) yield break;
        
        isAttacking = true;
        es.Animator.AttackTrigger();
        enemyLogic.CanMove = false;
        yield return new WaitForSeconds(timeBeforeAttack);

        direction.Normalize();
        rigidBody2D.AddForce(direction * rushStrength, ForceMode2D.Impulse);
        StartCoroutine(Reset());

        es.Animator.StopAttackTrigger();
        yield return new WaitForSeconds(timeAfterAttack);
        
        enemyLogic.CanMove = true;
        isAttacking = false;
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rigidBody2D.velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable toDamage = collision.GetComponent<IDamagable>();
        if (toDamage != null)
        {
            toDamage.GetDamage(this);
        }
    }
}
