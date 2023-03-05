using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
public class PlayerWeaponScript : MonoBehaviour
{
    [SerializeField] private List<WeaponLogic> weaponLogics;
    private int currWeapon = 0;

    private float timeAfterUse;

    private void Start()
    {
        timeAfterUse = 0f;
    }

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
    
    private void Update()
    {
        timeAfterUse += Time.deltaTime;
        
        // Поменять текущее оружие
        if (Input.GetKeyDown(KeyCode.E))
        {
            GM.UI.WeaponsBarScript.RightRotateWeapons();
            CurrWeapon++;
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GM.UI.WeaponsBarScript.LeftRotateWeapons();
            CurrWeapon--;
        }
        
        
        // Использовать текущее оружие
        if (Input.GetButtonDown("Fire1") 
            && (timeAfterUse > weaponLogics[currWeapon].CoolDown)
            && !GameManager.GM.PlayerMovement.usingWeapon
            && GameManager.GM.InventoryManager.standartItemGrid.checkAmmo(weaponLogics[currWeapon].AmmoType)
            )
        {
            timeAfterUse = 0;
            weaponLogics[currWeapon].UseWeapon();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            weaponLogics[currWeapon].StopWeapon();
        }
    }
}
