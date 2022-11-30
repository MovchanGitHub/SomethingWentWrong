using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameManagerFix : MonoBehaviour
{
    private void Start()
    {
        if (GameManagerScript.instance != null)
        {
            GameManagerScript.instance.UI = gameObject;
        }
    }
}
