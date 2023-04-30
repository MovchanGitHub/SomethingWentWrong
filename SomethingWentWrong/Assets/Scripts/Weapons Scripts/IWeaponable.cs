using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Bullet,
    Bomb,
    Laser,
    Enemy,
    Fists,
    RushAttack,
    Gringe,
    Eye,
    Dino,
    SurvivalParams
}

public interface IWeaponable
{
    public WeaponType Type { get; }
    public int Damage { get; }
}


