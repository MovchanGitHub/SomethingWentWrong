using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStats : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    private WeaponType type = WeaponType.Bullet;
    
    public WeaponType Type { get { return type; } }

    private int damage = 3;

    public int Damage { get { return damage; } }
    
    
    // BulletStats's unique methods
    public float speed;
    public float lifeTime;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<IDamagable>() != null && col.tag != "Player") 
        {
            col.GetComponent<IDamagable>().GetDamage(this);
            Destroy(gameObject);
        }
    }
    
    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    
    private void Start()
    {
        StartCoroutine(LifeTime());
    }
    
    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
