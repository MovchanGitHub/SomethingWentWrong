using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsometricCharacterRenderer : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetDirection(float x, float y)
    {
        _animator.SetFloat("HorizontalMovement", x);
        _animator.SetFloat("VerticalMovement", y);

        if (x != 0 || y != 0)
        {
            _animator.SetFloat("LastHorizontal", x);
            _animator.SetFloat("LastVertical", y);
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    public static int DirectionToIndex(Vector2 dir, int sliceCount)
    {
        Vector2 normDir = dir.normalized;
        float step = 360f / sliceCount;
        float halfstep = step / 2;
        float angle = Vector2.SignedAngle(Vector2.up, normDir);
        angle += halfstep;
        if (angle < 0) angle += 360;
        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }

    public void ChangeSpriteOrder(int new_order)
    {
        transform.GetComponent<SpriteRenderer>().sortingOrder = new_order;
    }
}
