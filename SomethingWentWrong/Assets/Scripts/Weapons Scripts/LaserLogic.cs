using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLogic : WeaponLogic
{
    private Camera cam;
    private LineRenderer lineRenderer;
    [SerializeField] private Transform firePoint;
    private Laser laser;
    
    [SerializeField] private LayerMask damagableLayers;

    [SerializeField] [ColorUsage(true, true)]
    private List<Color> laserColor;

    private bool isShooting;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    [SerializeField] private GameObject startVFX;
    [SerializeField] private GameObject endVFX;
    
    [SerializeField] private float laserDamageSpeed;

    private void Start()
    {
        cam = Camera.main;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        _renderer = lineRenderer.GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
        laser = projectileSample.GetComponent<Laser>();
        laser.LaserColor = laserColor[0];
    }

    void Update()
    {
        _renderer.GetPropertyBlock(_propBlock);
        
        // Changing colors of the laser
        if (Input.GetKeyDown("1"))
        {
            _propBlock.SetColor("_Color", laserColor[0]);
            laser.LaserColor = laserColor[0];
        }
        else if (Input.GetKeyDown("2"))
        {
            _propBlock.SetColor("_Color", laserColor[1]);
            laser.LaserColor = laserColor[1];
        }
        else if (Input.GetKeyDown("3"))
        {
            _propBlock.SetColor("_Color", laserColor[2]);
            laser.LaserColor = laserColor[2];
        }
        
        _renderer.SetPropertyBlock(_propBlock);
    }
    
    IEnumerator EnableLaser()
    {
        isShooting = true;
        GameManager.GM.PlayerMovement.isoRenderer.PlayShoot();
        GameManager.GM.PlayerMovement.usingWeapon = true;
            
        yield return new WaitForSeconds(0.2f);
        GameManager.GM.PlayerMovement.SetWalkingSpeed();
        lineRenderer.enabled = true;
        
        StartCoroutine(UpdateLaser());
    }

    IEnumerator UpdateLaser()
    {
        if (!isShooting)
        {
            StopWeapon();
        }
        else
        {
            startVFX.SetActive(true);
            endVFX.SetActive(true);
        }

        float timeToDamage = laserDamageSpeed;

        
        while (isShooting)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, mousePos);
            startVFX.transform.SetPositionAndRotation(firePoint.position, Quaternion.Euler(0f, Vector2.Angle(firePoint.position, mousePos), 0f));
            endVFX.transform.SetPositionAndRotation(mousePos, Quaternion.Euler(0f, Vector2.Angle(firePoint.position, mousePos), 0f));

            Vector2 direction = mousePos - (Vector2)transform.position;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, direction.magnitude, damagableLayers);

            timeToDamage -= Time.deltaTime;
            
            if (hit)
            {
                lineRenderer.SetPosition(1, hit.point);
                endVFX.transform.SetPositionAndRotation(hit.point, Quaternion.Euler(0f, Vector2.Angle(firePoint.position, hit.point), 0f));
                
                if (timeToDamage < 0)
                {
                    IDamagable target = hit.transform.GetComponentInChildren<IDamagable>();
                    target.GetDamage(laser);
                    timeToDamage = laserDamageSpeed;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    void DisableLaser()
    {
        if (isShooting)
        {
            isShooting = false;
            GameManager.GM.PlayerMovement.isoRenderer.PlayStopShooting();
        }


        GameManager.GM.PlayerMovement.usingWeapon = false;
        startVFX.SetActive(false);
        endVFX.SetActive(false);
        lineRenderer.enabled = false;
    }
    
    override public void UseWeapon() { StartCoroutine(EnableLaser()); }
    override public void StopWeapon() { DisableLaser(); }
}
