using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class LightHouse : MonoBehaviour, IDamagable
{
    // IDamagable's implementation
    [SerializeField] private int hp;
    [SerializeField] private int maxHp;
    [SerializeField] private Slider healthBar;
    
    public int HP
    {
        get { return hp; }
        set {

            if (value > 0)
            {
                if (value > maxHp)
                    hp = maxHp;
                else
                    hp = value;
                healthBar.value = value;
            }
            else
                Destroy(gameObject);
        }
    }

    private void Awake()
    {
        maxHp = HP;
    }

    public int MaxHP
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
            healthBar.maxValue = value;
        }
    }

    public void GetDamage(IWeaponable weapon, GameObject sender = null)
    {
        HP -= weapon.Damage;
    }
    
    // LightHouse unique methods
    public bool active = false;

    private void OnDestroy()
    {
        healthBar.value = 0;
        GM.GameOver("Ракета уничтожена");
        GM.UnlinkRocket();
    }
}
