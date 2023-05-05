using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserEnemyDamagable : EnemyDamagable
{
    public override void GetDamage(IWeaponable weapon, GameObject sender = null)
    {
        if (weapon.Type == WeaponType.Gringe || weapon.Type == WeaponType.Eye || weapon.Type == WeaponType.Dino) return;
        base.GetDamage(weapon, null);
        if (sender != null && weapon.Type != WeaponType.Fists)
        {
            enemyLogic.PlayFeedback(sender);
        }
    }
}
