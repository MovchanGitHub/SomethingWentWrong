using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamagable : DamagableCharacter
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        maxHp = HP;
    }

    public override int HP
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
    
    public override void GetDamage(IWeaponable weapon)
    {
        base.GetDamage(weapon);
        if (weapon.Type == WeaponType.Bomb)
            StartCoroutine(GameManager.GM.Cam.GetComponent<CameraShake>().Shake(.15f, .3f));
    }

    protected override void Die()
    {
        GameManager.GM.GameOver("Вы умерли");
    }
}
