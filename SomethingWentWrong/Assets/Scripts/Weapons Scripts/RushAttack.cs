using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAttack : MonoBehaviour, IWeaponable
{
    [SerializeField] private WeaponType type;
    [SerializeField] private int damage;
    public WeaponType Type { get { return type; } }
    public int Damage { get { return damage; } }

    private static IWeaponable instance;

    public static IWeaponable __RushAttack { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }
}
