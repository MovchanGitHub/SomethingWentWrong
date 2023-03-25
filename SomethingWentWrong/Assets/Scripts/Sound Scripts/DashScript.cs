using UnityEngine;
using static GameManager;

public class DashScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip Clip;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = Clip;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GM.SurvivalManager.CanRush() && GM.InputSystem.playerInput.actions["Dash"].triggered) // && GetComponent<IsometricPlayerMovementController>().isRushing)
        {
            DashSound();
        }
    }

    private void DashSound()
    {
        _audioSource.Play();
    }
}
