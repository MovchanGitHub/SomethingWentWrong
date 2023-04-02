using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    [SerializeField] private WeaponType type;
    
    public WeaponType Type { get { return type; } }

    [SerializeField] private int damage;

    public int Damage { get { return damage; } }
    
    
    // Bullet's unique methods
    private Rigidbody2D rb;
    public float speed;
    public float lifeTime;
    public Vector2 direction;
    private Vector2 newPos;

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.GetComponent<IDamagable>() != null && col.tag != "Player") 
        {
            col.GetComponent<IDamagable>().GetDamage(this, gameObject);
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
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(LifeTime());
        direction = Vector2.ClampMagnitude(direction, 1f);
        newPos = (Vector2)transform.position + direction * (speed * Time.deltaTime);
    }

    [SerializeField] private float rotateSpeed;
    [SerializeField] private Vector3 rotationDirection = new Vector3();
    
    private void Update()
    {
        transform.Rotate(rotationDirection  * (rotateSpeed * Time.deltaTime));
        newPos += direction * (speed * Time.deltaTime);
        rb.MovePosition(newPos);
    }
}
