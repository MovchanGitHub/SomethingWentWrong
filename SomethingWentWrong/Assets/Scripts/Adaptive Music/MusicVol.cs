using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using static GameManager;
using Slider = UnityEngine.UI.Slider;

public class MusicVol : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Start()
    {
        audioMixer.SetFloat("GameVol", Mathf.Log10(GM.UI.SettingsMenu.transform.GetChild(2).GetComponent<Slider>().value)  * 20);
    }

    public void OnChangeSlider(float value)
    {
        audioMixer.SetFloat("GameVol", Mathf.Log10(value) * 20);
    }
}