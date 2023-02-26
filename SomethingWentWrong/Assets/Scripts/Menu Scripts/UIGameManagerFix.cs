using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameManagerFix : MonoBehaviour
{
    private void Start()
    {
        if (SpawnSystemScript.instance != null)
        {
            SpawnSystemScript.instance.UI = gameObject;
        }
    }
}
