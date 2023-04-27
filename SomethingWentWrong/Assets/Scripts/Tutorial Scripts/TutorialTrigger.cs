using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private int popupIndex;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Tutorial: " + gameObject.name);
        GameManager.GM.Tutorial.PopupSystem.PopupTrigger(popupIndex);
        Destroy(gameObject);
    }
}
