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

    [SerializeField] private int minDistanceToPlayerX = 13;
    [SerializeField] private int minDistanceToPlayerY = 8;

    private int probabilitesSum = 0;

    private double windowLength;

    private void Awake()
    {
        windowLength =
            Math.Sqrt(minDistanceToPlayerX * minDistanceToPlayerX / 4 + minDistanceToPlayerY * minDistanceToPlayerY / 4);
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
            positionIndex = UnityEngine.Random.Range(0, spawnPointsAmount);
            int startIndex = positionIndex;
            Vector2 cameraPoints = GM.Camera.WorldToViewportPoint(spawnPoints[positionIndex].position);
            while (isBusy[positionIndex] || (cameraPoints.x > 0f && cameraPoints.y < 0f && cameraPoints.x > 1f && cameraPoints.y > 1f))
            {
                positionIndex++;
                if (positionIndex == spawnPointsAmount)
                    positionIndex = 0;

                if (positionIndex == startIndex)
                {
                    Debug.LogError("it is impossible to spawn a resource");
                    return;
                }
            }
            
            //Debug.Log("spawned resource at position " + positionIndex);

            var newResource = Instantiate(
                objectSamplesWithProbabilities[UnityEngine.Random.Range(0, probabilitesSum)],
                spawnPoints[positionIndex].position,
                Quaternion.identity).transform.parent = transform;
            
            isBusy[positionIndex] = true;

            spawnedResources++;
        }
    }
}
