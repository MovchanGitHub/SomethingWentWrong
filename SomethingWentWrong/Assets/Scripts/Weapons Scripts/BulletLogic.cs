using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletLogic : WeaponLogic
{
    public const string bulletName = "Shoot Fruit";
    //public int amountBullets;

    public float offset;
    private Bullet bullet;
    public Transform shotpoint;

    public float startTimeBtwShots;

    private void Start()
    {
        bullet = projectileSample.GetComponent<Bullet>();
        //InventoryCanvas = InventoryManager.instance.gameObject;
    }

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        /*if (!GameManagerScript.instance.isUIOpened && timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0) && InventoryManager.instance.bulletsAmount > 0)
            {
                ThrowBullet();
            }
        }*/
    }

    private void ThrowBullet()
    {
        Instantiate(bullet, shotpoint.position, transform.rotation);
        StartCoroutine(GoCoolDown());
        //InventoryCanvas.GetComponent<InventoryManager>().UseOneTimeWeapon(bulletName);
        //InventoryManager.instance.bulletsAmount--;
    }
    
    override public void UseWeapon() { ThrowBullet(); }
    override public void StopWeapon() {  }
}
