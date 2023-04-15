using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalPlayerDamage : MonoBehaviour, IWeaponable
{
    public WeaponType weaponType = WeaponType.SurvivalParams;
    public WeaponType Type { get => weaponType; }
    public int Damage { get => GameManager.GM.SurvivalManager.LoseHpEffect; }
}
