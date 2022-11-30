using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseEffect : MonoBehaviour
{
    public AudioMixer audioMixer;
    private float _cutoffValue = 22000;
    private float _dryValue = 0;
    private float _wetValue = -10000;
    private const int TransitionTime = 500;

    private InGameMenuScript _inGameMenuScript;
    private SettingsScript _settingsScript;
    private DeathScreen _deathScreen;
    private WinScreen _winScreen;
    void Start()
    {
        audioMixer.SetFloat("Cutoff", _cutoffValue);
        audioMixer.SetFloat("DryVol", _dryValue);
        audioMixer.SetFloat("WetVol", _wetValue);

        _inGameMenuScript = GetComponent<InGameMenuScript>();
        _settingsScript = GetComponent<SettingsScript>();
        _deathScreen = GetComponent<DeathScreen>();
        _winScreen = GetComponent<WinScreen>();
    }

    void Update()
    {
        if (_inGameMenuScript.isOpened || _settingsScript.isOpened || _deathScreen.isOpened || _winScreen.isOpened)
        {
            if (_cutoffValue > 460f) _cutoffValue -= (_cutoffValue - 452f) / (TransitionTime * 2);
            else _cutoffValue = 452f;
            if (_dryValue > -10000f) _dryValue -= 10000f / (TransitionTime * 10);
            else _dryValue = -10000f;
            if (_wetValue < 0) _wetValue += 10000f / TransitionTime;
            else _wetValue = 0;
            audioMixer.SetFloat("Cutoff", _cutoffValue);
            audioMixer.SetFloat("DryVol", _dryValue);
            audioMixer.SetFloat("WetVol", _wetValue);
        }
        else
        {
            if (_cutoffValue < 22000f) _cutoffValue += (22000f - 452f) / (TransitionTime * 2);
            else _cutoffValue = 22000f;
            if (_dryValue < 0) _dryValue += 10000f / (TransitionTime * 2);
            else _dryValue = 0;
            if (_wetValue > -10000f) _wetValue -= 10000f / (TransitionTime * 20);
            else _wetValue = -10000f;
            audioMixer.SetFloat("Cutoff", _cutoffValue);
            audioMixer.SetFloat("DryVol", _dryValue);
            audioMixer.SetFloat("WetVol", _wetValue);
        }
    }
}
