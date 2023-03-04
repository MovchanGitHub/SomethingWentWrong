using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamagable : DamagableCharacter
{
    private EnemyShaderLogic esl;
    [SerializeField] private Slider slider;

    public override int HP
    {
        get { return hp; }
        set
        {
            if (value > 0)
                hp = value;
            else
                Die();
            slider.value = hp;
        }
    }

    private void Start()
    {
        esl = transform.parent.GetComponentInChildren<EnemyShaderLogic>();
    }
    
    protected override void Die()
    {
        if (!creature.isOpenedInEcnyclopedia)
        {
            GameManager.GM.UI.Encyclopedia.OpenNewCreature(creature);
        }
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
        
        Destroy(gameObject.transform.parent.gameObject);
    }
}
