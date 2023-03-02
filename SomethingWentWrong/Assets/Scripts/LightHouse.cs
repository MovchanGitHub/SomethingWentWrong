using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightHouse : MonoBehaviour, IDamagable
{
    // IDamagable's implementation
    [SerializeField] private int hp;
    [SerializeField] private Slider healthBar;
    
    public int HP
    {
        get { return hp; }
        set {
            // здесь добавить обновление полоски хп
            healthBar.value = value;
            if (value > 0) 
                hp = value;
            else
                Die();
        }
    }
    
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
