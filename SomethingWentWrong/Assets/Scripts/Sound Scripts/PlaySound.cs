using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlaySound : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioMixerGroup audioMixer;
    public AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        StartCoroutine(PlayCor());
    }

    private IEnumerator PlayCor()
    {
        audioSource.clip = audioClip;
        audioSource.outputAudioMixerGroup = audioMixer;
        if (!audioClip.name.Contains("Click"))
            audioSource.pitch = 1 + UnityEngine.Random.Range(-0.15f, 0.15f);
        audioSource.Play();
        yield return new WaitForSecondsRealtime(audioClip.length);
        Destroy(gameObject);
    }
}
