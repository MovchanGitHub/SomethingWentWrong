using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class TutorialMusic : MusicFaderScript
{
    private AudioSource _audioSource;
    private Scene _scene;
    
    //private const int TransTime = 100;
    
    private float _timeToFade;
    private float _timeElapsed;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
        _timeToFade = 3;
        DontDestroyOnLoad(gameObject);
        _scene = SceneManager.GetActiveScene();
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (_scene == SceneManager.GetActiveScene())
        {
            if (_audioSource.volume < 1)
            {
                _audioSource.volume = Mathf.Pow(Mathf.Lerp(0, 1, _timeElapsed / _timeToFade), 2);
                _timeElapsed += Time.deltaTime;
            }
        }
        else
        {
            _audioSource.volume = Mathf.Pow(Mathf.Lerp(0, 1, _timeElapsed / _timeToFade), 2); 
            _timeElapsed -= Time.deltaTime;
            if (_timeElapsed < 0)
                _timeElapsed = 0;
            if (_audioSource.volume < 0.001f)
            {
                Destroy(gameObject);
            } 
        }
    }
}
