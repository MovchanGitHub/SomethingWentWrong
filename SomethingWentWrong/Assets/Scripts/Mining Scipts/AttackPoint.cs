using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    private Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.localPosition = new Vector3(0.25f, startPosition.y + 1 * Mathf.Sign(Input.GetAxis("Vertical")), startPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(0.25f + startPosition.x * Mathf.Sign(Input.GetAxis("Horizontal")), startPosition.y, startPosition.z);
        }

    }
}
