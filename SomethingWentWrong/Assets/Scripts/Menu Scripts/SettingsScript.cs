using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using static GameManager;

public class SettingsScript : MonoBehaviour
{
    [HideInInspector] public bool isOpened;

    private GameObject settingsMenu;
    private InGameMenuScript pauseScript;
    private TMPro.TMP_Dropdown dropdown;
    
    private string filePath;
    private int baseWidth;
    private int baseHeight;
    private int resolutionVariant;
    
    
    private void Awake()
    {
    }

    private void Start()
    {
        pauseScript = GM.UI.InGameMenuScript;
        settingsMenu = GM.UI.SettingsMenu;
        dropdown = settingsMenu.GetComponentInChildren<TMPro.TMP_Dropdown>();
        
        baseWidth = Screen.width;
        baseHeight = Screen.height;
        
        filePath = Application.persistentDataPath + "/save.gamesave";
        LoadSettings();
        settingsMenu.SetActive(false);
    }
    
    public void HideSettings()
    {
        settingsMenu.SetActive(false);
        pauseScript.ShowHideMenu();
        isOpened = false;
        SaveSettings();
    }
    public void ShowSettings()
    {
        isOpened = true;
        settingsMenu.SetActive(true);
        pauseScript.ShowHideMenu();
    }

    public void SetResolution(int value)
    {
        resolutionVariant = value;
        if (value == 0)
        {
            Screen.SetResolution(baseWidth, baseHeight, Screen.fullScreen);
            return;
        }
        var options = dropdown.options;
        var match = Regex.Match(options[value].text, @"(\d+)X(\d+)");
        Screen.SetResolution(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), Screen.fullScreen);
        SaveSettings();
    }

    private bool fullscreen = true;
    
    public bool Fullscreen {
        set => Screen.fullScreen = fullscreen = value;
    }

    public void ChangeFullscreen()
    {
        Fullscreen = !fullscreen;
    }


    public void SaveSettings()
    {
        var music =  GM.UI.SettingsMenu.transform.GetChild(2).GetComponent<Slider>().value;
        var sounds = GM.UI.SettingsMenu.transform.GetChild(3).GetComponent<Slider>().value;
        var isRetroWave = GM.UI.SettingsMenu.transform.GetChild(8).GetComponent<Toggle>().isOn;
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);
        
        Save save = new Save(
            music, 
            sounds,
            resolutionVariant, 
            fullscreen,
            isRetroWave
            );
        bf.Serialize(fs, save);
        fs.Close();
        Debug.Log("Settings saved");
    }

    public void LoadSettings()
    {
        if (!File.Exists(filePath))
            return;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open);
        Save save = (Save)bf.Deserialize(fs);
        fs.Close();
        
        var musicVolume = save.musicVolume;
        var soundsVolume = save.soundsVolume;
        var saveResolutionVariant = save.resolutionVariant;
        var isFullScreen = save.isFullscreen;
        var isRetroWave = save.isRetroWave;
        
        var musicSlider = GM.UI.SettingsMenu.transform.GetChild(2).GetComponent<Slider>();
        var musicVolumeScript = GM.UI.SettingsMenu.transform.GetChild(2).GetComponent<MusicVol>().audioMixer;
        var soundsSlider = GM.UI.SettingsMenu.transform.GetChild(3).GetComponent<Slider>();
        var soundsVolumeScript = GM.UI.SettingsMenu.transform.GetChild(3).GetComponent<SoundsVol>().audioMixer;
        var resolutionDropDown = GM.UI.SettingsMenu.transform.GetChild(6).GetComponent<TMPro.TMP_Dropdown>();
        var fullscreenToggle = GM.UI.SettingsMenu.transform.GetChild(4).GetComponent<Toggle>();
        var retroWaveToggle = GM.UI.SettingsMenu.transform.GetChild(8).GetComponent<Toggle>();
        var retroWaveScript = GM.UI.SettingsMenu.transform.GetChild(8).GetComponent<SetRetroWaveEffect>();
        
        // Music & Sounds
        musicSlider.value = musicVolume;
        musicVolumeScript.SetFloat("GameVol", Mathf.Log10(musicVolume) * 20);
        soundsSlider.value = soundsVolume;
        soundsVolumeScript.SetFloat("GameVol", Mathf.Log10(soundsVolume) * 20);
        // Resolution
        resolutionDropDown.value = saveResolutionVariant;
        resolutionDropDown.RefreshShownValue();
        SetResolution(saveResolutionVariant);
        // Fullscreen
        fullscreenToggle.isOn = isFullScreen;
        Screen.SetResolution(Screen.width, Screen.height, isFullScreen);
        // RetroWave
        retroWaveToggle.isOn = isRetroWave;
        retroWaveScript.RetroWaveEffect = isRetroWave;
        Debug.Log("Loading settings");
    }
}

[System.Serializable]
public class Save
{
    public int resolutionVariant;
    public bool isFullscreen;
    public bool isRetroWave;
    public float musicVolume;
    public float soundsVolume;
    
    public Save(
        float musicVolume, 
        float soundsVolume,
        int resolutionVariant, 
        bool isFullscreen, 
        bool isRetroWave
        )
    {
        this.musicVolume = musicVolume;
        this.soundsVolume = soundsVolume;
        this.resolutionVariant = resolutionVariant;
        this.isFullscreen = isFullscreen;
        this.isRetroWave = isRetroWave;
    }
}