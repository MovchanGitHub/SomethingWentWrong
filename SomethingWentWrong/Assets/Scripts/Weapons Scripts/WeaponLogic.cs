using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] protected GameObject projectileSample;
    [SerializeField] protected float coolDown;
    [SerializeField] protected ItemsBase ammoType;
    protected bool readyToFire = true;

    public bool ReadyToFire
    {
        get { return readyToFire;  }
    }

    protected IEnumerator GoCoolDown()
    {
        readyToFire = false;
        yield return new WaitForSeconds(coolDown);
        readyToFire = true;
    }

    public ItemsBase AmmoType
    {
        get { return ammoType; }
    }

    public virtual bool UseWeapon() { return true;}

    public virtual void StopWeapon() {}

    public virtual void CanNotUseWeapon()
    {
        // информирование игрока о том что оружие не доступно к использованию
        // Debug.Log("нельзя использовать это оружие сейчас");
    }
}
