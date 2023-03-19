using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    private EnemyShaderLogic _enemyShaderLogic;
    private EnemyAnimator _enemyAnimator;
    private EnemyDamagable _enemyDamagable;
    private EnemyAttack _enemyAttack;

    private void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyShaderLogic = GetComponentInChildren<EnemyShaderLogic>();
        _enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        _enemyDamagable = GetComponentInChildren<EnemyDamagable>();
        _enemyAttack = GetComponentInChildren<EnemyAttack>();

        _enemyMovement.es = this;
        _enemyAttack.es = this;
    }

    public EnemyMovement Movement => _enemyMovement;
    public EnemyShaderLogic Shader => _enemyShaderLogic;
    public EnemyAnimator Animator => _enemyAnimator;
    public EnemyDamagable Damagable => _enemyDamagable;
    public EnemyAttack Attack => _enemyAttack;
}
