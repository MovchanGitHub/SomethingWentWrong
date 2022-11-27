using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeMovementTriggerScript : MonoBehaviour
{
    private IsometricPlayerMovementController controller;
    
    private void Awake()
    {
        controller = GetComponentInParent<IsometricPlayerMovementController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "incline-max-W":
                controller.a11 =  4; //controller.a12 =  0;
                controller.a21 =  3; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-max-E"://
                controller.a11 =  4; //controller.a12 =  0;
                controller.a21 = -3; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-max-SW"://
                controller.a11 =  2; //controller.a12 = -2;
                controller.a21 =  3; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-max-SE"://
                controller.a11 =  2; //controller.a12 =  2;
                controller.a21 = -3; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-mid-W"://
                controller.a11 =  2; //controller.a12 =  0;
                controller.a21 =  1; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-mid-E"://
                controller.a11 =  2; //controller.a12 =  0;
                controller.a21 = -1; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-mid-SW"://
                controller.a11 =  1; //controller.a12 = -2;
                controller.a21 =  1; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-mid-SE"://
                controller.a11 =  1; //controller.a12 =  2;
                controller.a21 = -1; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-min-W"://
                controller.a11 =  3; //controller.a12 =  0;
                controller.a21 =  1; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-min-E"://
                controller.a11 =  3; //controller.a12 =  0;
                controller.a21 = -1; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-min-SW"://
                controller.a11 =  5; //controller.a12 = -2;
                controller.a21 =  4; //controller.a22 =  1;
                controller.normalMovement = false;
                break;//
            
            case "incline-min-SE"://
                controller.a11 =  5; //controller.a12 =  2;
                controller.a21 = -4; //controller.a22 =  1;
                controller.normalMovement = false;
                break;
            
            case "incline-min-NE":
                controller.a11 = 4; //controller.a12 =  2;
                controller.a21 = 1; //controller.a22 =  1;
                controller.normalMovement = false;
                break;
            
            case "incline-min-NW":
                controller.a11 = 4; //controller.a12 =  2;
                controller.a21 = -1; //controller.a22 =  1;
                controller.normalMovement = false;
                break;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "incline-max-W":
            case "incline-max-E":
            case "incline-max-SW":
            case "incline-max-SE":            
            case "incline-mid-W":            
            case "incline-mid-E":
            case "incline-mid-SW":            
            case "incline-mid-SE":           
            case "incline-min-W":            
            case "incline-min-E":
            case "incline-min-SW":            
            case "incline-min-SE":            
            case "incline-min-NE":            
            case "incline-min-NW":
                controller.a11 = 1;
                controller.a21 = 0;
                controller.normalMovement = true;
                break;
        }
    }
}
