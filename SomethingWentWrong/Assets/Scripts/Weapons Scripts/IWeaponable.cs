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
    Golem,
    Eye,
    Dino
}

public interface IWeaponable
{
    public WeaponType Type { get; }
    public int Damage { get; }
}


