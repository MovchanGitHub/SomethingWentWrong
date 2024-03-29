using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static EnemiesSpawnSystem;

public class DayNightSwitching : MusicFaderScript
{
    private AudioSource _dayAudio;
    private float _dayVolume;
    private AudioSource _nightAudio;
    private float _nightVolume;
    private AudioSource _intenseAudio;
    private float _intenseVolume;

    private const int TransTime = 100;

    private DayTime _dayCycle;
    private bool _inCombat;
    public AudioMixer intenseMixer;
    public float intenseMixerVol;

    private bool _waveEnded;
    private bool _enemiesEnded;
    [SerializeField] private EnemiesSpawnSystem _enemiesScript;

    [SerializeField] private DayNightCycle DayNightCycleScript;

    private static DayNightSwitching instance;
    private Scene _scene;
    private float _dayTime;
    private float _nightTime;

    public static DayNightSwitching Instance
    {
        get => instance;
    }

    private int enemiesInCombat = 0;

    public int EnemiesInCombat
    {
        get => enemiesInCombat;
        set
        {
            enemiesInCombat = value;
            _inCombat = enemiesInCombat != 0;
        }
    }

    private void CombatStarts()
    {
        if (_inCombat)
        {
            MusicVolumeUpRoot(_intenseAudio, TransTime / 10, ref _intenseVolume);
        }
        else
        {
            MusicVolumeDownLinear(_intenseAudio, TransTime * 2, ref _intenseVolume);
        }
    }
    
    private void Awake()
    {
        // if (instance != null && instance != this)
        // {
        //     Destroy(this.gameObject);
        // }
        // else
        // {
        //     instance = this;
        // }
        
        _dayAudio = GetComponents<AudioSource>()[0];
        _dayAudio.Play();
        _dayAudio.loop = true;
        _dayAudio.volume = _dayVolume;
        _nightAudio = GetComponents<AudioSource>()[1];
        _nightAudio.volume = _nightVolume;
        _intenseAudio = GetComponents<AudioSource>()[2];
        _intenseAudio.Play();
        _intenseAudio.loop = true;
        _intenseAudio.volume = _intenseVolume;
    }

    private void Start()
    {
        _scene = SceneManager.GetActiveScene();
        DontDestroyOnLoad(gameObject);
        _dayTime = 0f;
        _nightTime = 0f;
    }

    private void Update()
    {
        if (_scene == SceneManager.GetActiveScene())
        {
            _enemiesEnded = _enemiesScript.ExistingEnemies == 0;
            if ((_dayCycle == DayTime.Midnight || _dayCycle == DayTime.Night) && _enemiesEnded)
                _waveEnded = true;
            intenseMixer.SetFloat("intenseMusicVol", _dayAudio.volume * intenseMixerVol - intenseMixerVol * 2);
            _dayCycle = DayNightCycleScript.dayCycle;
            if ((_dayCycle == DayTime.Midnight || _dayCycle == DayTime.Night) && !_waveEnded)
            {
                MusicVolumeDownRoot(_dayAudio, TransTime / 2, ref _dayVolume);
                if (_dayAudio.volume < 0.001f)
                {
                    if (_dayAudio.isPlaying)
                        _dayTime = _dayAudio.time;
                    _dayAudio.Stop();
                    _intenseAudio.Stop();
                }

                if (_nightAudio.isPlaying == false)
                {
                    _nightAudio.time = _nightTime;
                    _nightAudio.Play();
                }
                MusicVolumeUpRoot(_nightAudio, TransTime / 2, ref _nightVolume);

                if (_intenseVolume > 0.001f)
                {
                    MusicVolumeDownRoot(_intenseAudio, TransTime / 4, ref _intenseVolume);
                }

                if (_inCombat) _inCombat = !_inCombat;
            }
            else
            {
                if ((_dayCycle == DayTime.Sunrise || _waveEnded) && !_dayAudio.isPlaying)
                {
                    _dayAudio.time = _dayTime;
                    _dayAudio.Play();
                    _intenseAudio.Play();
                }

                MusicVolumeDownRoot(_nightAudio, TransTime / 2, ref _nightVolume);
                if (_nightAudio.volume < 0.001f)
                {
                    if (_nightAudio.isPlaying)
                        _nightTime = _nightAudio.time;
                    _nightAudio.Stop();
                    _waveEnded = false;
                }

                MusicVolumeUpRoot(_dayAudio, TransTime, ref _dayVolume);

                CombatStarts();
            }
        }
        else
        {
            MusicVolumeDownRoot(_dayAudio, TransTime * 2, ref _dayVolume);
            MusicVolumeDownRoot(_nightAudio, TransTime * 2, ref _nightVolume);
            if (_dayAudio.volume < 0.0001 && _nightAudio.volume < 0.0001)
                Destroy(gameObject);
        }
    }

}
