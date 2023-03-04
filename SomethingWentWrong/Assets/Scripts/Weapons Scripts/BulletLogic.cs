using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class BulletLogic : WeaponLogic
{
    public const string bulletName = "Shoot Fruit";
    //public int amountBullets;

    public float offset;
    private Bullet bullet;
    public Transform shotpoint;

    private float timeBtwShots;
    public float startTimeBtwShots;

    private void Start()
    {
        bullet = projectileSample.GetComponent<Bullet>();
        //InventoryCanvas = InventoryManager.instance.gameObject;
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

    private IEnumerator ThrowBullet()
    {
        GM.PlayerMovement.isoRenderer.PlayShoot();
        Instantiate(bullet, shotpoint.position, transform.rotation);
        timeBtwShots = startTimeBtwShots;
        //InventoryCanvas.GetComponent<InventoryManager>().UseOneTimeWeapon(bulletName);
        //InventoryManager.instance.bulletsAmount--;
        yield return new WaitForSeconds(.2f);
        GM.PlayerMovement.isoRenderer.PlayStopShooting();
    }
    
    override public void UseWeapon() { StartCoroutine(ThrowBullet()); }
    override public void StopWeapon() {  }
}
