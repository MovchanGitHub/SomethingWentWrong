using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LightHouse rocket;
    [SerializeField] private SurvivalManager survivalManager;
    [SerializeField] private IsometricPlayerMovementController playerMovement;
    [SerializeField] private InventoryController inventoryManager;
    [SerializeField] private MiniGMUI ui;
    [SerializeField] private InputSystem inputSystem;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Spawners _spawners;

    private void Awake()
    {
        GM = this;
    }

    public static GameManager GM { get; private set; }

    public LightHouse Rocket { get { return rocket; } }

    public void UnlinkRocket() { rocket = null; }
    
    public SurvivalManager SurvivalManager { get { return survivalManager; } }
    public IsometricPlayerMovementController PlayerMovement { get { return playerMovement; } }
    public InventoryController InventoryManager { get { return inventoryManager; } }
    public MiniGMUI UI { get { return ui; } }
    public InputSystem InputSystem { get { return inputSystem; } }
    public Camera Camera { get { return mainCamera; } }
    public Spawners Spawner { get { return _spawners; } }

    public void GameOver(string message)
    {
        UI.GetComponent<EndScreen>().ShowDeathScreen(message);

        playerMovement.IsAbleToMove = false;

        inventoryManager.canBeOpened = false;

        SurvivalManager.gameObject.SetActive(false);

        playerMovement.gameObject.SetActive(false);
    }
}
