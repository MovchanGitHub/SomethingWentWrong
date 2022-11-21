using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DayNightSwitching : MusicFaderScript
{
    private AudioSource _dayAudio;
    private float _dayVolume;
    private AudioSource _nightAudio;
    private float _nightVolume;
    private AudioSource _intenseAudio;
    private float _intenseVolume;
    private const int TransTime = 500;
    public GameObject adaptiveParameter;
    private DayTime _dayCycle;
    private bool _inCombat;
    public AudioMixer intenseMixer;
    public float intenseMixerVol;

    private void CombatStarts()
    {
        if (_inCombat)
        {
            MusicVolumeUpSqr(_intenseAudio, TransTime, ref _intenseVolume);
        }
        else
        {
            MusicVolumeDownRoot(_intenseAudio, TransTime / 5, ref _intenseVolume);
        }
    }
    
    private void Awake()
    {
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
        intenseMixer.SetFloat("intenseMusicVol", _dayAudio.volume * intenseMixerVol - intenseMixerVol * 2);
        if (Input.GetKeyDown(KeyCode.P)) _inCombat = !_inCombat;

        _dayCycle = adaptiveParameter.GetComponent<DayNightCycle>().dayCycle;

        if (_dayCycle == DayTime.Midnight || _dayCycle == DayTime.Night)
        {
            MusicVolumeDownRoot(_dayAudio, TransTime / 2, ref _dayVolume);
            if (_dayAudio.volume < 0.001f)
            {
                _dayAudio.Stop();
                _intenseAudio.Stop();
            }
            
            if (_nightAudio.isPlaying == false) _nightAudio.Play();
            MusicVolumeUpRoot(_nightAudio, TransTime, ref _nightVolume);

            if (_intenseVolume > 0.001f)
            {
                MusicVolumeDownRoot(_intenseAudio, TransTime / 2, ref _intenseVolume);
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
            }
            
            MusicVolumeUpRoot(_dayAudio, TransTime, ref _dayVolume);
            
            CombatStarts();
        }
    }
}
