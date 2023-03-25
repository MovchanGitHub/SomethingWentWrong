using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public Transform[] patrolPoints;
    public float waitTime;
    private int currentPointIndex;
    public Transform target;
    public float minDistance;
    public bool isEnemyNight = false;
    public bool moveToLightHouse = false;

    public bool isPatrolling;
    private bool isWaiting;

    private bool canMove;

    public bool CanMove
    {
        get => canMove;
        set
        {
            canMove = value;
            if (es && es.Animator)
            {
                if (canMove)
                    es.Animator.IdleAnim();
                else 
                    es.Animator.WalkAnim();
            }
        }
    }

    private EnemyNoticeTarget lookAt;

    [HideInInspector] public EnemyScript es;
    
    private void Awake()
    {
        lookAt = GetComponentInChildren<EnemyNoticeTarget>();
    }

    private void Start()
    {
        if (es && es.Animator)
            es.Animator.ChangeXY(patrolPoints[currentPointIndex].position - transform.position);
    }

    private void Update()
    {
        if (!canMove) return;

        if (isPatrolling)
        {
            Patrol();
        }
        else 
        {
            GoToTarget();
        }
    }

    public void GoToTarget()
    {
        if (Vector2.Distance(transform.position, target.position) > minDistance && !moveToLightHouse)
        {
            //es.Animator.ChangeXY(target.position - transform.position);
            transform.position = Vector2.MoveTowards(transform.position,
                target.position, speed * Time.deltaTime);
        }
        if (moveToLightHouse)
        {
            if (GM.Rocket)
            {
                //es.Animator.ChangeXY(GM.Rocket.gameObject.transform.position - transform.position);
                transform.position = Vector2.MoveTowards(transform.position, GM.Rocket.gameObject.transform.position, speed * Time.deltaTime);
            }
        }
    }
    
    private void Patrol()
    {
        if (patrolPoints == null) return;
        if (Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position) > 0.1)
        {
            //es.Animator.ChangeXY(patrolPoints[currentPointIndex].position - transform.position);
            transform.position = Vector2.MoveTowards(transform.position,
                patrolPoints[currentPointIndex].position, speed * Time.deltaTime);
        }
        else
        {
            if (!isWaiting)
            {
                StartCoroutine(Wait());
            }
        }
    }
    
    private IEnumerator Wait()
    {
        if (es &&es.Animator)
            es.Animator.IdleAnim();

        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        int newPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        lookAt.ChangeLookAtPoint(patrolPoints[currentPointIndex].position, patrolPoints[newPointIndex].position);
        currentPointIndex = newPointIndex;
        if (es &&es.Animator)
        {
            es.Animator.WalkAnim();
            es.Animator.ChangeXY(patrolPoints[currentPointIndex].position - transform.position);

        }
        isWaiting = false;
    }
}
