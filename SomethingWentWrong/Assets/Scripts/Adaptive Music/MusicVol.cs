using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class MusicVol : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Start()
    {
        audioMixer.SetFloat("GameVol", GetComponent<Slider>().value);
    }

    public void OnChangeSlider(float value)
    {
        audioMixer.SetFloat("GameVol", Mathf.Log10(value) * 20);
    }
}