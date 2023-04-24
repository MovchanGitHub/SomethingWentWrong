using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class LightHouse : MonoBehaviour, IDamagable
{
    // IDamagable's implementation
    [SerializeField] private int hp;
    [SerializeField] private int maxHp;
    [SerializeField] private Slider healthBar;
    
    public List<AudioClip> _AudioClips;
    private AudioSource _hitSource;
    private int _hitInd;
    private RocketSkins _rocketSkins;
    
    public int laserRepairingEffect = 5;
    
    public int HP
    {
        get { return hp; }
        set
        {

            if (value > 0)
            {
                if (value > maxHp)
                    hp = maxHp;
                else
                    hp = value;
                healthBar.value = value;
                _rocketSkins.ChangeSpriteAccordingToHP(hp);
            }
            else
                Die();
        }
    }

    private void Awake()
    {
        _rocketSkins = GetComponentInChildren<RocketSkins>();
        maxHp = HP;
        _hitSource = GetComponent<AudioSource>();
    }

    public int MaxHP
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
            healthBar.maxValue = value;
        }
    }

    public void GetDamage(IWeaponable weapon, GameObject sender = null)
    {
        switch (weapon.Type)
        {
            case WeaponType.Fists: // игрок не ломает ракету кулаками
                break;
            case WeaponType.Laser: // ракета чинится от лазера (сварка)
                HP += laserRepairingEffect;
                break;
            default:
            {
                HP -= weapon.Damage;
                _hitSource.pitch = 1 + UnityEngine.Random.Range(-0.10f, 0.10f);
                _hitSource.PlayOneShot(_AudioClips[_hitInd]);
                _hitInd++;
                if (_hitInd == 3)
                    _hitInd = 0;
            }
                break;
        }
    }
    
    // LightHouse unique methods
    public bool active = false;

    private void Die()
    {
        healthBar.value = 0;
        GM.GameOver("Ракета уничтожена");
        GM.UnlinkRocket();
        Destroy(gameObject);
    }
}
