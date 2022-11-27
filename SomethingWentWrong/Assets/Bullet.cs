using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float offset;
    public GameObject BulletSample;
    public Transform shotpoint;
    public float damageAmount;

    private float timeBtwShots;
    public float startTimeBtwShots;

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0) && !InventoryManager.instance.isOpened)
            {
                GameObject bullet = Instantiate(BulletSample, shotpoint.position, transform.rotation);
                bullet.GetComponent<BulletStats>().damageAmount = damageAmount;
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
