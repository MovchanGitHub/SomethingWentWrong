using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.TestTools;
using Vector3 = UnityEngine.Vector3;

public class EnemyNoticeTarget : MonoBehaviour
{
    public string targetTag;
    private EnemyMovement enemyLogic;

    private void Awake()
    {
        enemyLogic = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == targetTag)
        {
            enemyLogic.isPatrolling = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == targetTag)
        {
            enemyLogic.isPatrolling = true;
        }
    }

    public void ChangeLookAtPoint(Vector3 from, Vector3 to)
    {
        
    }
}
