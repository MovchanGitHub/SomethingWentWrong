using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bomb : MonoBehaviour, IWeaponable
{
    // IWeaponable's implementation
    [SerializeField] private WeaponType type;
    
    public WeaponType Type { get { return type; } }

    [SerializeField] private int damage;

    public int Damage { get { return damage; } }
    
    
    // Bomb's unique values
    [SerializeField] private float lifeTime;
    [SerializeField] private float beforeAnimTime;
    [SerializeField] private LayerMask damagableLayers;
    private GameObject _collider2D;
    private GameObject _area;
    [SerializeField] private Animator _animator;
    private Collider2D[] hitObjects;

    private AudioSource _audioSource;
    public List<AudioClip> _explosions;
    public GameObject playSound;

    private void Awake()
    {
        _collider2D = gameObject.transform.GetChild(1).gameObject;
        _area = gameObject.transform.GetChild(2).gameObject;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(beforeAnimTime);
        _animator.Play("Explosion");
        yield return new WaitForSeconds(lifeTime - beforeAnimTime);
        _collider2D.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        Explode();
    }

    private void Explode()
    {
        hitObjects = Physics2D.OverlapCircleAll(_area.transform.position, 4, damagableLayers);

        //_audioSource.PlayOneShot(_explosions[UnityEngine.Random.Range(0, 2)]); 
        
        foreach (Collider2D hitObject in hitObjects)
        {
            hitObject.transform.GetComponentInParent<IDamagable>().GetDamage(this, gameObject);
        }
        GameObject playSoundObj = Instantiate(playSound, transform.position, Quaternion.identity);
        PlaySound playSoundTemp = playSoundObj.GetComponent<PlaySound>();
        playSoundTemp.audioClip = _explosions[UnityEngine.Random.Range(0, 2)];
        playSoundTemp.audioMixer = _audioSource.outputAudioMixerGroup;
        playSoundTemp.Play();
        
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_area.transform.position, _area.transform.localScale.y);
    }
}
