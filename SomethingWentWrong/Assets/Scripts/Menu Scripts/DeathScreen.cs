using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject screen;
    [SerializeField] private bool isOpened;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponentInParent<DeathScreen>().ShowHideDeathScreen();
        }
    }
    
    public void ShowHideDeathScreen()
    {
        isOpened = !isOpened;
        screen.GameObject().SetActive(isOpened);
    }
}
