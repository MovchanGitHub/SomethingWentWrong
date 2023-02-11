using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    [SerializeField] private WeaponType type;
    
    public WeaponType Type { get { return type; } }

    [SerializeField] private int damage;

    public int Damage { get { return damage; } }
    
    
    // Bullet's unique methods
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
