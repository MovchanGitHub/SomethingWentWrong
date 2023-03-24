using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    protected WeaponType type = WeaponType.Enemy;

    public WeaponType Type { get { return type; } }

    protected int damage = 5;

    public int Damage { get { return damage; } }

    [SerializeField] protected LayerMask damagableLayers;
    protected float distanceToTarget;
    protected EnemyMovement enemyLogic;
    protected float triggerAttackDistance = 1.5f;
    protected Vector2 direction;
    [HideInInspector] public EnemyScript es;

    private void Awake()
    {
        enemyLogic = GetComponentInParent<EnemyMovement>();
    }

    protected virtual void Update()
    {
        distanceToTarget = Vector2.Distance(transform.position, enemyLogic.actualTarget.transform.position);
        direction = enemyLogic.actualTarget.transform.position - transform.position;
        direction.Normalize();
        if (distanceToTarget < triggerAttackDistance && enemyLogic.CanMove)
        {
            enemyLogic.CanMove = false;
            if (es && es.Animator)
                es.Animator.ChangeXY(enemyLogic.actualTarget.transform.position - transform.position);
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        if (es && es.Animator)
            es.Animator.AttackTrigger();
        yield return new WaitForSeconds(0.3f);
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, triggerAttackDistance + 1f, damagableLayers);
        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.GetComponent<IDamagable>() != null)
            {
                hitObject.GetComponent<IDamagable>().GetDamage(this);
            }
        }
        yield return new WaitForSeconds(1f);
        if (es && es.Animator)
            es.Animator.StopAttackTrigger();
        enemyLogic.CanMove = true;
    }

    public virtual void stopAttack()
    {
        if (es && es.Animator)
            es.Animator.StopAttackTrigger();
        StopAllCoroutines();
    }


    /*
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

    public EnemyScript es;

    private bool isAttacking;
    
    private void Awake()
    {
        enemyLogic = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isAttacking && (col.tag == plantTag || col.tag == playerTag || col.tag == buildingTag))
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == plantTag || other.tag == playerTag || other.tag == buildingTag)
        {
            if (es && es.Animator)
            {
                es.Animator.WalkAnim();
            }

            enemyLogic.CanMove = true;
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        enemyLogic.CanMove = false;
        while (!enemyLogic.CanMove)
        {
            if (es && es.Animator)
            {
                es.Animator.AttackTrigger();
                yield return new WaitForSeconds(es.Animator.attackAnimationDuration);
                //es.Animator.StopAttackTrigger();
            }
            
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

        isAttacking = false;
    }
    */
}
