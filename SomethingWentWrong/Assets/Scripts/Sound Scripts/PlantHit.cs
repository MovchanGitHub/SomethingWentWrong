using System.Collections.Generic;
using UnityEngine;

public class PlantHit : MonoBehaviour
{
    private AudioClip _clip;
    private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _hits;
    private int _ind;
    private int _hp;
    private int _prevHP;
    
    // Start is called before the first frame update
    void Start()
    {
        _hp = GetComponent<ResourceScript>().HP;
    }

    // Update is called once per frame
    void Update()
    {
        _prevHP = _hp;
        _hp = GetComponent<ResourceScript>().HP;
        if (_hp != _prevHP)
        {
            
        }
    }
}
