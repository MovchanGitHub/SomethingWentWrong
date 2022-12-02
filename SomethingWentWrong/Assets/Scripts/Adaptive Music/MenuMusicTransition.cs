using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuMusicTransition : MusicFaderScript
{
    private AudioSource _audioSource;
    private Scene _menuScene;
    private float _volume;
    private const int TransTime = 500;
    public AudioMixer audioMixer;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
        _volume = 0.0001f;
        _audioSource.volume = _volume;
        _audioSource.Play();
        _menuScene = SceneManager.GetActiveScene();
    }

    private void Start()
    {
        audioMixer.SetFloat("Cutoff", 22000f);
        audioMixer.SetFloat("DryVol", 0f);
        audioMixer.SetFloat("WetVol", -10000f);
    }

    private void Update()
    {
        
        if (_menuScene != SceneManager.GetActiveScene())
        {
            /*
            _audioSource.volume = MathF.Sqrt(_volume);
            _volume -= _volume / TransTime;
            if (MathF.Sqrt(_volume) < 0.0001f)
            {
                Destroy(gameObject);
            } */
            
            MusicVolumeDownRoot(_audioSource, TransTime / 3, ref _volume);
            if (MathF.Sqrt(_volume) < 0.001f)
            {
                Destroy(gameObject);
            } 
        }
        else if (MathF.Sqrt(_volume - _volume / TransTime) < 1)
        {
            /*
            _audioSource.volume = MathF.Sqrt(_volume);
            _volume += _volume / TransTime; */
            
            MusicVolumeUpRoot(_audioSource, TransTime, ref _volume);
        }
        
        
    }
}