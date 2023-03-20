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

    public virtual int MaxHP
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
            
        }
    }



    // DamagableCharacter unique methods
    public SpriteRenderer sprite;
    public float redTime;
    public CreaturesBase creature;

    protected virtual void Die() { }
    
    public virtual void GetDamage(IWeaponable weapon, GameObject sender = null)
    {
        lastWeapon = weapon;
        HP -= weapon.Damage;
    }
}
