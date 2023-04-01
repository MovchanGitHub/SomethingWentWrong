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

    private int _inclineId;
    private int _mouseXId;
    private int _mouseYId;
    private int _isMovingId;
    private int _shootId;
    private int _stopShootingId;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _inclineId = Animator.StringToHash("Incline");
        _mouseXId = Animator.StringToHash("MouseX");
        _mouseYId = Animator.StringToHash("MouseY");
        _isMovingId = Animator.StringToHash("IsMoving");
        _shootId = Animator.StringToHash("Shoot");
        _stopShootingId = Animator.StringToHash("StopShooting");
    }

    public void SetDirection(float x, float y)
    {
        _animator.SetBool(_isMovingId, x != 0 || y != 0);
        
        if (GM.PlayerMovement.usingWeapon 
            && !GM.PlayerMovement.hand_to_hand)
        {
            SetDirectionToMouse();
        }
        else if (x != 0 || y != 0)
        {
            GM.PlayerMovement.lastHorizontalInput = x;
            GM.PlayerMovement.lastVerticalInput = y;
            _animator.SetFloat(_mouseXId, x);
            _animator.SetFloat(_mouseYId, y);
        }
    }

    public void PlayShoot()
    {
        GM.PlayerMovement.usingWeapon = true;        
        _animator.SetTrigger(_shootId);
        _animator.ResetTrigger(_stopShootingId);
    }
    
    public void PlayStopShooting()
    {
        GM.PlayerMovement.usingWeapon = false;
        _animator.ResetTrigger(_shootId);
        _animator.SetTrigger(_stopShootingId);
    }
    
    private void SetDirectionToMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        _animator.SetFloat(_mouseXId, mousePos.x - Screen.width * 0.5f);
        _animator.SetFloat(_mouseYId, mousePos.y - Screen.height * 0.5f);
    }

    public void ChangeSpriteOrder(int newOrder)
    {
        transform.GetComponent<SpriteRenderer>().sortingOrder = newOrder;
    }

    public void SetAnimationsSpeed(float newSpeed)
    {
        _animator.speed = newSpeed;
    }

    public void InclineMovement(bool inclineMovement)
    {
        _animator.SetBool(_inclineId, inclineMovement);
    }
}
