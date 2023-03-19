using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserEnemyDamagable : DamagableCharacter
{
    private EnemyShaderLogic esl;
    [SerializeField] private Slider slider;
    [SerializeField] private DamagePopup damagePopupPrefab;
    private LaserEnemyMovement enemyLogic;

    public override int HP
    {
        get { return hp; }
        set
        {
            spawnDamagePopup(transform.position, hp - value);
            if (value > 0)
                hp = value;
            else
                Die();
            slider.value = hp;
        }
    }

    private void Awake()
    {
        enemyLogic = GetComponentInParent<LaserEnemyMovement>();
    }

    private void Start()
    {
        esl = transform.parent.GetComponentInChildren<EnemyShaderLogic>();
    }

    public override void GetDamage(IWeaponable weapon)
    {
        base.GetDamage(weapon);
        enemyLogic.playFeedback(GameManager.GM.PlayerMovement.gameObject);

    }


    protected override void Die()
    {
        if (!creature.isOpenedInEcnyclopedia)
        {
            GameManager.GM.UI.Encyclopedia.OpenNewCreature(creature);
        }
        StartCoroutine(EnemyDie());
    }

    private IEnumerator EnemyDie()
    {
        if (lastWeapon.Type == WeaponType.Laser)
        {
            Laser laser = lastWeapon as Laser;
            esl.ChangeDissolvingColor(laser.LaserColor);
            esl.EnemyLaserDieShader();
            yield return new WaitForSeconds(1f);
        }

        Destroy(gameObject.transform.parent.gameObject);
    }

    public void spawnDamagePopup(Vector3 position, int damageAmount)
    {
        DamagePopup damagePopup = Instantiate(damagePopupPrefab, position, Quaternion.identity);
        damagePopup.setup(damageAmount);
    }
}
