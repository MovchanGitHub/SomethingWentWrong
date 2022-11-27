using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public GameObject screen;
    [SerializeField] private bool isOpened;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GetComponentInParent<WinScreen>().ShowHideWinScreen();
        }
    }
    
    public void ShowHideWinScreen()
    {
        isOpened = !isOpened;
        screen.GameObject().SetActive(isOpened);
    }
}
