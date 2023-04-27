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
    [SerializeField] private Slider sliderIncrease;
    [SerializeField] private int minimumHealth = 20;
    private int damageNumber = 0;
    private PlayerShaderLogic psl;

    private void Awake()
    {
        maxHp = HP;
        psl = transform.parent.GetComponentInChildren<PlayerShaderLogic>();
        StartCoroutine(IncreaseHP());
    }

    public override int HP
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
            }
            else
                Die();
            slider.value = hp;
        }
    }
    public override int MaxHP
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
            slider.maxValue = maxHp;
            sliderIncrease.maxValue = maxHp;
        }
    }
    
    public override void GetDamage(IWeaponable weapon, GameObject sender = null)
    {
        base.GetDamage(weapon);
        if (weapon.Type == WeaponType.Bomb)
            StartCoroutine(GameManager.GM.Camera.GetComponent<CameraShake>().Shake(.15f, .3f));
        StartCoroutine(psl.BecomeRed());
    }
    
    // private IEnumerator BecomeRed()
    // {
    //     sprite.color = Color.red;
    //     damageNumber++;
    //     yield return new WaitForSeconds(redTime);
    //     damageNumber--;
    //     if (damageNumber == 0)
    //         sprite.color = Color.white;
    // }
    
    protected override void Die()
    {
        slider.value = 0;
        if (GM.IsTutorial && lastWeapon.Type == WeaponType.Bomb && GM.Tutorial.endOfTutorial)
        {
            GM.GameOver("Вы прошли обучение!\nно какой ценой...");
        }
        else
        {
            GM.GameOver("ПОМЕР");
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (GM.InventoryManager != null)
        {
            GM.InventoryManager.isCanvasActive = false;
        }

        if (GM.InventoryManager.canvasTransform != null)
        {
            GM.InventoryManager.canvasTransform.gameObject.SetActive(false);
        }
    }

    private IEnumerator IncreaseHP()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            int loseHpEffect = GM.SurvivalManager.LoseHpEffect;
            if (loseHpEffect == 0 && HP < minimumHealth)
                HP++;
            else if (loseHpEffect != 0)
                GetDamage(GM.SurvivalManager.survivalPlayerDamage);
        }
    }
}
