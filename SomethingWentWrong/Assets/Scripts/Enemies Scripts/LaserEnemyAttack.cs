using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;


public class LaserEnemyAttack : EnemyAttack
{

    [SerializeField] private GameObject laser;
    private float angle;
    [SerializeField] private float laserDamageSpeed;
    private float timeToDamage;
    private int attackDirection;
    private int actualAttackDirection;
    private Vector2 targetPosition;
    [HideInInspector] public List<GameObject> damagedEntities;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        distanceToTarget = Vector2.Distance(transform.position, enemyLogic.actualTarget.transform.position);
        
        if (distanceToTarget < triggerAttackDistance && enemyLogic.CanMove)
        {
            direction = enemyLogic.actualTarget.transform.position - transform.position;
            direction.Normalize();
            attackDirection = Random.Range(0, 2) * 2 - 1;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + attackDirection * 60; //80
            actualAttackDirection = attackDirection;
            enemyLogic.CanMove = false;
            enemyLogic.rotateSprite();
            StartCoroutine(LaserAttack(angle));
        }
    }

    private IEnumerator LaserAttack(float angle)
    {
        es.Animator.AttackTrigger();
        yield return new WaitForSeconds(timeBeforeAttack);
        _audioSource.volume = 0.65f;
        _audioSource.pitch = 1.7f + Random.Range(-0.15f, 0.15f);
        _audioSource.Play();
        Quaternion startRotation = Quaternion.Euler(Vector3.forward * angle);
        Quaternion endRotation = Quaternion.Euler(Vector3.forward * (angle + (-1) * actualAttackDirection * 100)); //160
        float rate = 1f;
        laser.transform.rotation = startRotation;
        laser.gameObject.SetActive(true);
        for (float t = 0; t < 1; t += rate * Time.deltaTime)
        {   /*
            Vector2 laserDirection = laser.transform.rotation * Vector2.one;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)laser.transform.position, laserDirection.normalized, laser.transform.localScale.x, damagableLayers);
            timeToDamage -= Time.deltaTime;
            if (hit && !damagedEntities.Contains(hit.transform.gameObject))
            {
                if (timeToDamage < 0)
                {
                    IDamagable target = hit.transform.GetComponentInChildren<IDamagable>();
                    target.GetDamage(this);
                    damagedEntities.Add(hit.transform.gameObject);
                    timeToDamage = laserDamageSpeed;
                }
            }
            */
            laser.transform.rotation = Quaternion.Lerp(startRotation, endRotation, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        laser.gameObject.SetActive(false);
        _audioSource.volume = 0f;
        yield return new WaitForSeconds(timeAfterAttack);
        enemyLogic.CanMove = true;
        damagedEntities.Clear();
        es.Animator.StopAttackTrigger();
    }

    public override void stopAttack()
    {
        laser.gameObject.SetActive(false);
        StopAllCoroutines();
        enemyLogic.CanMove = true;
    }
}
