using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static GameManager;

public class EnemiesSpawnSystem : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    private int spawnPointsAmount;

    [SerializeField] private List<GameObject> enemiesSamples;
    [SerializeField] private List<int> enemiesProbabilities;
    
    private GameObject[] enemiesSamplesWithProbabilities;

    // кол-во врагов которое должно заспавниться
    [SerializeField] private int enemiesToSpawn;
    private int spawnedEnemies = 0;

    private int existingEnemies = 0;

    public int ExistingEnemies
    {
        get
        {
            return existingEnemies;
        }
        
        set
        {
            existingEnemies = value;
            if (existingEnemies == 0)
            {
                Debug.Log("Волна противников подавлена");
                // событие происходящее при убийстве всех заспавненных врагов (e.g смена музыки)
                // вызов окна скилов
                GM.UI.SkillsMenu.GetComponentInParent<SkillsScript>().InitSkills();
                if (!GM.UI.EndScreen.GetComponentInParent<EndScreen>().isOpened)
                    GM.UI.SkillsMenu.SetActive(true);
            }
        }
    }
    
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private float deltaTimeBetweenSpawn;
    [SerializeField] private float minTimeBetweenSpawn;
    //private float timePassed = 0f;

    [SerializeField] private float minDistanceToPlayer;
    
    private int probabilitesSum = 0;

    private void Awake()
    {
        spawnPointsAmount = spawnPoints.Count;
        
        for (int i = 0; i < enemiesProbabilities.Count; ++i)
        {
            probabilitesSum += enemiesProbabilities[i];
        }
        
        enemiesSamplesWithProbabilities = new GameObject[probabilitesSum];

        int ind = 0;
        
        for (int i = 0; i < enemiesProbabilities.Count; ++i)
        {
            for (int j = 0; j < enemiesProbabilities[i]; ++j)
            {
                enemiesSamplesWithProbabilities[ind] = enemiesSamples[i];
                ind++;
            }
        }
    }

    public IEnumerator SpawnEnemies()
    {
        spawnedEnemies = 0;
        
        while (spawnedEnemies != enemiesToSpawn)
        {
            int positionIndex = FindPointFarFromPlayer();
            
            if (positionIndex == -1)
                yield break;
            
            EnemyMovement newEnemyMovement = Instantiate(
                enemiesSamplesWithProbabilities[UnityEngine.Random.Range(0, probabilitesSum)],
                spawnPoints[positionIndex].position,
                Quaternion.identity).GetComponent<EnemyMovement>();
            
            //newEnemyMovement.isPatrolling = false;
            //newEnemyMovement.target = GM.PlayerMovement.transform;
            newEnemyMovement.isEnemyNight = true;
            //newEnemyMovement.moveToLightHouse = true;
            //newEnemyMovement.GoToTarget();

            //Debug.Log("spawned enemy at position " + positionIndex);
            
            spawnedEnemies++;
            existingEnemies++;
            yield return new WaitForSeconds(timeBetweenSpawn);
        }

        enemiesToSpawn++;
        timeBetweenSpawn -= deltaTimeBetweenSpawn;
        if (timeBetweenSpawn < minTimeBetweenSpawn)
            timeBetweenSpawn = minTimeBetweenSpawn;
    }

    private int FindPointFarFromPlayer()
    {
        int positionIndex = UnityEngine.Random.Range(0, spawnPointsAmount);

        if (Vector2.Distance(
                GM.PlayerMovement.transform.position,
                spawnPoints[positionIndex].position)
            < minDistanceToPlayer)
        {
            int startIndex = positionIndex;

            while (Vector2.Distance(
                       GM.PlayerMovement.transform.position,
                       spawnPoints[positionIndex].position)
                   < minDistanceToPlayer)
            {
                positionIndex++;
                if (positionIndex == spawnPointsAmount)
                    positionIndex = 0;

                if (positionIndex == startIndex)
                {
                    Debug.LogError("it is impossible to spawn an enemy");
                    return -1;
                }
            }
        }

        return positionIndex;
    }
}
