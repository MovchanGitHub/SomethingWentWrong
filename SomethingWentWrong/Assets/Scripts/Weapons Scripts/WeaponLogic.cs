using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] protected GameObject projectileSample;
    [SerializeField] protected float coolDown;

    public float CoolDown
    {
        get { return coolDown;  }
    }

    virtual public void UseWeapon() {}

    virtual public void StopWeapon() {}
}
