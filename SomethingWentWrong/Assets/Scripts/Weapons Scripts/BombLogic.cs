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
    }

    

    private void ThrowBomb()
    {
        Instantiate(bomb, transform.position, quaternion.identity);
        StartCoroutine(GoCoolDown());
    }
    

    override public void UseWeapon() { ThrowBomb(); }
    override public void StopWeapon() {  }
}
