using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : MonoBehaviour
{
    [SerializeField] private EnemiesSpawnSystem enemiesSpawnSystems;
    [SerializeField] private EnvironmentSpawnScript environmentSpawnScript;

    public EnemiesSpawnSystem Enemies { get { return enemiesSpawnSystems; } }
    public EnvironmentSpawnScript Resources { get { return environmentSpawnScript; } }
}
