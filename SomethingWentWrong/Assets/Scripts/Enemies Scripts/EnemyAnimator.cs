using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator[] anims;
    private int xId;
    private int yId;
    private int isMovingId;
    private int attackId;
    private int stopAttackId;
    public float attackAnimationDuration;

    private void Awake()
    {
        xId = Animator.StringToHash("X");
        yId = Animator.StringToHash("Y");
        isMovingId = Animator.StringToHash("isMoving");
        attackId = Animator.StringToHash("Attack");
        stopAttackId = Animator.StringToHash("StopAttack");
    }

    public void ChangeXY(Vector3 newXY)
    {
        foreach (var anim in anims)
        {
            anim.SetFloat(xId, newXY.x);
            anim.SetFloat(yId, newXY.y);
        }
    }

    public void IdleAnim()
    {
        foreach (var anim in anims)
            anim.SetBool(isMovingId, false);
    }
    
    public void WalkAnim()
    {
        foreach (var anim in anims)
            anim.SetBool(isMovingId, true);
    }

    public void AttackTrigger()
    {
        foreach (var anim in anims)
            anim.SetTrigger(attackId);
        //anim.ResetTrigger(stopAttackId);
    }

    public void StopAttackTrigger()
    {
        foreach (var anim in anims)
            anim.SetTrigger(stopAttackId);
        //anim.ResetTrigger(attackId);
    }
}
