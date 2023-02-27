using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamagable : DamagableCharacter
{
    public Slider slider;
    [SerializeField] public int MaxHp;
    
    public int Hp
    {
        get { return hp; }
        set { 
            // Обновление индикатора здоровья
            if (value > 0) {
                if (value <= MaxHp) {
                    slider.value = value;
                    hp = value;
                }
            }
            else
                Die();
        }
    }

    protected override void Die()
    {
        SpawnSystemScript.instance.GameOver("Вы умерли");
    }
}
