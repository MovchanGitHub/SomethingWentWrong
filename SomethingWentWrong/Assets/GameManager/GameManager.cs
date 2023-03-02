using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LightHouse rocket;
    [SerializeField] private SurvivalManager survivalManager;
    [SerializeField] private IsometricPlayerMovementController playerMovement;
    [SerializeField] private InventoryController inventoryManager;
    [SerializeField] private MiniGMUI ui;



    private void Awake()
    {
        GM = this;
        
    }

    public static GameManager GM { get; private set; }


    public LightHouse Rocket { get { return rocket; } }
    public SurvivalManager SurvivalManager { get { return survivalManager; } }
    public IsometricPlayerMovementController PlayerMovement { get { return playerMovement; } }
    public InventoryController InventoryManager { get { return inventoryManager; } }
    public MiniGMUI UI { get { return ui; } }


    public void GameOve()
    {
        //GameOver.Invoke();
    }
    public void GameOver(string message)
    {
        //UI.GetComponent<DeathScreen>().ShowDeathScreen(message);

        //IsometricPlayerMovementController.Instance.IsAbleToMove = false;

        //InventoryController.instance.canBeOpened = false;

        //GameManager.GM.SurvivalManager.gameObject.SetActive(false);

        //player.gameObject.SetActive(false);

        //instance.isUIOpened = true;
    }
}
