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
            if (value >= weaponLogics.Count)
                currWeapon = 0;
            else if (value < 0)
                currWeapon = weaponLogics.Count - 1;
            else
                currWeapon = value;
        }
    }
    
    private void Update()
    {
        // Поменять текущее оружие
        if (Input.GetKeyDown(KeyCode.E))
            CurrWeapon++;
        if (Input.GetKeyDown(KeyCode.Q))
            CurrWeapon--;
        
        
        // Использовать текущее оружие
        if (Input.GetButtonDown("Fire1"))
        {
            weaponLogics[currWeapon].UseWeapon();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            weaponLogics[currWeapon].StopWeapon();
        }
    }
}
