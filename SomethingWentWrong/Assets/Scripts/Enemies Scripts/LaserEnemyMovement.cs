using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyMovement : MonoBehaviour
{
    public GameObject actualTarget;
    private GameObject playerTarget;
    private GameObject rocketTarget;

    public float speed = 5f;
    private float distance;
    private float triggerDistance = 5f;
    [HideInInspector] public bool canMove = true;
    private Rigidbody2D rigidBody2D;
    private LaserEnemyAttack attackLogic;

    [SerializeField] private float strength = 15;
    [SerializeField] private float delay = 0.3f;

    [HideInInspector] public LaserEnemyScript es;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        attackLogic = GetComponentInChildren<LaserEnemyAttack>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameManager.GM.PlayerMovement.gameObject;
        rocketTarget = GameManager.GM.Rocket.gameObject;
        actualTarget = rocketTarget;
    }

    // Update is called once per frame
    void Update()
    {
        es.Animator.WalkAnim();
        distance = Vector2.Distance(transform.position, playerTarget.transform.position);
        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, actualTarget.transform.position, speed * Time.deltaTime);
        }

        if (distance < triggerDistance)
        {
            actualTarget = playerTarget;
            es.Animator.ChangeXY(playerTarget.transform.position - transform.position);
        }
        else
        {
            actualTarget = rocketTarget;
            es.Animator.ChangeXY(playerTarget.transform.position - transform.position);
        }
    }

    public void playFeedback(GameObject sender)
    {
        StopAllCoroutines();
        attackLogic.stopAttack();
        canMove = false;
        Vector2 direction = ((transform.position - sender.transform.position).normalized);
        rigidBody2D.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());

    }

    private IEnumerator Reset()
    {
        es.Animator.IdleAnim();
        yield return new WaitForSeconds(delay);
        rigidBody2D.velocity = Vector3.zero;
        canMove = true;
    }
}
