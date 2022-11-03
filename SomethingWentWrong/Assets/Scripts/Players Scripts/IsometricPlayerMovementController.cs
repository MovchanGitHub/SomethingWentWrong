using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{
    public static bool IsAbleToMove = true;

    [SerializeField]
    private float movementSpeedInit = 1f;
    private float movementSpeed;
    private IsometricCharacterRenderer isoRenderer;

    private Rigidbody2D rbody;
    
    private Vector2 inputVector;
    private float verticalInput;
    private float horizontalInput;

    private float a11 = 1f, a12 = 0f;
    private float a21 = 0f, a22 = 1f;
    
    
    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }

    private void Start()
    {
        movementSpeed = movementSpeedInit;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.LeftShift))
            MaximizeSpeed();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            MinimizeSpeed();*/
    }

    private void FixedUpdate()
    {
        if (IsAbleToMove)
        {
            Vector2 currentPos = rbody.position;
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");

            inputVector = new Vector2(
                a11 * horizontalInput + a12 * verticalInput, 
                a21 * horizontalInput + a22 * verticalInput);
            
            inputVector = Vector2.ClampMagnitude(inputVector, 1);
            Vector2 movement = inputVector * movementSpeed;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
            isoRenderer.SetDirection(movement);
            rbody.MovePosition(newPos);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "incline-max-W":
                a11 = 4f; a12 = 0f;
                a21 = 3f; a22 = 1f;
                break;
            case "incline-max-E":
                a11 = 4f;  a12 = 0f;
                a21 = -3f; a22 = 1f;
                break;
            case "incline-max-SW":
                a11 = 2f; a12 = -2f;
                a21 = 3f; a22 = 1f;
                break;
            case "incline-max-SE":
                a11 = 2f;  a12 = 2f;
                a21 = -3f; a22 = 1f;
                break;
            case "incline-mid-W":
                a11 = 2f; a12 = 0f;
                a21 = 1f; a22 = 1f;
                break;
            case "incline-mid-E":
                a11 = 2f; a12 = 0f;
                a21 = -1f; a22 = 1f;
                break;
            case "incline-mid-SW":
                a11 = 1f; a12 = -2f;
                a21 = 1f; a22 = 1f;
                break;
            case "incline-mid-SE":
                a11 = 1f; a12 = 2f;
                a21 = -1f; a22 = 1f;
                break;
        }
    }
    
    
    private void OnTriggerExit2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "incline-max-W":
            case "incline-max-E":
            case "incline-max-SW":
            case "incline-max-SE":            
            case "incline-mid-W":            
            case "incline-mid-E":
            case "incline-mid-SW":            
            case "incline-mid-SE":
                a11 = 1f; a12 = 0f;
                a21 = 0f; a22 = 1f;
                break;
        }
    }

    private void MaximizeSpeed()
    {
        movementSpeed = 2 * movementSpeedInit;
    }
    
    private void MinimizeSpeed()
    {
        movementSpeed = movementSpeedInit;
    }
}
