using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static EnemiesSpawnSystem;

public class DayNightSwitching : MusicFaderScript
{
    private AudioSource _dayAudio;
    private float _dayVolume;
    private AudioSource _nightAudio;
    private float _nightVolume;
    private AudioSource _intenseAudio;
    private float _intenseVolume;

    private const int TransTime = 500;

    private DayTime _dayCycle;
    private bool _inCombat;
    public AudioMixer intenseMixer;
    public float intenseMixerVol;

    private bool _waveEnded;
    private bool _enemiesEnded;
    private EnemiesSpawnSystem _enemiesScript;

    [SerializeField] private DayNightCycle DayNightCycleScript;

    private static DayNightSwitching instance;

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
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        
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

    private void Update()
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
                _dayAudio.Stop();
                _intenseAudio.Stop();
            }
            
            if (_nightAudio.isPlaying == false) _nightAudio.Play();
            MusicVolumeUpRoot(_nightAudio, TransTime / 2, ref _nightVolume);

            if (_intenseVolume > 0.001f)
            {
                MusicVolumeDownRoot(_intenseAudio, TransTime / 4, ref _intenseVolume);
            }

            if (_inCombat) _inCombat = !_inCombat;
        }
        else
        {
            if (_dayCycle == DayTime.Sunrise && _dayAudio.isPlaying == false)
            {
                _dayAudio.Play();
                _intenseAudio.Play();
            }
            
            MusicVolumeDownRoot(_nightAudio, TransTime / 2, ref _nightVolume);
            if (_nightAudio.volume < 0.001f)
            {
                _nightAudio.Stop();
                _waveEnded = false;
            }
            
            MusicVolumeUpRoot(_dayAudio, TransTime, ref _dayVolume);
            
            CombatStarts();
        }
    }

}
