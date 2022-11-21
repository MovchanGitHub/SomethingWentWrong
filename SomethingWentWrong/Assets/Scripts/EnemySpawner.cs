using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefab;

    void Start()
    {
        
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            int randEnemy = Random.Range(0, enemyPrefab.Length);
            int randSpawnPoint = Random.Range(0, spawnPoints.Length);

            Instantiate(enemyPrefab[0], spawnPoints[randSpawnPoint].position, transform.rotation);
        }
    }
}
