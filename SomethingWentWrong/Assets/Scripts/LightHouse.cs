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


    private void Die()
    {
        healthBar.value = 0;
        GameManager.GM.GameOver("Вы проиграли (ракета уничтожена)");

        Destroy(gameObject);
    }
}
