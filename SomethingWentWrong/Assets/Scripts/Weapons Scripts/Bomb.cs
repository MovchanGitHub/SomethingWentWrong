using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bomb : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    [SerializeField] private WeaponType type;
    
    public WeaponType Type { get { return type; } }

    [SerializeField] private int damage;

    public int Damage { get { return damage; } }
    
    
    // Bomb's unique values
    [SerializeField] private float lifeTime;
    [SerializeField] private float beforeAnimTime;
    [SerializeField] private LayerMask damagableLayers;
    private GameObject _collider2D;
    private GameObject _area;
    [SerializeField] private Animator _animator;
    private Collider2D[] hitObjects;
    
    private void Awake()
    {
        _collider2D = gameObject.transform.GetChild(1).gameObject;
        _area = gameObject.transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(beforeAnimTime);
        _animator.Play("Explosion");
        yield return new WaitForSeconds(lifeTime - beforeAnimTime);
        _collider2D.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        Explode();
    }

    private void Explode()
    {
        hitObjects = Physics2D.OverlapCircleAll(_area.transform.position, _area.transform.localScale.y, damagableLayers);

        foreach (Collider2D hitObject in hitObjects)
        {
            hitObject.transform.GetComponentInParent<IDamagable>().GetDamage(this, gameObject);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_area.transform.position, _area.transform.localScale.y);
    }
}
