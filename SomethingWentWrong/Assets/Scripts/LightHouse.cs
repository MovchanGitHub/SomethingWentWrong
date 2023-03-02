using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            healthBar.value = value;
            if (value > 0)
                if (value > maxHp)
                    hp = maxHp;
                else
                    hp = value;
            else
                Die();
        }
    }

    public int MaxHP { get { return maxHp; } set { maxHp = value; } }
    
    public void GetDamage(IWeaponable weapon)
    {
        HP -= weapon.Damage;
    }
    
    // LightHouse unique methods
    public bool active = false;

    private void Start()
    {
        if (SpawnSystemScript.instance != null)
        {
            SpawnSystemScript.instance.lightHouse = gameObject;
        }
    }


    private void Die()
    {
        SpawnSystemScript.instance.GameOver("Вы проиграли");

        Destroy(gameObject);
    }
}
