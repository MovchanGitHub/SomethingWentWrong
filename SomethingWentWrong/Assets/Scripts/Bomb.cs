using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float lifeTime;
    public float damageAmount;
    private GameObject _collider2D;

    private void Awake()
    {
        _collider2D = this.gameObject.transform.GetChild(1).gameObject;
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
        Destroy(gameObject);
    }
}
