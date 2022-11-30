using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicVol : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void OnChangeSlider(float value)
    {
        audioMixer.SetFloat("GameVol", value);
    }
}
