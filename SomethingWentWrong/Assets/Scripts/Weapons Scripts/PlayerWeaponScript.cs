using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{
    [SerializeField] private List<WeaponLogic> weaponLogics;

    private int currWeapon = 0;


    private int CurrWeapon
    {
        get { return currWeapon;  }
        
        set
        {
            weaponLogics[currWeapon].StopWeapon();
            if (value >= weaponLogics.Count)
                currWeapon = 0;
            else if (value < 0)
                currWeapon = weaponLogics.Count - 1;
            else
                currWeapon = value;
        }
    }

    public void ChangeWeapon (UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.control.name == "q")
            CurrWeapon--;
        else
            CurrWeapon++;
    }

    public void Attack (UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (weaponLogics[CurrWeapon].ReadyToFire && GameManager.GM.InventoryManager.standartItemGrid.checkAmmo(weaponLogics[currWeapon].AmmoType))
            weaponLogics[currWeapon].UseWeapon();
    }

    public void StopAttack (UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        weaponLogics[currWeapon].StopWeapon();
    }
}
