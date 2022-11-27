using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance { get; private set; }

    public GameObject player;

    public GameObject lightHouse;

    //private bool isLightHouseActive;

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
}
