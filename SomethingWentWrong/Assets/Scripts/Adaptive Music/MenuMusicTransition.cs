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
    public AudioMixer audioMixer;
    private float _timeToFade;
    private float _timeElapsed;

    private void Awake()
    {
        
    }

    private void Start()
    {
        audioMixer.SetFloat("Cutoff", 22000f);
        audioMixer.SetFloat("DryVol", 0f);
        audioMixer.SetFloat("WetVol", -10000f);
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0.0001f;
        _audioSource.Play();
        _menuScene = SceneManager.GetActiveScene();
        _timeToFade = 5f;
        _timeElapsed = 0;
        _audioSource.volume = 0;
    }

    private void Update()
    {
        //Debug.Log(Time.timeScale);
        if (_menuScene != SceneManager.GetActiveScene())
        {
            _audioSource.volume = Mathf.Pow(Mathf.Lerp(0, 1, _timeElapsed / _timeToFade), 2);
            _timeElapsed -= Time.deltaTime;
            if (_timeElapsed < 0)
                _timeElapsed = 0;
            if (_audioSource.volume < 0.001f)
            {
                Destroy(gameObject);
            } 
        }
        else if (_audioSource.volume < 1)
        {
            _audioSource.volume = Mathf.Lerp(0, 1, _timeElapsed / _timeToFade);
            _timeElapsed += Time.deltaTime;
        }
    }
}