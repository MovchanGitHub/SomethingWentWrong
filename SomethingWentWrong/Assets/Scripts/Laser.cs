using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;

    private LayerMask mask;

    void Start()
    {
        DisableLaser();
        mask = LayerMask.GetMask("LaserBlocker");
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            EnableLaser();
        }
        else if (Input.GetButton("Fire1"))
        {
            UpdateLaser();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            DisableLaser();
        }
    }
    
    void EnableLaser()
    {
        lineRenderer.enabled = true;
        UpdateLaser();
    }

    void UpdateLaser()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, mousePos);

        Vector2 direction = mousePos - (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, direction.magnitude, mask);

        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
    }
}
