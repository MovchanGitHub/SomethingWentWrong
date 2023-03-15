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

    public bool canMove;

    private EnemyNoticeTarget lookAt;

    [HideInInspector] public EnemyScript es;
    
    private void Awake()
    {
        lookAt = GetComponentInChildren<EnemyNoticeTarget>();
    }

    private void Start()
    {
        es.Animator.ChangeXY(patrolPoints[currentPointIndex].position - transform.position);
    }

    private void Update()
    {
        if (isPatrolling)
        {
            Patrol();
        }
        else if (canMove)
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
        es.Animator.IdleAnim();

        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        int newPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        lookAt.ChangeLookAtPoint(patrolPoints[currentPointIndex].position, patrolPoints[newPointIndex].position);
        currentPointIndex = newPointIndex;
        es.Animator.ChangeXY(patrolPoints[currentPointIndex].position - transform.position);
        isWaiting = false;
        
        es.Animator.WalkAnim();
    }

    private void OnDestroy()
    {
        if (isEnemyNight)
        {
            GM.Spawner.Enemies.ExistingEnemies--;
        }
    }
}
