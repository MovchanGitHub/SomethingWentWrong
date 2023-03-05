using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class BulletLogic : WeaponLogic
{
    public const string bulletName = "Shoot Fruit";

    private Bullet bullet;
    public Transform shotpoint;

	
    private void Start()
    {
        bullet = projectileSample.GetComponent<Bullet>();
    }

    private IEnumerator ThrowBullet()
    {
        GM.PlayerMovement.isoRenderer.PlayShoot();
        Instantiate(bullet, shotpoint.position, transform.rotation)
            .direction = Vector2.ClampMagnitude(GM.Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position, 1);
        
        yield return new WaitForSeconds(.3f);
        GM.PlayerMovement.isoRenderer.PlayStopShooting();
    }
    
    override public void UseWeapon() { StartCoroutine(ThrowBullet()); }
    override public void StopWeapon() {  }
}
