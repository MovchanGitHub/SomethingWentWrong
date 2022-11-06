using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIsometricMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1f;
    
    private Rigidbody2D rbody;

    private Vector2 inputVector;
    private float verticalInput;
    private float horizontalInput;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        rbody.MovePosition(newPos);
    }
}
