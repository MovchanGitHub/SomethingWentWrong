using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamagable : DamagableCharacter
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        maxHp = HP;
    }

    new public int HP
    {
        get { return hp; }
        set {
            if (value > 0)
            {
                if (value > maxHp)
                    hp = maxHp;
                else
                    hp = value;
            }
            else
                Die(); //В событии указать обнуление слайдера
            slider.value = hp;
        }
    }

    protected override void Die()
    {
        GameManager.GM.GameOver("Вы умерли");
    }
}
