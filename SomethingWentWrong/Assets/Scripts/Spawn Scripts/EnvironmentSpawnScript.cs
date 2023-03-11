using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static GameManager;

public class EnvironmentSpawnScript : MonoBehaviour
{
    [SerializeField] private int simultaneouslyAmount;
    
    [SerializeField] private List<Transform> spawnPoints;
    private int spawnPointsAmount;

    private bool[] isBusy;

    [SerializeField] private List<GameObject> objectSamples;
    [SerializeField] private List<int> resourcesProbabilities;
    
    private GameObject[] objectSamplesWithProbabilities;

    private int positionIndex;

    private int spawnedResources;

    [SerializeField] private int minDistanceToPlayer;

    private int probabilitesSum = 0;

    private void Awake()
    {
        for (int i = 0; i < resourcesProbabilities.Count; ++i)
        {
            probabilitesSum += resourcesProbabilities[i];
        }
        
        objectSamplesWithProbabilities = new GameObject[probabilitesSum];

        int ind = 0;
        
        for (int i = 0; i < resourcesProbabilities.Count; ++i)
        {
            for (int j = 0; j < resourcesProbabilities[i]; ++j)
            {
                objectSamplesWithProbabilities[ind] = objectSamples[i];
                ind++;
            }
        }
    }

    public int PositionIndex
    {
        get { return positionIndex; }
        private set { positionIndex = value; }
    }

    private void Start()
    {
        positionIndex = -1;
        spawnPointsAmount = spawnPoints.Count;
        
        isBusy = new bool[spawnPointsAmount];
        spawnedResources = 0;
        SpawnResources();
    }

    public void PurgePointWithIndex(int index)
    {
        if (index != -1) 
        {
            isBusy[index] = false;
            spawnedResources--;
            SpawnResources();
        }
        
    }

    public void SpawnResources()
    {
        while (spawnedResources != simultaneouslyAmount)
        {
            positionIndex = UnityEngine.Random.Range(0, spawnPointsAmount - 1);
            
            if (isBusy[positionIndex] || 
                Vector2.Distance(GM.PlayerMovement.transform.position, spawnPoints[positionIndex].position) < 
                minDistanceToPlayer)
            {
                int startIndex = positionIndex;
                while (isBusy[positionIndex] ||
                       Vector2.Distance(GM.PlayerMovement.transform.position, spawnPoints[positionIndex].position) <
                       minDistanceToPlayer)
                {
                    positionIndex++;
                    if (positionIndex == spawnPointsAmount)
                        positionIndex = 0;

                    if (positionIndex == startIndex)
                    {
                        Debug.Log("it is impossible to spawn a resource");
                        return;
                    }
                }
            }
            
            Debug.Log("spawned resource at position " + positionIndex);

            Instantiate(
                objectSamplesWithProbabilities[UnityEngine.Random.Range(0, probabilitesSum)],
                spawnPoints[positionIndex].position,
                Quaternion.identity);
            
            isBusy[positionIndex] = true;

            spawnedResources++;
        }
    }
}
