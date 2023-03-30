using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundsVol : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void OnChangeSlider(float value)
    {
        audioMixer.SetFloat("GameVol", Mathf.Log10(value) * 20);
    }
}
