using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;

    [SerializeField] private GameObject followingCamera;
    [SerializeField] private GameObject lighthouse;
    [SerializeField] private GameObject uI;
    //[SerializeField] private GameObject encyclopedia;
    [SerializeField] private GameObject spawnSystem;
    [SerializeField] private GameObject dayNightCycle;
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject survivalManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dayNightMusic;
    [SerializeField] private GameObject inventoryController;
    //[SerializeField] private GameObject newInventory;

    private IsometricPlayerMovementController playerMovement;

    private void Awake()
    {
        instance = this;

        playerMovement = player.GetComponent<IsometricPlayerMovementController>();
        
    }

    public GameManager __GameManager { get { return instance; } }

    public GameObject FollowingCamera { get { return followingCamera; } }
    public GameObject Lighthouse { get { return lighthouse; } }
    public GameObject UI { get { return uI; } }
    //public GameObject Encyclopedia { get { return encyclopedia; } }
    public GameObject SpawnSystem { get { return spawnSystem; } }
    public GameObject DayNightCycle { get { return dayNightCycle; } }
    public GameObject EventSystem { get { return eventSystem; } }
    public GameObject SurvivalManager { get { return survivalManager; } }
    public GameObject Player { get { return player; } }
    public GameObject DayNightMusic { get { return dayNightMusic; } }
    public GameObject InventoryController { get { return inventoryController; } }
    //public GameObject NewInventory { get { return newInventory; } }

    public IsometricPlayerMovementController PlayerMovement { get { return playerMovement; } }
}
