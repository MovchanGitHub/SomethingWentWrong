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
    //[SerializeField] GameObject InventoryCanvas;
    public float coolDown;
    //private int amountBombs;
    private float timeAfterLastUse;

    private InventoryManager inventory;
    private IsometricPlayerMovementController player;

    private void Awake()
    {
        player = GetComponentInParent<IsometricPlayerMovementController>();
        inventory = InventoryManager.instance;
    }

    private void Start()
    {
        bomb = projectileSample.GetComponent<Bomb>();
        timeAfterLastUse = 0f;
        if (InventoryManager.instance != null)
        {
            InventoryManager.instance.BombSpawner = gameObject;
        }
    }

    private void Update()
    {
        timeAfterLastUse += Time.deltaTime;

        /*if (Input.GetKeyDown(KeyCode.B) && InventoryManager.instance.bombsAmount > 0 && timeAfterLastUse >= coolDown)
        {
            //ThrowBomb();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            //Debug.Log(amountBombs);
        }*/
    }


    private void ThrowBomb()
    {
        Instantiate(bomb, transform.position, quaternion.identity);
        timeAfterLastUse = 0f;
        InventoryManager.instance.UseOneTimeWeapon(bombName);
        InventoryManager.instance.bombsAmount--;
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
