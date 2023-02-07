using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;

public class DamagableCharacter : MonoBehaviour, IDamagable
{
    public SpriteRenderer sprite;
    public float redTime;
    private int damageNumber = 0;
    [SerializeField] private float hp;
    public CreaturesBase creature;


    public float HP()
    {
        return hp;
    }

    public void GetDamage(float damage)
    {
        hp -= damage;
        StartCoroutine(BecomeRed());
        if (hp <= 0)
        {
            if (!creature.isOpenedInEcnyclopedia)
            {
                EncyclopediaManager.Instance.OpenNewCreature(creature);
            }
            Die();
        }
    }

    private IEnumerator BecomeRed()
    {
        sprite.color = Color.red;
        damageNumber++;
        yield return new WaitForSeconds(redTime);
        damageNumber--;
        if (damageNumber == 0)
            sprite.color = Color.white;
    }

    /*
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bomb")
        {
            Bomb bomb = col.GetComponentInParent<Bomb>();
            CircleCollider2D cc2d = bomb.GetComponentInChildren<CircleCollider2D>();
            Vector2 distToBomb = Vector2.ClampMagnitude(bomb.transform.position - transform.position, 0.9f);
            float magnitude = distToBomb.SqrMagnitude();
            GetDamage((1f - magnitude * magnitude) * bomb.damageAmount);
        }
    }
    */

    private void Die()
    {
        if (transform.tag == "Player")
        {
            GameManagerScript.instance.GameOver();
        }
        else
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
