using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public const string bulletName = "Shoot Fruit";
    //public int amountBullets;
    private GameObject InventoryCanvas;

    public float offset;
    public GameObject BulletSample;
    public Transform shotpoint;
    public float damageAmount;

    private float timeBtwShots;
    public float startTimeBtwShots;

    private void Start()
    {
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

        if (!GameManagerScript.instance.isUIOpened && timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0) && InventoryManager.instance.bulletsAmount > 0)
            {
                Debug.Log(!GameManagerScript.instance.isUIOpened);
                GameObject bullet = Instantiate(BulletSample, shotpoint.position, transform.rotation);
                bullet.GetComponent<BulletStats>().damageAmount = damageAmount;
                timeBtwShots = startTimeBtwShots;
                InventoryCanvas.GetComponent<InventoryManager>().UseOneTimeWeapon(bulletName);
                InventoryManager.instance.bulletsAmount--;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    public int GetAmountBullets()
    {
        return InventoryManager.instance.bulletsAmount;
    }
    public void SetAmountBullets(int a)
    {
        InventoryManager.instance.bulletsAmount = a;
    }
}
