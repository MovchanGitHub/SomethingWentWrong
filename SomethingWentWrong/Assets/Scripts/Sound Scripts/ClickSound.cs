using UnityEngine;

public class ClickSound : MonoBehaviour
{
    private AudioSource _clickSource;
    public AudioClip _clickSound;
    
    public void Click()
    {
        _clickSource.Play();
    }

    void Start()
    {
        _clickSource.clip = _clickSound;
        _clickSource.volume = 1;
    }
}
