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
    public float attackTimer = 0f;

    private Animator anim;

    private bool attackWithRightHand = true;

    public List<AudioClip> _AudioClips;
    private AudioSource _punchSource;
    private int _punchInd;
    private AudioSource _crystallHit;
    private AudioSource _treeHit;
    
    void Awake()
    {
        startPosition = transform.localPosition;
        anim = transform.parent.GetComponentInChildren<Animator>();
        _punchSource = GetComponents<AudioSource>()[0];
        _crystallHit = GetComponents<AudioSource>()[1];
        _treeHit = GetComponents<AudioSource>()[2];
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
        
        _punchSource.clip = _AudioClips[_punchInd];
        _punchSource.pitch = 1 + Random.Range(-0.10f, 0.10f);
        _punchSource.Play();
        _punchInd++;
        if (_punchInd == 3)
            _punchInd = 0;
        
        yield return new WaitForSeconds(0.3f);

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, attackRange, damagableLayers);

        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.GetComponent<IDamagable>() != null)
            {
                hitObject.GetComponent<IDamagable>().GetDamage(this, GameManager.GM.PlayerMovement.gameObject);
            }
            
            ObjectHitSound(hitObject, _crystallHit, _treeHit);
        }
        
        yield return new WaitForSeconds(0.4f);
        GameManager.GM.PlayerMovement.usingWeapon = false;
        GameManager.GM.PlayerMovement.hand_to_hand = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public static void ObjectHitSound(Collider2D hitObject, AudioSource _crystallHit, AudioSource _treeHit)
    {
        if (hitObject.name.Contains("Tennosey"))
        {
            _crystallHit.pitch = 1 + Random.Range(-0.15f, 0.15f);
            _crystallHit.Play();
        }
        else if (hitObject.name.Contains("AguaBerryPlant Variant")
                 || hitObject.name.Contains("Bomb Fruit Plant")
                 || hitObject.name.Contains("FrambuesaBush Variant")
                 || hitObject.name.Contains("HomeOfBunzha Variant")
                 || hitObject.name.Contains("Bubble Plant Variant") 
                 || hitObject.name.Contains("Shoot Fruit Plant"))
        {
            _treeHit.pitch = 1 + Random.Range(-0.15f, 0.15f);
            _treeHit.Play();
        }
    }
}
