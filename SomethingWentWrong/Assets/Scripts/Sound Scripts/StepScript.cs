using System.Collections.Generic;
using UnityEngine;

public class StepScript : MonoBehaviour
{
    private string _sprite;
    private AudioSource Settings;
    [SerializeField] private List<AudioClip> _steps;
    private int ind;
    private string _previousSprite;
    private GameObject _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        Settings = GetComponent<AudioSource>();
        ind = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _sprite = GetComponent<SpriteRenderer>().sprite.name;
        if (_sprite[0..13] == "PlayerWalking" && (_sprite[^1] == '1' || _sprite[^1] == '3') && _sprite[^1] != _previousSprite[^1] && !_player.GetComponent<IsometricPlayerMovementController>().isRushing)
        {
            Settings.clip = _steps[ind]; 
            Settings.Play();
            // Settings.PlayOneShot(_steps[ind]);
            ind++;
            if (ind == 6)
                ind = 0;
        }

        _previousSprite = _sprite;
    }
}
