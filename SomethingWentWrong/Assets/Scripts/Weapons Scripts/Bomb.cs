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
    public float lifeTime;
    public LayerMask damagableLayers;
    private GameObject _collider2D;
    private GameObject _area;

    private void Awake()
    {
        _collider2D = this.gameObject.transform.GetChild(1).gameObject;
        _area = this.gameObject.transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        _collider2D.SetActive(true);        
        yield return new WaitForSeconds(0.05f);
        Explode();
    }

    private void Explode()
    {

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_area.transform.position, _area.transform.localScale.y, damagableLayers);

        foreach (Collider2D hitObject in hitObjects)
        {
            hitObject.transform.GetComponentInParent<IDamagable>().GetDamage(this);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_area.transform.position, _area.transform.localScale.y);
    }
}
