using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        
        if (IsometricPlayerMovementController.Instance.isShooting)
        {
            SetDirectionToMouse();
        }
        else if (x != 0 || y != 0)
        {
            _animator.SetFloat("MouseX", x);
            _animator.SetFloat("MouseY", y);
        }
    }

    public void PlayUseLaserAnim()
    {
        _animator.SetTrigger("UseLaser");
    }
    
    public void PlayStopLaserAnim()
    {
        _animator.SetTrigger("StopLaser");
    }
    
    private void SetDirectionToMouse()
    {
        _animator.SetFloat("MouseX", Input.mousePosition.x - Screen.width * 0.5f);
        _animator.SetFloat("MouseY", Input.mousePosition.y - Screen.height * 0.5f);
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
