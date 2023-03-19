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

    public void TryAttack(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (Time.time >= attackTimer && !GameManager.GM.PlayerMovement.usingWeapon)
        {
            StartCoroutine(Attack());
            attackTimer = Time.time + 1f / attackRate;
        }
    }

    private IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        anim.SetBool("RightHand", attackWithRightHand);
        GameManager.GM.PlayerMovement.usingWeapon = true;
        GameManager.GM.PlayerMovement.hand_to_hand = true;
        attackWithRightHand = !attackWithRightHand;

        yield return new WaitForSeconds(0.3f);

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, attackRange, damagableLayers);

        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.GetComponent<IDamagable>() != null)
            {
                hitObject.GetComponent<IDamagable>().GetDamage(this, GameManager.GM.PlayerMovement.gameObject);
            }
        }
        
        yield return new WaitForSeconds(0.4f);
        GameManager.GM.PlayerMovement.usingWeapon = false;
        GameManager.GM.PlayerMovement.hand_to_hand = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
