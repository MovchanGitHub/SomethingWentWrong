using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : WeaponLogic
{
    public const string bulletName = "Shoot Fruit";
    //public int amountBullets;
    private GameObject InventoryCanvas;

    public float offset;
    private Bullet bullet;
    public Transform shotpoint;

    private float timeBtwShots;
    public float startTimeBtwShots;

    private void Start()
    {
        bullet = projectileSample.GetComponent<Bullet>();
        if (InventoryManager.instance != null)
        {
            InventoryManager.instance.BulletSpawner = gameObject;
        }
        InventoryCanvas = InventoryManager.instance.gameObject;
    }

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        timeBtwShots -= Time.deltaTime;

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
        timeBtwShots = startTimeBtwShots;
        InventoryCanvas.GetComponent<InventoryManager>().UseOneTimeWeapon(bulletName);
        InventoryManager.instance.bulletsAmount--;
    }

    public int GetAmountBullets()
    {
        return InventoryManager.instance.bulletsAmount;
    }
    public void SetAmountBullets(int a)
    {
        InventoryManager.instance.bulletsAmount = a;
    }
    
    override public void UseWeapon() { ThrowBullet(); }
    override public void StopWeapon() {  }
}
