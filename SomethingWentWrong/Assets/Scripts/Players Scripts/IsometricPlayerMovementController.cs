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

    public int a11 = 1, a12 = 0;
    public int a21 = 0, a22 = 1;

    public bool normalMovement = true;
    
    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }

    private void Start()
    {
        movementSpeed = movementSpeedInit;
        if (GameManagerScript.instance != null)
        {
            GameManagerScript.instance.player = gameObject;
        }
    }

    /*private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.LeftShift))
            MaximizeSpeed();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            MinimizeSpeed();#1#
    }*/

    private void FixedUpdate()
    {
        if (IsAbleToMove)
        {
            float verticalInput; 
            float horizontalInput;
            
            Vector2 currentPos = rbody.position;
            horizontalInput = Input.GetAxisRaw("Horizontal");
            if (normalMovement || Math.Abs(horizontalInput) < float.Epsilon)
            {
                verticalInput = Input.GetAxisRaw("Vertical");
            }
            else
            {
                verticalInput = 0;
            }
            verticalInput = Input.GetAxisRaw("Vertical");
            
            if (Math.Abs(horizontalInput) > 0f && Math.Abs(verticalInput) > 0f)
            {
                horizontalInput *= 2;
            }
            else
            {
                float tempInput = horizontalInput;
                horizontalInput = a11 * horizontalInput + a12 * verticalInput;
                verticalInput = a21 * tempInput + a22 * verticalInput;
            }
            
            isoRenderer.SetDirection(horizontalInput, verticalInput);

            inputVector = new Vector2(horizontalInput, verticalInput);
            
            inputVector = Vector2.ClampMagnitude(inputVector, 1);
            Vector2 movement = inputVector * movementSpeed;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
            rbody.MovePosition(newPos);
        }
    }

    /*
    private void MaximizeSpeed()
    {
        movementSpeed = 2 * movementSpeedInit;
    }
    
    private void MinimizeSpeed()
    {
        movementSpeed = movementSpeedInit;
    }*/
}
