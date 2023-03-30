using UnityEngine;
using static GameManager;

public class SurvivalWarning : MonoBehaviour
{
    public AudioSource _warningSource;
    private float _hunger;
    private float _thirst;
    private float _anoxaemia;
    private bool _condition;
    private bool _IsPaused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _hunger = GM.SurvivalManager.HungerPercent;
        _thirst = GM.SurvivalManager.ThirstPercent;
        _anoxaemia = GM.SurvivalManager.AnoxaemiaPercent;
        _condition = _hunger < 0.15 || _thirst < 0.15 || _anoxaemia < 0.15;
        _IsPaused = (GM.UI.GetComponent<SettingsScript>().isOpened || GM.UI.GetComponent<EndScreen>().isOpened || GM.UI.GetComponent<InGameMenuScript>().isPaused);
        if (_condition && !_IsPaused)
        {
            _warningSource.loop = true;
            if (!_warningSource.isPlaying)
            {
                _warningSource.Play();
            }
        }
        else
        {
            _warningSource.Stop();
        }
    }
}
