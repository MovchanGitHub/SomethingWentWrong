using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStats : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<IDamagable>() != null && col.gameObject.transform.parent.tag != "Player") 
        {
            col.GetComponent<IDamagable>().GetDamage(damageAmount);
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
