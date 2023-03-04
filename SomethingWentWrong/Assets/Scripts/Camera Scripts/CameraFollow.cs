using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;
    private GameObject сameraHolder;

    private void Awake()
    {
        сameraHolder = transform.parent.gameObject;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(сameraHolder.transform.position, desiredPosition, smoothSpeed);
        сameraHolder.transform.position = smoothedPosition;
    }
}
