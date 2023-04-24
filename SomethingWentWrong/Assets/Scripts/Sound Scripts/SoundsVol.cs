using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using static GameManager;
using Slider = UnityEngine.UI.Slider;

public class SoundsVol : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    private void Start()
    {
        audioMixer.SetFloat("GameVol", GM.UI.SettingsMenu.transform.GetChild(3).GetComponent<Slider>().value);
    }

    public void OnChangeSlider(float value)
    {
        audioMixer.SetFloat("GameVol", Mathf.Log10(value) * 20);
    }
}
