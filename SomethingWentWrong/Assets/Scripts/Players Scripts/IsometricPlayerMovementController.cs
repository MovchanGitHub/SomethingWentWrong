using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{
    public static bool IsAbleToMove = true;

    [SerializeField]
    private float movementSpeedMin = 1f;
    [SerializeField]
    private float movementSpeedMax = 2f;
    [SerializeField]
    private float walkingAnimationSpeed = 1f;
    [SerializeField]
    private float runningAnimationSpeed = 1.65f;

    private bool isRunning;
    
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
        startPosition = attackPoint.transform.localPosition;
    }

    private void Start()
    {
        movementSpeed = movementSpeedMin;
        if (GameManagerScript.instance != null)
        {
            GameManagerScript.instance.player = gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && SurvivalManager.Instance.canRun())
            MaximizeSpeed();
        if (Input.GetKeyUp(KeyCode.LeftShift) || !SurvivalManager.Instance.canRun())
            MinimizeSpeed();
    }

    private void FixedUpdate()
    {
        if (IsAbleToMove)
        {
            float verticalInput; 
            float horizontalInput;
            
            Vector2 currentPos = rbody.position;
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            
            /*if (normalMovement || Math.Abs(horizontalInput) < float.Epsilon)
            {
                verticalInput = Input.GetAxisRaw("Vertical");
            }
            else
            {
                verticalInput = 0;
            }*/
            
            
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

    private void MaximizeSpeed()
    {
        movementSpeed = movementSpeedMax;
        isoRenderer.SetAnimationsSpeed(runningAnimationSpeed);
        isRunning = true;
    }
    
    private void MinimizeSpeed()
    {
        movementSpeed = movementSpeedMin;        
        isoRenderer.SetAnimationsSpeed(walkingAnimationSpeed);
        isRunning = false;
    }

    public bool IsRunning
    {
        get { return isRunning; }
    }
}
