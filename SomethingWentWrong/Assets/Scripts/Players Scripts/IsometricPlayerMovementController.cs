using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeedInit = 1f;
    private float movementSpeed;
    private IsometricCharacterRenderer isoRenderer;

    private Rigidbody2D rbody;
    
    private Vector2 inputVector;
    private float verticalInput;
    private float horizontalInput;
    
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
            MaximizeSpeed();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            MinimizeSpeed();
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        
        //Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        CalculateIsometricMovement();
        
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        isoRenderer.SetDirection(movement);
        rbody.MovePosition(newPos);
    }

    private void CalculateIsometricMovement()
    {
        //Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = new Vector2(
            horizontalInput + verticalInput, 
            horizontalInput * -0.5f + 0.5f * verticalInput);
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
