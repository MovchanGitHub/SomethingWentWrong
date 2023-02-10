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

    private bool isShooting;

    [SerializeField] private float laserDamageSpeed;

    void Start()
    {
        mask = LayerMask.GetMask("Minable Objects");
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(EnableLaser());
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            DisableLaser();
        }
    }
    
    IEnumerator EnableLaser()
    {
        Debug.Log("EnableLaser");
        isShooting = true;
        IsometricPlayerMovementController.Instance.isoRenderer.PlayUseLaserAnim();
            
        yield return new WaitForSeconds(0.2f);
        IsometricPlayerMovementController.Instance.isShooting = true;
        IsometricPlayerMovementController.Instance.MinimizeSpeed();
        lineRenderer.enabled = true;
        
        StartCoroutine(UpdateLaser());
    }

    IEnumerator UpdateLaser()
    {
        Debug.Log("UpdateLaser");

        if (!isShooting)
        {
            DisableLaser();
        }

        float timeToDamage = laserDamageSpeed;
        
        while (isShooting)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, mousePos);

            Vector2 direction = mousePos - (Vector2)transform.position;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, direction.magnitude, mask);

            timeToDamage -= Time.deltaTime;
            
            if (hit)
            {
                lineRenderer.SetPosition(1, hit.point);

                if (timeToDamage < 0)
                {
                    ResourceScript res = hit.transform.GetComponent<ResourceScript>();
                    res.GetDamage(3);
                    timeToDamage = laserDamageSpeed;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    void DisableLaser()
    {
        Debug.Log("DisableLaser");

        if (isShooting)
            IsometricPlayerMovementController.Instance.isoRenderer.PlayStopLaserAnim();
        
        isShooting = false;
        
        IsometricPlayerMovementController.Instance.isShooting = false;
        lineRenderer.enabled = false;
    }
}
