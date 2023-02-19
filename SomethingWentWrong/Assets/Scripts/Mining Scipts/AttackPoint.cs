using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    private WeaponType type = WeaponType.Fists;
    
    public WeaponType Type { get { return type; } }

    private int damage = 5;

    public int Damage { get { return damage; } }
    
    
    // AttackPoint's unique methods
    private Vector3 startPosition;
    [SerializeField] private float attackRange = 0.5f;
    public LayerMask damagableLayers;

    [SerializeField] private float attackRate = 2f;
    private float attackTimer = 0f;

    private Animator anim;

    private bool attackWithRightHand = true;
    
    void Awake()
    {
        startPosition = transform.localPosition;
        anim = transform.parent.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Time.time >= attackTimer)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !IsometricPlayerMovementController.Instance.usingWeapon)
            {
                StartCoroutine(Attack());
                attackTimer = Time.time + 1f / attackRate;
            }
        }
    }

    private IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        anim.SetBool("RightHand", attackWithRightHand);
        IsometricPlayerMovementController.Instance.usingWeapon = true;
        IsometricPlayerMovementController.Instance.hand_to_hand = true;
        attackWithRightHand = !attackWithRightHand;

        yield return new WaitForSeconds(0.3f);

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, attackRange, damagableLayers);

        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.GetComponent<IDamagable>() != null)
            {
                hitObject.GetComponent<IDamagable>().GetDamage(this);
            }
        }
        
        yield return new WaitForSeconds(0.4f);
        IsometricPlayerMovementController.Instance.usingWeapon = false;
        IsometricPlayerMovementController.Instance.hand_to_hand = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
