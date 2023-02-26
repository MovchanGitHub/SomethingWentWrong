using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamagable : DamagableCharacter
{
    public Slider slider;
    
    public int HP
    {
        get { return hp; }
        set { 
            // здесь добавить обновление полоски хп
            slider.value = value;
            if (value > 0) 
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
