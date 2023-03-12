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


    private GameObject playerTarget;
    [SerializeField] private GameObject laser;
    private float distanceToPlayer;
    private LaserEnemyMovement enemyLogic;
    public float triggerAttackDistance = 2f;
    private float angle;
    private float attackDuration = 1f;

    private void Awake()
    {
        enemyLogic = GetComponentInParent<LaserEnemyMovement>();
    }

    void Start()
    {
        playerTarget = GameManager.GM.PlayerMovement.gameObject;
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, playerTarget.transform.position);
        Vector2 direction = playerTarget.transform.position - transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 45;
        if (distanceToPlayer < triggerAttackDistance && enemyLogic.canMove)
        {
            enemyLogic.canMove = false;
            StartCoroutine(LaserAttack());
        }
    }

    private IEnumerator LaserAttack()
    {
        yield return new WaitForSeconds(3f);
        laser.gameObject.SetActive(true);
        Quaternion startRotation = Quaternion.Euler(Vector3.forward * angle);
        Quaternion endRotation = Quaternion.Euler(Vector3.forward * (angle + 90));
        float rate = 3f;
        laser.transform.rotation = startRotation;
        for (float t = 0; t < 1; t += rate * Time.deltaTime)
        {
            laser.transform.rotation = Quaternion.Lerp(startRotation, endRotation, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        laser.gameObject.SetActive(false);
        enemyLogic.canMove = true;
    }
}
