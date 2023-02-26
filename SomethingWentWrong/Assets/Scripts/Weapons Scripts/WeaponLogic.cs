using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] protected GameObject projectileSample;
    [SerializeField] protected float coolDown;
    [SerializeField] protected ItemsBase ammoType;

    public float CoolDown
    {
        get { return coolDown;  }
    }

    public ItemsBase AmmoType
    {
        get { return ammoType; }
    }

    virtual public void UseWeapon() {}

    virtual public void StopWeapon() {}
}
