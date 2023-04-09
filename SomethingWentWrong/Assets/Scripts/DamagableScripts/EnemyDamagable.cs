using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class EnemyDamagable : DamagableCharacter
{
    private EnemyShaderLogic esl;
    [SerializeField] private Slider slider;
    [SerializeField] private DamagePopup damagePopupPrefab;
    private EnemyMovement enemyLogic; //new

    public override int HP
    {
        get { return hp; }
        set
        {
            spawnDamagePopup(transform.position, hp - value);
            if (value > 0)
                hp = value;
            else
                Die();
            slider.value = hp;
        }
    }

    private void Start()
    {
        enemyLogic = GetComponentInParent<EnemyMovement>(); //new
        esl = transform.parent.GetComponentInChildren<EnemyShaderLogic>();
    }

    //new
    public override void GetDamage(IWeaponable weapon, GameObject sender = null)
    {
        if (weapon.Type == WeaponType.Enemy) return;
        base.GetDamage(weapon, null);
        if (sender != null)
        {
            enemyLogic.PlayFeedback(sender);
        }
    }

    protected override void Die()
    {
        // if (!creature.isOpenedInEcnyclopedia)
        // {
        //     GameManager.GM.UI.Encyclopedia.OpenNewCreature(creature);
        // }
        StartCoroutine(EnemyDie());
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
        else if (lastWeapon.Type == WeaponType.Bomb)
        {
            esl.EnemyBombDieShader();
            yield return new WaitForSeconds(0.15f);
        }
        
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void spawnDamagePopup(Vector3 position, int damageAmount)
    {
        DamagePopup damagePopup= Instantiate(damagePopupPrefab, position, Quaternion.identity);
        damagePopup.setup(damageAmount);
    }
}
