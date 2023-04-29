using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;
using static DayNightSwitching;

public class EnemyMovement : MonoBehaviour
{
    private bool canMove = true;

    public bool CanMove
    {
        get => canMove;
        set
        {
            canMove = value;
            if (canMove) es.Animator.WalkAnim();
            else es.Animator.IdleAnim();
        }
    }

    public GameObject actualTarget;
    private GameObject playerTarget;
    private GameObject rocketTarget;

    public float speed = 5f;
    private float distance;
    [SerializeField] private float triggerDistance = 5f;
    private Rigidbody2D rigidBody2D;
    public bool isEnemyNight = false;

    [SerializeField] private float strength = 15;
    [SerializeField] private float delay = 0.3f;

    public GameObject attackPoint;
    public Vector2 direction;

    [HideInInspector] public EnemyScript es;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        playerTarget = GM.PlayerMovement.gameObject;
        rocketTarget = GM.Rocket.gameObject;
        actualTarget = rocketTarget;
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, playerTarget.transform.position);
        var heading = transform.position - actualTarget.transform.position;
        var d = heading.magnitude;
        direction = heading / d;

        if (CanMove)
        {
            attackPoint.transform.localPosition = new Vector3(0 - 0.8f * Math.Sign(direction.x), 1 - 0.8f * Math.Sign(direction.y), attackPoint.transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, actualTarget.transform.position, speed * Time.deltaTime);
        }

        if (distance < triggerDistance)
        {
            actualTarget = playerTarget;
            if (canMove)
            {
                rotateSprite();
            }
        }
        else
        {
            actualTarget = rocketTarget;
            if (canMove)
            {
                rotateSprite();
            }
        }
    }

    public void rotateSprite()
    {
        es.Animator.ChangeXY(actualTarget.transform.position - transform.position);
    }

    public void PlayFeedback(GameObject sender)
    {
        if (strength == 0) return;
        StopAllCoroutines();
        es.Attack.stopAttack();
        CanMove = false;
        Vector2 direction = ((transform.position - sender.transform.position).normalized);
        rigidBody2D.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());

    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rigidBody2D.velocity = Vector3.zero;
        CanMove = true;
    }

    private void OnDestroy()
    {
        if (isEnemyNight)
        {
            GM.Spawner.Enemies.ExistingEnemies--;
        }
    }


    /*
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

    private void OnDestroy()
    {
        if (isEnemyNight)
        {
            GM.Spawner.Enemies.ExistingEnemies--;
        }
    }
    */
}
