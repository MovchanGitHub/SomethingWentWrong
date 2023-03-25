using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class LaserLogic : WeaponLogic
{
    private Camera cam;
    private LineRenderer lineRenderer;
    [SerializeField] private Transform firePoint;
    private Laser laser;
    
    [SerializeField] private LayerMask damagableLayers;

    //[SerializeField] [ColorUsage(true, true)]
    //private List<Color> laserColors;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color laserColor;
    
    [SerializeField]
    [ColorUsage(true, true)]
    private Color laserEmptyColor;

    private bool isShooting;

    private Renderer _laserRenderer;
    private MaterialPropertyBlock _laserPropBlock;

    [SerializeField] private GameObject startVFX;
    [SerializeField] private GameObject endVFX;
    
    [SerializeField] private float laserDamageSpeed;

    // сколько времени можно использовать лазер до того как его надо перезарядить 
    [SerializeField] private float timeBeforeReload;
    
    // сколько времени лазер используется с последней перезарядки
    private float usedTime;

    private float timeToDamage;

    private void Start()
    {
        usedTime = timeBeforeReload + 1f;
        cam = Camera.main;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        _laserRenderer = lineRenderer.GetComponent<Renderer>();
        _laserPropBlock = new MaterialPropertyBlock();
        laser = projectileSample.GetComponent<Laser>();
        laser.LaserColor = laserColor;
    }

    private bool allowReuseLaser = true;

    [SerializeField] private float intensity;
    
    IEnumerator EnableLaser()
    {
        if (!allowReuseLaser)
            yield break;

        allowReuseLaser = false;
        isShooting = true;
        GM.PlayerMovement.isoRenderer.PlayShoot();
            
        yield return new WaitForSeconds(0.2f);
        allowReuseLaser = true;

        if (!isShooting)
        {
            StopWeapon();
            yield break;
        }
        
        GM.PlayerMovement.usingWeapon = true;   
        
        startVFX.SetActive(true);
        endVFX.SetActive(true);
        GM.PlayerMovement.SetWalkingSpeed();
        lineRenderer.enabled = true;
        
        StartCoroutine(UpdateLaser());
        
        
        
        timeToDamage = laserDamageSpeed;
    }

    private bool tryToReloadLaser()
    {
        if (GM.InventoryManager.standartItemGrid.checkAmmo(AmmoType))
        {
            StartCoroutine(ReloadLaser());
            return true;
        }

        return false;
    }

    private IEnumerator ReloadLaser()
    {
        isShooting = false;
        DisableLaser();
        isShooting = true;
        usedTime = 0f;
        yield return new WaitForSeconds(0.1f);
        
        if (!isShooting) yield break;
        StartCoroutine(EnableLaser());
    }

    IEnumerator UpdateLaser()
    {
        while (isShooting)
        {
            usedTime += Time.deltaTime;
            
            if (usedTime > timeBeforeReload)
            {
                StopWeapon();
                tryToReloadLaser();
                break;
            }
            
            _laserRenderer.GetPropertyBlock(_laserPropBlock);
            _laserPropBlock.SetColor("_Color", Color.Lerp(laserEmptyColor, laser.LaserColor, (timeBeforeReload - usedTime) / timeBeforeReload) * intensity);
            /*// Changing colors of the laser
            if (Input.GetKeyDown("1"))
            {
                _laserPropBlock.SetColor("_Color", laserColors[0]);
                laser.LaserColor = laserColors[0];
            }
            else if (Input.GetKeyDown("2"))
            {
                _laserPropBlock.SetColor("_Color", laserColors[1]);
                laser.LaserColor = laserColors[1];
            }
            else if (Input.GetKeyDown("3"))
            {
                _laserPropBlock.SetColor("_Color", laserColors[2]);
                laser.LaserColor = laserColors[2];
            }*/
        
            _laserRenderer.SetPropertyBlock(_laserPropBlock);
            
            
            
            Vector2 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
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
        isShooting = false;
        GM.PlayerMovement.isoRenderer.PlayStopShooting();
        
        startVFX.SetActive(false);
        endVFX.SetActive(false);
        lineRenderer.enabled = false;
    }
    
    override public bool UseWeapon()
    {
        if (usedTime <= timeBeforeReload)
        {
            StartCoroutine(EnableLaser());
            return true;
        }
        
        return tryToReloadLaser();
    }
    override public void StopWeapon() { DisableLaser(); }
    
    public override void CanNotUseWeapon()
    {
        Debug.Log("нельзя стрелять: нет кристалла");
    }
}
