using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class BombLogic : WeaponLogic
{
    public const string bombName = "Bomb Fruit";
    private Bomb bomb;

    private void Awake()
    {
    }

    private void Start()
    {
        bomb = projectileSample.GetComponent<Bomb>();
        coolDown = 0f;
    }

    private void Update()
    {
        coolDown += Time.deltaTime;
    }


    private void ThrowBomb()
    {
        Instantiate(bomb, transform.position, quaternion.identity);
        coolDown = 0f;
    }
    
    public int GetAmountBombs()
    {
        return InventoryManager.instance.bombsAmount;
    }
    public void SetAmountBombs(int a)
    {
        InventoryManager.instance.bombsAmount = a;
    }

    override public void UseWeapon() { ThrowBomb(); }
    override public void StopWeapon() {  }
}
