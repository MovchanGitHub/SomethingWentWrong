using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float lifeTime;
    public float damageAmount;
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
        Boom();
    }

    private void Boom()
    {

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_area.transform.position, _area.transform.localScale.y / 2, damagableLayers);

        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.GetComponent<IDamagable>() != null)
            {
                hitObject.GetComponent<IDamagable>().GetDamage(damageAmount);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_area.transform.position, _area.transform.localScale.y / 2);
    }
}
