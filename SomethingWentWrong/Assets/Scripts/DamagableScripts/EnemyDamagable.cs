using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagable : DamagableCharacter
{
    private EnemyShaderLogic esl;
    
    private void Start()
    {
        esl = transform.parent.GetComponentInChildren<EnemyShaderLogic>();
    }
    
    protected override void Die()
    {
        if (!creature.isOpenedInEcnyclopedia)
        {
            EncyclopediaManager.Instance.OpenNewCreature(creature);
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
