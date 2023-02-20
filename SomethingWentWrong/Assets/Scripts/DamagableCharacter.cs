using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DamagableCharacter : MonoBehaviour, IDamagable
{
    // IDamagable's implementation
    [SerializeField] private int hp;
    public Slider slider;
    private IWeaponable lastWeapon;
    
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
    
    public void GetDamage(IWeaponable weapon)
    {
        lastWeapon = weapon;
        HP -= weapon.Damage;
        StartCoroutine(BecomeRed());
    }
    
    
    // DamagableCharacter unique methods
    public SpriteRenderer sprite;
    public float redTime;
    private int damageNumber = 0;
    public CreaturesBase creature;

    private EnemyShaderLogic esl;


    private void Start()
    {
        esl = transform.parent.GetComponentInChildren<EnemyShaderLogic>();
    }

    private IEnumerator BecomeRed()
    {
        //sprite.color = Color.red;
        damageNumber++;
        yield return new WaitForSeconds(redTime);
        damageNumber--;
        if (damageNumber == 0)
            sprite.color = Color.white;
    }

    private void Die()
    {
        if (transform.tag == "Player")
        {
            GameManagerScript.instance.GameOver("Вы умерли");
        }
        
        else if (!creature.isOpenedInEcnyclopedia)
        {
            EncyclopediaManager.Instance.OpenNewCreature(creature);
        }
        
        else
        {
            StartCoroutine(EnemyDie());
        }
    }

    private IEnumerator EnemyDie()
    {
        if (lastWeapon.Type == WeaponType.Laser)
        {
            Laser laser = lastWeapon as Laser;
            esl.ChangeDissolvingColor(laser.LaserColor);
            esl.EnemyLaserDieShader();
            yield return new WaitForSeconds(1f);
        }
        
        Destroy(gameObject.transform.parent.gameObject);
    }
}
