using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;
    private int xId;
    private int yId;
    private int isMovingId;
    private int attackId;
    private int stopAttackId;
    public float attackAnimationDuration;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        xId = Animator.StringToHash("X");
        yId = Animator.StringToHash("Y");
        isMovingId = Animator.StringToHash("isMoving");
        attackId = Animator.StringToHash("Attack");
        stopAttackId = Animator.StringToHash("StopAttack");
    }

    public void ChangeXY(Vector3 newXY)
    {
        anim.SetFloat(xId, newXY.x);
        anim.SetFloat(yId, newXY.y);
    }

    public void IdleAnim()
    {
        anim.SetBool(isMovingId, false);
    }
    
    public void WalkAnim()
    {
        anim.SetBool(isMovingId, true);
    }

    public void AttackTrigger()
    {
        anim.SetTrigger(attackId);
        //anim.ResetTrigger(stopAttackId);
    }

    public void StopAttackTrigger()
    {
        anim.SetTrigger(stopAttackId);
        //anim.ResetTrigger(attackId);
    }
}
