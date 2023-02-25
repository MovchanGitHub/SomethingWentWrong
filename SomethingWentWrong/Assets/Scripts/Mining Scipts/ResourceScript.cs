using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceScript : MonoBehaviour, IDamagable
{
    // IDamagable's implementation
    [SerializeField] private int hp;
    [SerializeField] private Slider slider;
    
    public int HP
    {
        get { return hp; }
        set
        {
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
    
    public void GetDamage(IWeaponable weapon)
    {
        HP -=  weapon.Damage;
    }
    
    
    // Resource unique methods
    [SerializeField] private int lootDropBarrier = 5;
    [SerializeField] private CreaturesBase creature;
    private int currentDamage;

    [SerializeField] private GameObject drop;
    [SerializeField] private int dropCount = 1;
    [SerializeField] private float spread = 2f;
    [SerializeField] private float dropSpeed = 5f;
    
    private void DropItem(int dropCount)
    {
        int amountOfDrop = dropCount;
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
            EncyclopediaManager.Instance.OpenNewCreature(creature);
        }
        Destroy(gameObject);
    }
}
