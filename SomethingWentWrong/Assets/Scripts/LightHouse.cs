using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightHouse : MonoBehaviour, IDamagable
{
    // IDamagable's implementation
    [SerializeField] private int hp;
    public Slider slider;
    
    public int HP
    {
        get { return hp; }
        set { 
            // здесь добавить обновление полоски хп
            slider.value = hp;
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
        if (GameManagerScript.instance != null)
        {
            GameManagerScript.instance.lightHouse = gameObject;
        }
    }


    private void Die()
    {
        GameManagerScript.instance.GameOver("Вы проиграли");

        Destroy(gameObject);
    }
}
