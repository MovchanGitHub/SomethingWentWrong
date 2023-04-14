using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;


public class BombLogic : WeaponLogic
{
    public const string bombName = "Bomb Fruit";
    private Bomb bomb;
    public List<AudioClip> _explosions;

    private void Awake()
    {
    }

    private void Start()
    {
        bomb = projectileSample.GetComponent<Bomb>();
    }

    

    private void ThrowBomb()
    {
        Instantiate(bomb, transform.position, quaternion.identity);
        StartCoroutine(GoCoolDown());
    }


    override public bool UseWeapon()
    {
        if (GM.InventoryManager.standartItemGrid.checkAmmo(AmmoType))
        {
            ThrowBomb();
            return true;
        }

        return false;
    }
    override public void StopWeapon() {  }
    
    public override void CanNotUseWeapon()
    {
        Debug.Log("нельзя стрелять: нет бомбы");
    }
}
