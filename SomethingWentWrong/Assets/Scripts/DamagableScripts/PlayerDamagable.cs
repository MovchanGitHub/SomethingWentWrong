using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

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
                Destroy(gameObject);
            slider.value = hp;
        }
    }
    
    public override void GetDamage(IWeaponable weapon)
    {
        base.GetDamage(weapon);
        if (weapon.Type == WeaponType.Bomb)
            StartCoroutine(GameManager.GM.Camera.GetComponent<CameraShake>().Shake(.15f, .3f));
    }

    private void OnDestroy()
    {
        slider.value = 0;
        GM.GameOver("Вы умерли");
    }
}
