using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepScript : MonoBehaviour
{
    private string _sprite;
    private AudioSource Settings;
    public AudioClip Step1;
    public AudioClip Step2;
    public AudioClip Step3;
    public AudioClip Step4;
    public AudioClip Step5;
    public AudioClip Step6;
    private AudioClip[] _steps;
    private int ind;
    
    // Start is called before the first frame update
    void Start()
    {
        Settings = GetComponent<AudioSource>();
        _steps = new [] { Step1, Step2, Step3, Step4, Step5, Step6 };
        ind = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _sprite = GetComponent<SpriteRenderer>().sprite.name;
        if (_sprite[0..13] == "PlayerWalking" && (_sprite[^1] == '1' || _sprite[^1] == '3'))
        {
            if (Settings.isPlaying)
                Settings.Stop();
            Settings.clip = _steps[ind]; 
            Settings.Play();
            ind++;
            if (ind == 6)
                ind = 0;
        }
    }
}
