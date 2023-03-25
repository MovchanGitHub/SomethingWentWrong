using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    [SerializeField] private WeaponType type;
    
    public WeaponType Type { get { return type; } }

    [SerializeField] int damage;

    public int Damage { get { return Random.Range(damage - damageDispersion, damage + damageDispersion + 1); } }
    
    public Color LaserColor { get; set; }
    
    [SerializeField] private int damageDispersion = 1;

}
