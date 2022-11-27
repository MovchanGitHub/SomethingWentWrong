using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerBombSpawnerScript : MonoBehaviour
{
    public const string bombName = "Bomb Fruit";

    public GameObject bombPrefab;
    [SerializeField] GameObject InventoryCanvas;
    public float coolDown;
    private int amountBombs;
    private float timeAfterLastUse;

    private InventoryManager inventory;
    private IsometricPlayerMovementController player;

    private void Awake()
    {
        player = GetComponentInParent<IsometricPlayerMovementController>();
        inventory = InventoryCanvas.GetComponent<InventoryManager>();
    }

    private void Start()
    {
        timeAfterLastUse = 0f;
    }

    private void Update()
    {
        timeAfterLastUse += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.B) && amountBombs > 0 && timeAfterLastUse >= coolDown)
        {
            Instantiate(bombPrefab, transform.position, quaternion.identity);
            timeAfterLastUse = 0f;
            inventory.UseOneTimeWeapon(bombName);
            amountBombs--;
        }
        else
            if (Input.GetKeyDown(KeyCode.B))
                Debug.Log(amountBombs);
    }

    public int GetAmountBombs()
    {
        return amountBombs;
    }
    public void SetAmountBombs(int a)
    {
        amountBombs = a;
    }
}
