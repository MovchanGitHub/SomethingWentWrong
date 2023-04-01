using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static GameManager;

public class SlopeMovementTriggerScript : MonoBehaviour
{
    private IsometricPlayerMovementController controller;
    
    private void Start()
    {
        controller = GM.PlayerMovement;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Length < 11 || col.tag.Substring(0, 11) != "incline-min") return;

        controller.NormalMovement = false;
        
        switch (col.tag)
        {
            case "incline-min-W"://
                controller.a11 =  3; //controller.a12 =  0;
                controller.a21 =  1; //controller.a22 =  1;
                break;//
            //
            case "incline-min-E"://
                controller.a11 =  3; //controller.a12 =  0;
                controller.a21 = -1; //controller.a22 =  1;
                break;//
            //
            case "incline-min-SW"://
                controller.a11 =  5; //controller.a12 = -2;
                controller.a21 =  4; //controller.a22 =  1;
                break;//
            //
            case "incline-min-SE"://
                controller.a11 =  5; //controller.a12 =  2;
                controller.a21 = -4; //controller.a22 =  1;
                break;//
            //
            case "incline-min-NE"://
                controller.a11 = 4;  //controller.a12 =  2;
                controller.a21 = 1;  //controller.a22 =  1;
                break;//
            //
            case "incline-min-NW"://
                controller.a11 = 4;  //controller.a12 =  2;
                controller.a21 = -1; //controller.a22 =  1;
                break;//
        }//
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag.Length >= 11 && col.tag.Substring(0, 11) == "incline-min")
            controller.NormalMovement = true;
    }
}
