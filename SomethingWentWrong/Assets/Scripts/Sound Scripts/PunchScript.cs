using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using static GameManager;

public class PunchScript : MonoBehaviour
{
    private AudioClip _clip;
    private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _punches;
    private int _ind;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _ind = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PunchSound(ref int ind)
    {
        _audioSource.clip = _punches[_ind];
        _audioSource.Play();
    }
}
