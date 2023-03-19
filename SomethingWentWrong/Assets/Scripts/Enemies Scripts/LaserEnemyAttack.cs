using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserEnemyAttack : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    private WeaponType type = WeaponType.Enemy;

    public WeaponType Type { get { return type; } }

    private int damage = 5;

    public int Damage { get { return damage; } }

    [SerializeField] private LayerMask damagableLayers;

    private GameObject actualTarget;
    [SerializeField] private GameObject laser;
    private float distanceToTarget;
    private LaserEnemyMovement enemyLogic;
    public float triggerAttackDistance = 2f;
    private float angle;
    private float attackDuration = 0.3f;
    private Vector2 direction;
    [SerializeField] private float laserDamageSpeed;
    private float timeToDamage;
    private int attackDirection;
    private int actualAttackDirection;

    private void Awake()
    {
        enemyLogic = GetComponentInParent<LaserEnemyMovement>();
    }

    void Update()
    {
        distanceToTarget = Vector2.Distance(transform.position, enemyLogic.actualTarget.transform.position);
        direction = enemyLogic.actualTarget.transform.position - transform.position;
        direction.Normalize();
        attackDirection = Random.Range(0, 2) * 2 - 1;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + attackDirection * 80;
        if (distanceToTarget < triggerAttackDistance && enemyLogic.canMove)
        {
            Debug.Log(enemyLogic.actualTarget.name + " - " + direction + " - " + angle);
            actualAttackDirection = attackDirection;
            enemyLogic.canMove = false;
            StartCoroutine(LaserAttack(angle));
        }
    }

    private IEnumerator LaserAttack(float angle)
    {
        yield return new WaitForSeconds(1f);
        laser.gameObject.SetActive(true);
        Quaternion startRotation = Quaternion.Euler(Vector3.forward * angle);
        Quaternion endRotation = Quaternion.Euler(Vector3.forward * (angle + (-1) * actualAttackDirection * 160));
        float rate = 1f;
        laser.transform.rotation = startRotation;
        for (float t = 0; t < 1; t += rate * Time.deltaTime)
        {
            Vector2 laserDirection = laser.transform.rotation * Vector2.one;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, laserDirection.normalized, laser.transform.localScale.x, damagableLayers);
            timeToDamage -= Time.deltaTime;
            if (hit)
            {
                if (timeToDamage < 0)
                {
                    IDamagable target = hit.transform.GetComponentInChildren<IDamagable>();
                    target.GetDamage(this);
                    timeToDamage = laserDamageSpeed;
                }
            }

            laser.transform.rotation = Quaternion.Lerp(startRotation, endRotation, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        laser.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        enemyLogic.canMove = true;
    }

    public void stopAttack()
    {
        laser.gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
