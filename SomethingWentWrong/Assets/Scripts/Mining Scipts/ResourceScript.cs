using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class ResourceScript : MonoBehaviour, IDamagable
{
    // IDamagable's implementation
    [SerializeField] private int hp;
    [SerializeField] private Slider slider;
    [SerializeField] private DamagePopup damagePopupPrefab;

    public int positionIndex;
    
    public int HP
    {
        get { return hp; }
        set
        {
            spawnDamagePopup(transform.position, hp - value);
            slider.value = value;
            if ((int)(((hp - value + currentDamage) / lootDropBarrier) ) >= 1)
            {
                DropItem(((hp - value + currentDamage) / lootDropBarrier) * dropCount);
                currentDamage = (hp - value + currentDamage) - lootDropBarrier * ((hp - value + currentDamage) / lootDropBarrier);
            }
            else
            {
                currentDamage += hp - value;
            }
            
            if (value > 0)
                hp = value;
            else
                Die();
        }
    }

    public int MaxHP { get { throw new System.NotSupportedException("Don`t use Plants` MaxHP getter! >:("); } set { throw new System.NotSupportedException("Don`t use Plants` MaxHP setter! >:("); } }
    
    public void GetDamage(IWeaponable weapon)
    {
        HP -=  weapon.Damage;
    }

    private void Awake()
    {
        if (!GM || !GM.Spawner)
            positionIndex = -1;
        else
            positionIndex = GM.Spawner.Resources.PositionIndex;
    }


    // Resource unique methods
    [SerializeField] private int lootDropBarrier = 5;
    [SerializeField] private CreaturesBase creature;
    private int currentDamage;

    [SerializeField] private GameObject drop;
    [SerializeField] private int dropCount = 1;
    [SerializeField] private float spread = 2f;
    [SerializeField] private float dropSpeed = 5f;
    
    private void DropItem(int dropAmount)
    {
        int amountOfDrop = Mathf.Clamp(dropAmount, 0, dropCount);
        while (amountOfDrop > 0)
        {
            amountOfDrop--;
            Vector3 pos = transform.position;
            pos.x += spread * UnityEngine.Random.value - spread / 2;
            pos.y += spread * UnityEngine.Random.value - spread / 2;
            GameObject dropObject = Instantiate(drop);
            dropObject.transform.position = transform.position;
            dropObject.GetComponent<PickUpScript>().StartMove(pos, dropSpeed);
        }
    }

    private void Die()
    {
        if (!creature.isOpenedInEcnyclopedia)
        {
            GM.UI.Encyclopedia.OpenNewCreature(creature);
        }

        GM.Spawner.Resources.PurgePointWithIndex(positionIndex);
        
        Destroy(gameObject);
    }

    public void spawnDamagePopup(Vector3 position, int damageAmount)
    {
        DamagePopup damagePopup = Instantiate(damagePopupPrefab, position, Quaternion.identity);
        damagePopup.setup(damageAmount);
    }
}
