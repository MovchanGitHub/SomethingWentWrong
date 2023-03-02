using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeVyzov : MonoBehaviour
{
    private Shake shake;

    private void Start()
    {
        shake = GetComponent<Shake>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            shake.CamShake();
        }
    }
}
