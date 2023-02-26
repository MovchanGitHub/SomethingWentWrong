using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystemScript : MonoBehaviour
{
    public static SpawnSystemScript instance { get; private set; }

    public GameObject player;

    public GameObject lightHouse;

    public GameObject UI;

    public bool isUIOpened;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }

        Destroy(this.gameObject);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ActivateLightHouse();
        }        
    }

    public void ActivateLightHouse()
    {
        //isLightHouseActive = true;
        lightHouse.GetComponent<LightHouse>().active = true;
    }

    public void DeactivateLightHouse()
    {
        //isLightHouseActive = false;
        lightHouse.GetComponent<LightHouse>().active = false;
    }

    public void GameOver(string message)
    {
        UI.GetComponent<DeathScreen>().ShowDeathScreen(message);

        IsometricPlayerMovementController.Instance.IsAbleToMove = false;

        InventoryController.instance.canBeOpened = false;

        SurvivalManager.Instance.transform.gameObject.SetActive(false);

        player.gameObject.SetActive(false);

        instance.isUIOpened = true;
    }
}
