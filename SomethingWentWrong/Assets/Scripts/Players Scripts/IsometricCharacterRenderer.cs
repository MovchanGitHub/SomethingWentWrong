using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class IsometricCharacterRenderer : MonoBehaviour
{
    private Animator _animator;

    private float screenCenterX;
    private float screenCenterY;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        screenCenterX = Screen.width * 0.5f;
        screenCenterY = Screen.height * 0.5f;
    }

    public void SetDirection(float x, float y)
    {
        _animator.SetBool("IsMoving", x != 0 || y != 0);
        
        if (GameManager.GM.PlayerMovement.usingWeapon 
            && !GameManager.GM.PlayerMovement.hand_to_hand)
        {
            SetDirectionToMouse();
        }
        else if (x != 0 || y != 0)
        {
            GameManager.GM.PlayerMovement.lastHorizontalInput = x;
            GameManager.GM.PlayerMovement.lastVerticalInput = y;
            _animator.SetFloat("MouseX", x);
            _animator.SetFloat("MouseY", y);
        }
    }

    public void PlayShoot()
    {
        GM.PlayerMovement.usingWeapon = true;        
        _animator.SetTrigger("Shoot");
        _animator.ResetTrigger("StopShooting");
    }
    
    public void PlayStopShooting()
    {
        GM.PlayerMovement.usingWeapon = false;
        _animator.ResetTrigger("Shoot");
        _animator.SetTrigger("StopShooting");
    }
    
    private void SetDirectionToMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        _animator.SetFloat("MouseX", mousePos.x - Screen.width * 0.5f);
        _animator.SetFloat("MouseY", mousePos.y - Screen.height * 0.5f);
    }

    public void ChangeSpriteOrder(int new_order)
    {
        transform.GetComponent<SpriteRenderer>().sortingOrder = new_order;
    }

    public void SetAnimationsSpeed(float newSpeed)
    {
        _animator.speed = newSpeed;
    }
}
