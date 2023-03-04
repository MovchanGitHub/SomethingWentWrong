using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DamagableCharacter : MonoBehaviour, IDamagable
{
    // IDamagable's implementation
    [SerializeField] protected int hp;
    [SerializeField] protected int maxHp;
    protected IWeaponable lastWeapon;
    
    public virtual int HP
    {
        get { return hp; }
        set { 
            if (value > 0) 
                hp = value;
            else
                Die();
        }
    }
    
    public int MaxHP { get { return maxHp; } set { maxHp = value; } }

    public virtual void GetDamage(IWeaponable weapon)
    {
        lastWeapon = weapon;
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
        //sprite.color = Color.red;
        damageNumber++;
        yield return new WaitForSeconds(redTime);
        damageNumber--;
        if (damageNumber == 0)
            sprite.color = Color.white;
    }

    protected virtual void Die() { }

}
