using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static GameManager;
public class PauseEffect : MonoBehaviour
{
    public AudioMixer audioMixer;
    private float _cutoffValue = 22000;
    private float _dryValue = 0;
    private float _wetValue = -5000;
    private const int TransitionTime = 500;
    void Start()
    {
        audioMixer.SetFloat("Cutoff", _cutoffValue);
        audioMixer.SetFloat("DryVol", _dryValue);
        audioMixer.SetFloat("WetVol", _wetValue);
    }

    void Update()
    {
        if (Time.timeScale < 0.1)
        {
            if (_cutoffValue > 460f) _cutoffValue -= (22000f - 452f) / (TransitionTime * 1.2f);
            else _cutoffValue = 452f;
            if (_dryValue > -5000f) _dryValue -= 10000f / (TransitionTime * 8);
            else _dryValue = -5000f;
            if (_wetValue < 0) _wetValue += 10000f / (TransitionTime * 3);
            else _wetValue = 0;
            audioMixer.SetFloat("Cutoff", _cutoffValue);
            audioMixer.SetFloat("DryVol", _dryValue);
            audioMixer.SetFloat("WetVol", _wetValue);
        }
        else
        {
            if (_cutoffValue < 22000f) _cutoffValue += (22000f - 452f) / (TransitionTime);
            else _cutoffValue = 22000f;
            if (_dryValue < 0) _dryValue += 10000f / (TransitionTime * 3);
            else _dryValue = 0;
            if (_wetValue > -5000f) _wetValue -= 10000f / (TransitionTime * 8);
            else _wetValue = -5000f;
            audioMixer.SetFloat("Cutoff", _cutoffValue);
            audioMixer.SetFloat("DryVol", _dryValue);
            audioMixer.SetFloat("WetVol", _wetValue);
        }
    }
}
