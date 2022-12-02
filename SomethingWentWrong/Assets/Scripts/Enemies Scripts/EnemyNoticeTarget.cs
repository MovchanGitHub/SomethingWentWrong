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
        if (col.tag == "Player")
        {
            DayNightSwitching.Instance.EnemiesInCombat += 1;

            enemyLogic.isPatrolling = false;
            if (enemyLogic.isEnemyNight)
            {
                enemyLogic.moveToLightHouse = false;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            DayNightSwitching.Instance.EnemiesInCombat -= 1;
        }

        if (!enemyLogic.isEnemyNight)
        {
            enemyLogic.isPatrolling = true;
        }
        if (enemyLogic.isEnemyNight)
        {
            enemyLogic.moveToLightHouse = true;
        }
    }

    public void ChangeLookAtPoint(Vector3 from, Vector3 to)
    {
        
    }
}
