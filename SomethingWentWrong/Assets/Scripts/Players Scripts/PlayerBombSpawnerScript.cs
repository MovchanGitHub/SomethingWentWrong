using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBombSpawnerScript : MonoBehaviour
{
    public GameObject bombPrefab;
    public float coolDown;
    private float timeAfterLastUse;

    private IsometricPlayerMovementController player;

    private void Awake()
    {
        player = GetComponentInParent<IsometricPlayerMovementController>();
    }

    private void Start()
    {
        timeAfterLastUse = 0f;
    }

    private void Update()
    {
        timeAfterLastUse += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.B) && timeAfterLastUse >= coolDown)
        {
            Instantiate(bombPrefab, transform.position, quaternion.identity);
            timeAfterLastUse = 0f;
        }
    }
}
