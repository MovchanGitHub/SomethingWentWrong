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

    public bool isRunning;

    public bool isShooting;

    static private IsometricPlayerMovementController instance;

    static public IsometricPlayerMovementController Instance
    {
        get { return instance;  }
    }
    
    private float movementSpeed;
    public IsometricCharacterRenderer isoRenderer;

    private Rigidbody2D rbody;
    
    private Vector2 inputVector;

    public int a11 = 1, a12 = 0;
    public int a21 = 0, a22 = 1;

    public bool normalMovement = true; 

    [SerializeField] private GameObject attackPoint;
    private Vector3 startPosition;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        startPosition = attackPoint.transform.localPosition;

        instance = this;
    }

    private void Start()
    {
        movementSpeed = movementSpeedMin;
        if (GameManagerScript.instance != null)
        {
            GameManagerScript.instance.player = gameObject;
        }

        if (SurvivalManager.Instance != null)
        {
            SurvivalManager.Instance.player = gameObject;
            SurvivalManager.Instance.playerController = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && SurvivalManager.Instance.canRun() && !isShooting)
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

            if (horizontalInput != 0 || verticalInput != 0)
            {
                Vector3 newAttackPointPosition = new Vector3(startPosition.x + 1 * horizontalInput, startPosition.y + 1 * verticalInput, startPosition.z);
                newAttackPointPosition = Vector3.ClampMagnitude(newAttackPointPosition, 1);
                attackPoint.transform.localPosition = newAttackPointPosition;
            }
        }
    }

    private void MaximizeSpeed()
    {
        movementSpeed = movementSpeedMax;
        isoRenderer.SetAnimationsSpeed(runningAnimationSpeed);
        isRunning = true;
    }
    
    public void MinimizeSpeed()
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

