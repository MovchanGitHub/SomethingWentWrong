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
            {
                hp = 0;
                Die();
            }
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
        if (GM.IsTutorial) GM.Tutorial.Counter.SetActive(false);

        if (GM.UI.Encyclopedia.EnemiesTab.transform.GetChild(0).childCount == 3)
            GM.UI.Encyclopedia.EncyclopediaScript.OpenPlayerInEncyclopedia();
        
        if (GM.IsTutorial && lastWeapon.Type == WeaponType.Bomb && GM.Tutorial.endOfTutorial)
            GM.GameOver("Вы прошли обучение!\nно какой ценой...");
        else
        {
            switch (lastWeapon.Type)
            {
                case WeaponType.Gringe:
                    GM.GameOver("Умер от гринжа");
                    break;
                case WeaponType.Eye:
                    GM.GameOver("Решил потрогать лазер");
                    break;
                case WeaponType.Dino:
                    GM.GameOver("Исцарапан досмерти");
                    break;
                case WeaponType.Bomb:
                    GM.GameOver("Смерть это не выход");
                    break;
                default:
                    switch (UnityEngine.Random.Range(1, 3))
                    {
                        case 1:
                            GM.GameOver("УМЕР");
                            break;
                        case 2:
                            GM.GameOver("ПОТРАЧЕНО");
                            break;
                        default:
                            GM.GameOver("ПОМЕР");
                            break;
                    }
                    break;
            }
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
