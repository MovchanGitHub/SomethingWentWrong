using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    private void Awake()
    {
        instance = this;
    }
    public GameObject player;
}
