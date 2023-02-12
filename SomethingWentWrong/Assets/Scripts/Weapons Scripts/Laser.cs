using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    [SerializeField] private WeaponType type;
    
    public WeaponType Type { get { return type; } }

    [SerializeField] int damage;

    public int Damage { get { return damage; } }
}
