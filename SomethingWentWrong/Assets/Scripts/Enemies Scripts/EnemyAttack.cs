using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    public WeaponType type;

    public WeaponType Type { get { return type; } }

    [SerializeField] protected int damage = 5;

    public int Damage { get { return damage; } }

    protected bool isAttacking = false;

    [SerializeField] protected LayerMask damagableLayers;
    protected float distanceToTarget;
    protected EnemyMovement enemyLogic;
    [SerializeField] protected float triggerAttackDistance = 1.5f;
    protected Vector2 direction;
    [HideInInspector] public EnemyScript es;
    [SerializeField] protected float timeBeforeAttack = 0.5f;
    [SerializeField] protected float timeAfterAttack = 1f;
    [SerializeField] protected float attackRange = 0.5f;

    protected virtual void Awake()
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
            es.Animator.ChangeXY(enemyLogic.actualTarget.transform.position - transform.position);
            enemyLogic.attackPoint.transform.localPosition = new Vector3(0 - 0.8f * Math.Sign(enemyLogic.direction.x), 1 - 0.8f * Math.Sign(enemyLogic.direction.y), enemyLogic.attackPoint.transform.position.z);
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        es.Animator.AttackTrigger();
        
        yield return new WaitForSeconds(timeBeforeAttack);
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(enemyLogic.attackPoint.transform.position, 1.4f, damagableLayers);
        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.GetComponent<IDamagable>() != null)
            {
                hitObject.GetComponent<IDamagable>().GetDamage(this);
            }
        }
        es.Animator.StopAttackTrigger();
        yield return new WaitForSeconds(timeAfterAttack);
        
        enemyLogic.CanMove = true;
        isAttacking = false;
    }

    public virtual void stopAttack()
    {
        StopAllCoroutines();
        es.Animator.StopAttackTrigger();
        StartCoroutine(StopAttackCoroutine());
    }

    protected IEnumerator StopAttackCoroutine()
    {
        yield return new WaitForSeconds(timeAfterAttack);
        enemyLogic.CanMove = true;
        isAttacking = false;
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
