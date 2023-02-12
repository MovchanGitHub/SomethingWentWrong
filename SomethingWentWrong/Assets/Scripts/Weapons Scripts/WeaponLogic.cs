using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] protected GameObject projectileSample;

    virtual public void UseWeapon() {}
    virtual public void StopWeapon() {}
}
