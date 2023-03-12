using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyMovement : MonoBehaviour
{
    private GameObject actualTarget;
    private GameObject playerTarget;
    private GameObject rocketTarget;

    public float speed = 5f;
    private float distance;
    private float triggerDistance = 5f;
    [HideInInspector] public bool canMove = true;

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
        distance = Vector2.Distance(transform.position, playerTarget.transform.position);
        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, actualTarget.transform.position, speed * Time.deltaTime);
        }

        if (distance < triggerDistance)
        {
            actualTarget = playerTarget;
        }
        else
        {
            actualTarget = rocketTarget;
        }
    }
}
