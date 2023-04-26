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
    [SerializeField] private int timesToDrop;
    public int TimesToDrop { get { return timesToDrop; } }

    public int positionIndex;
    private AudioSource _audioSource;
    public GameObject playSound;

    public int HP
    {
        get { return hp; }
        set
        {
            spawnDamagePopup(transform.position, hp - value);
            slider.value = value;
            if ((int)(((hp - value + currentDamage) / lootDropBarrier)) >= 1)
            {
                DropItem(((hp - value + currentDamage) / lootDropBarrier) * dropCount);
                currentDamage = (hp - value + currentDamage) - lootDropBarrier * ((hp - value + currentDamage) / lootDropBarrier);
            }
            else
            {
                currentDamage += hp - value;
            }

            hp = value;
            if (hp <= 0)
                StartCoroutine(Die());
        }
    }

    public int MaxHP { get; set; }

    public void GetDamage(IWeaponable weapon, GameObject sender = null)
    {
        HP -= weapon.Damage;
        if (hp > 0)
            ObjectHitSound(_audioSource);
    }

    private void Awake()
    {
        if (!GM || !GM.Spawner)
            positionIndex = -1;
        else
            positionIndex = GM.Spawner.Resources.PositionIndex;
        _audioSource = GetComponents<AudioSource>()[0];
    }


    // Resource unique methods
    [SerializeField] private int lootDropBarrier = 5;
    [SerializeField] private CreaturesBase creature;
    private int currentDamage;

    [SerializeField] private GameObject drop;
    [SerializeField] private int dropCount = 1;
    [SerializeField] private float spread = 2f;
    [SerializeField] private float dropSpeed = 5f;
    public GameObject Drop { get { return drop; } }

    private void DropItem(int dropAmount)
    {
        int amountOfDrop = Mathf.Clamp(dropAmount, 1, dropCount * timesToDrop);
        if (amountOfDrop > 0)
        {
            timesToDrop--;
        }
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

    private IEnumerator Die()
    {
        if (!creature.isOpenedInEcnyclopedia)
        {
            GM.UI.Encyclopedia.EncyclopediaScript.OpenNewCreature(creature);
        }

        GameObject playSoundObj = Instantiate(playSound, transform.position, Quaternion.identity);
        PlaySound playSoundTemp = playSoundObj.GetComponent<PlaySound>();
        playSoundTemp.audioClip = _audioSource.clip;
        playSoundTemp.audioMixer = _audioSource.outputAudioMixerGroup;
        playSoundTemp.Play();
        
        if (GM.Spawner)
            GM.Spawner.Resources.PurgePointWithIndex(positionIndex);
        
        Destroy(gameObject);

        yield return new WaitForSeconds(playSoundTemp.audioClip.length * 2);
        Destroy(playSoundObj);
    }

    public void spawnDamagePopup(Vector3 position, int damageAmount)
    {
        DamagePopup damagePopup = Instantiate(damagePopupPrefab, position, Quaternion.identity);
        damagePopup.setup(damageAmount);
    }
    
    public static void ObjectHitSound(AudioSource _audioSource)
    {
        _audioSource.pitch = 1 + UnityEngine.Random.Range(-0.15f, 0.15f);
        _audioSource.Play();
    }
}
