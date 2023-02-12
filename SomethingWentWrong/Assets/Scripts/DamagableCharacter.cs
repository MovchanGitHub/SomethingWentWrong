using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;

public class DamagableCharacter : MonoBehaviour, IDamagable
{
    // IDamagable's implementation
    [SerializeField] private int hp;
    
    public int HP
    {
        get { return hp; }
        set { 
            // здесь добавить обновление полоски хп
            if (value > 0) 
                hp = value;
            else
                Die();
        }
    }
    
    public void GetDamage(IWeaponable weapon)
    {
        HP -= weapon.Damage;
        StartCoroutine(BecomeRed());
    }
    
    
    // DamagableCharacter unique methods
    public SpriteRenderer sprite;
    public float redTime;
    private int damageNumber = 0;
    public CreaturesBase creature;


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
        if (!creature.isOpenedInEcnyclopedia)
        {
            EncyclopediaManager.Instance.OpenNewCreature(creature);
        }
        
        if (transform.tag == "Player")
        {
            GameManagerScript.instance.GameOver();
        }
        else
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
