using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsometricCharacterRenderer : MonoBehaviour
{
    public static readonly string[] idleDirections = { "idle N", "idle W", "idle S", "idle E" };
    public static readonly string[] runDirections = { "run N", "run W", "run S", "run E" };

    private Animator _animator;
    private int lastDirection;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction)
    {
        string[] directionArray = null;

        if (direction.magnitude < .01f)
        {
            directionArray = idleDirections;
        }
        else
        {
            directionArray = runDirections;
            lastDirection = DirectionToIndex(direction, 4);
        }
        
        //_animator.Play(directionArray[lastDirection]);
        if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
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
