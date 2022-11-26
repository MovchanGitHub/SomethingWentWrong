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

    public int a11 = 1, a12 = 0;
    public int a21 = 0, a22 = 1;

    public bool ignoreVerticalInput;

    [SerializeField] private GameObject attackPoint;
    private Vector3 startPosition;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        startPosition = attackPoint.transform.localPosition;
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
            Vector2 currentPos = rbody.position;
            horizontalInput = Input.GetAxisRaw("Horizontal");
            if (!ignoreVerticalInput || Math.Abs(horizontalInput) < float.Epsilon)
                verticalInput = Input.GetAxisRaw("Vertical");
            isoRenderer.SetDirection(horizontalInput, verticalInput);

            if (horizontalInput != 0 || verticalInput != 0)
            {
                Vector3 newAttackPointPosition = new Vector3(startPosition.x + 1 * horizontalInput, startPosition.y + 1 * verticalInput, startPosition.z);
                newAttackPointPosition = Vector3.ClampMagnitude(newAttackPointPosition, 1);
                attackPoint.transform.localPosition = newAttackPointPosition;
            }

            inputVector = new Vector2(
                a11 * horizontalInput + a12 * verticalInput, 
                a21 * horizontalInput + a22 * verticalInput);
            
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
