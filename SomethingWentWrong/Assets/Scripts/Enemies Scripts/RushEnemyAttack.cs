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
        //es.Animator.AttackTrigger();
        yield return new WaitForSeconds(2f);

        direction.Normalize();
        rigidBody2D.AddForce(direction * rushStrength, ForceMode2D.Impulse);
        StartCoroutine(Reset());

        //es.Animator.StopAttackTrigger();
    }

    private IEnumerator Reset()
    {
        //if (es && es.Animator)
        //es.Animator.IdleAnim();
        yield return new WaitForSeconds(delay);
        rigidBody2D.velocity = Vector3.zero;
        enemyLogic.CanMove = true;
    }
}
