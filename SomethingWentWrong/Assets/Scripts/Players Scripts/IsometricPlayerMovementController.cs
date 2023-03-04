using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using static GameManager;

public class IsometricPlayerMovementController : MonoBehaviour
{
    private PlayerInput playerInput;

    public bool IsAbleToMove = true;

    [SerializeField]
    private float movementSpeedMin = 1f;
    [SerializeField]
    private float movementSpeedMax = 2f;
    [SerializeField]
    private float walkingAnimationSpeed = 1f;
    [SerializeField]
    private float runningAnimationSpeed = 1.65f;    
    [SerializeField]
    private float rushingAnimationSpeed = 5f;

    public bool isRunning;

    public bool usingWeapon= false;
    public bool hand_to_hand;
    
    private float movementSpeed;
    [HideInInspector]public IsometricCharacterRenderer isoRenderer;

    private Rigidbody2D rbody;
    
    private Vector2 inputVector;

    public int a11 = 1, a12 = 0;
    public int a21 = 0, a22 = 1;

    public bool normalMovement = true; 

    private GameObject attackPoint;
    private Vector3 startPosition;

    [SerializeField] private float rushTime = 0.25f;

    private RushAttack rushAttack;
    float verticalInput; 
    float horizontalInput;
    public float lastVerticalInput;
    public float lastHorizontalInput;
    private Vector2 currentPos;
    private bool isRushing = false;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        attackPoint = GetComponentInChildren<AttackPoint>().gameObject;
        rushAttack = GetComponentInChildren<RushAttack>();
        startPosition = attackPoint.transform.localPosition;
    }

    private void Start()
    {
        playerInput = GM.InputSystem.GetComponent<PlayerInput>();

        lastHorizontalInput = 1;
        lastVerticalInput = -1;
        SetWalkingSpeed();
    }

    private IEnumerator StartRushing()
    {
        SetRushingSpeed();

        yield return new WaitForSeconds(rushTime);
        
        SetWalkingSpeed();
    }

    public void Rush(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!GM.SurvivalManager.CanRush())
            return;
        
        horizontalInput = lastHorizontalInput;
        verticalInput = lastVerticalInput;
        
        if (verticalInput == 0)
            transform.rotation = Quaternion.Euler(0, 0, -Math.Sign(horizontalInput) * 15);
        else
            transform.rotation = Quaternion.Euler(0, 0, -Math.Sign(horizontalInput) * 5);

        GM.SurvivalManager.ReplenishStamina(-GM.SurvivalManager.staminaToRush);
        StartCoroutine(StartRushing());
    }

    private IEnumerator Running()
    {
        SetRunningSpeed();
        while (GM.SurvivalManager.CanRun())
            yield return null;
        SetWalkingSpeed();
    }

    public void Run(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!GM.SurvivalManager.CanRun() || usingWeapon) return;
        StartCoroutine(Running());
    }

    public void Walk(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SetWalkingSpeed();
    }

    private void FixedUpdate()
    {
        if (IsAbleToMove)
        {
            currentPos = rbody.position;

            if (!isRushing)
            {
                verticalInput = 0; 
                horizontalInput = 0;
            
                if (!usingWeapon)
                {
                    horizontalInput = playerInput.actions["Move"].ReadValue<Vector2>().x;
                    verticalInput = playerInput.actions["Move"].ReadValue<Vector2>().y;
                }
            
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
            }
            
            Moving();

            if (horizontalInput != 0 || verticalInput != 0)
            {
                Vector3 newAttackPointPosition = new Vector3(startPosition.x + 1 * horizontalInput, startPosition.y + 1 * verticalInput, startPosition.z);
                newAttackPointPosition = Vector3.ClampMagnitude(newAttackPointPosition, 1);
                attackPoint.transform.localPosition = newAttackPointPosition;
            }
        }
    }

    private void Moving()
    {
        isoRenderer.SetDirection(horizontalInput, verticalInput);

        inputVector = new Vector2(horizontalInput, verticalInput);
            
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        rbody.MovePosition(newPos);
    }

    private void SetRunningSpeed()
    {
        movementSpeed = movementSpeedMax;
        isoRenderer.SetAnimationsSpeed(runningAnimationSpeed);
        isRunning = true;
    }
    
    private void SetRushingSpeed()
    {
        movementSpeed = 3 * movementSpeedMax;
        isoRenderer.SetAnimationsSpeed(rushingAnimationSpeed);
        isRushing = true;
        isRunning = true;
    }
    
    public void SetWalkingSpeed()
    {
        movementSpeed = movementSpeedMin;
        isoRenderer.SetAnimationsSpeed(walkingAnimationSpeed);
        isRushing = false;        
        isRunning = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public bool IsRunning
    {
        get { return isRunning; }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isRushing)
            return;
        
        IDamagable obj;
        obj = col.GetComponent<IDamagable>();
        if (obj != null)
        {
            StopCoroutine(StartRushing());
            SetWalkingSpeed();
            obj.GetDamage(rushAttack);
        }
    }
}

