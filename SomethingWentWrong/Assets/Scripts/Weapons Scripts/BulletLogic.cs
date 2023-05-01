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


    public AudioSource BulletSource;

    private void Start()
    {
        bullet = projectileSample.GetComponent<Bullet>();
    }

    private IEnumerator ThrowBullet()
    {
        BulletSource.Play();
        GM.PlayerMovement.isoRenderer.PlayShoot();
        Instantiate(bullet, shotpoint.position, transform.rotation)
            .direction = Vector2.ClampMagnitude(GM.Camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position, 1);
        
        yield return new WaitForSeconds(.3f);
        GM.PlayerMovement.isoRenderer.PlayStopShooting();
    }

    override public bool UseWeapon()
    {
        if (GM.InventoryManager.standartItemGrid.checkAmmo(AmmoType))
        {
            StartCoroutine(ThrowBullet());
            return true;
        }

        return false;
    }
    override public void StopWeapon() {  }
    
    public override void CanNotUseWeapon()
    {
        Debug.Log("нельзя стрелять: нет шляпки острого подсолнуха");
    }
}
