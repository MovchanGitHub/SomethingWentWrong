using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyScript : MonoBehaviour
{
    private LaserEnemyMovement _enemyMovement;
    private EnemyShaderLogic _enemyShaderLogic;
    private EnemyAnimator _enemyAnimator;
    private LaserEnemyDamagable _enemyDamagable;
    private LaserEnemyAttack _enemyAttack;

    private void Awake()
    {
        _enemyMovement = GetComponent<LaserEnemyMovement>();
        _enemyShaderLogic = GetComponentInChildren<EnemyShaderLogic>();
        _enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        _enemyDamagable = GetComponentInChildren<LaserEnemyDamagable>();
        _enemyAttack = GetComponentInChildren<LaserEnemyAttack>();

        _enemyMovement.es = this;
        _enemyAttack.es = this;
    }

    public LaserEnemyMovement Movement => _enemyMovement;
    public EnemyShaderLogic Shader => _enemyShaderLogic;
    public EnemyAnimator Animator => _enemyAnimator;
    public LaserEnemyDamagable Damagable => _enemyDamagable;
    public LaserEnemyAttack Attack => _enemyAttack;
}