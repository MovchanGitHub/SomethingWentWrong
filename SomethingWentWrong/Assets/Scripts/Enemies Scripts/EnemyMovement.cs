using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private void Awake()
    {
        lookAt = GetComponentInChildren<EnemyNoticeTarget>();
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
            transform.position = Vector2.MoveTowards(transform.position,
                target.position, speed * Time.deltaTime);
        }
        if (moveToLightHouse)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager.GM.Rocket.gameObject.transform.position, speed * Time.deltaTime);
        }
    }
    
    private void Patrol()
    {
        if (patrolPoints == null) return;
        if (Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position) > 0.1)
        {
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
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        int newPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        lookAt.ChangeLookAtPoint(patrolPoints[currentPointIndex].position, patrolPoints[newPointIndex].position);
        currentPointIndex = newPointIndex;
        isWaiting = false;
    }
}
