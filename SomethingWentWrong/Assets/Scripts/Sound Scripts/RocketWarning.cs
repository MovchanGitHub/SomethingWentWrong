using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketWarning : MonoBehaviour
{
    private int _HP;
    private int _maxHP;
    private bool _firstHit;
    private AudioSource _warning;
    
    // Start is called before the first frame update
    void Start()
    {
        _warning = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _HP = GetComponent<LightHouse>().HP;
        _maxHP = GetComponent<LightHouse>().MaxHP;
        if (_HP < _maxHP && _HP >= 0 && !_firstHit)
        {
            _warning.Play();
            _firstHit = true;
        }
        else if (_HP <= _maxHP / 4 && _HP >= 0 && _firstHit)
        {
            _warning.loop = true;
            if (!_warning.isPlaying)
            {
                _warning.Play();
            }
        }
        else if (_HP <= 0)
        {
            _warning.Stop();
        }
    }
}
