using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamagable : DamagableCharacter
{
    public Slider slider;
    [SerializeField] public int MaxHp;
    
    public int HP
    {
        get { return hp; }
        set { 
            // здесь добавить обновление полоски хп
            slider.value = value;
            if (value > 0 && value + hp > MaxHp)
                hp = value;
            else
                Die();
        }
    }

    protected override void Die()
    {
        SpawnSystemScript.instance.GameOver("Вы умерли");
    }
}
