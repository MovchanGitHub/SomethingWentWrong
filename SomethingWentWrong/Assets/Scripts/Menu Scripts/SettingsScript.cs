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
    private TMPro.TMP_Dropdown resolutionDropdown;
    private Slider musicSlider;
    private Slider soundsSlider;
    private Toggle fullscreenToggle;
    private Toggle retroWaveToggle;
    
    
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
        resolutionDropdown = settingsMenu.GetComponentInChildren<TMPro.TMP_Dropdown>();
        musicSlider = GM.UI.SettingsMenu.transform.GetChild(2).GetComponent<Slider>();
        soundsSlider = GM.UI.SettingsMenu.transform.GetChild(3).GetComponent<Slider>();
        fullscreenToggle = GM.UI.SettingsMenu.transform.GetChild(4).GetComponent<Toggle>();
        retroWaveToggle = GM.UI.SettingsMenu.transform.GetChild(8).GetComponent<Toggle>();
        
        baseWidth = Screen.width;
        baseHeight = Screen.height;
        var defaultResolutionPath = Application.persistentDataPath + "/default_resolution.gamesave";
        if (!File.Exists(defaultResolutionPath)) {
            var bf = new BinaryFormatter();
            var fs = new FileStream(defaultResolutionPath, FileMode.Create);
        
            var save = new SaveDefaultResolution(
                baseWidth,
                baseHeight
            );
            
            bf.Serialize(fs, save);
            fs.Close();
        }
        else {
            var bf = new BinaryFormatter();
            var fs = new FileStream(defaultResolutionPath, FileMode.Open);
            var save = (SaveDefaultResolution)bf.Deserialize(fs);
            baseWidth = save.width;
            baseHeight = save.height;
            fs.Close();
        }
            

        filePath = Application.persistentDataPath + "/save.gamesave";
        LoadSettings();
        settingsMenu.SetActive(false);
    }
    
    public void HideSettings()
    {
        SaveSettings();
        settingsMenu.SetActive(false);
        pauseScript.ShowHideMenu();
        isOpened = false;
    }
    public void ShowSettings()
    {
        isOpened = true;
        settingsMenu.SetActive(true);
        LoadSettings(); // костыль, зато какой!!!
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
        var options = resolutionDropdown.options;
        var match = Regex.Match(options[value].text, @"(\d+)X(\d+)");
        Screen.SetResolution(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), Screen.fullScreen);
    }

    private bool fullscreen = true;
    
    public bool Fullscreen {
        set => Screen.fullScreen = fullscreen = value;
    }

    public void ChangeFullscreen()
    {
        Fullscreen = !fullscreen;
        SaveSettings();
    }


    public void SaveSettings()
    {
        var music =  GM.UI.SettingsMenu.transform.GetChild(2).GetComponent<Slider>().value;
        var sounds = GM.UI.SettingsMenu.transform.GetChild(3).GetComponent<Slider>().value;
        
        var bf = new BinaryFormatter();
        var fs = new FileStream(filePath, FileMode.Create);
        
        var save = new Save(
            music, 
            sounds,
            resolutionVariant, 
            fullscreen,
            retroWaveToggle.isOn
            );
        Debug.Log(save.ToString());
        bf.Serialize(fs, save);
        fs.Close();
        Debug.Log("Settings saved");
    }

    public void LoadSettings()
    {
        if (!File.Exists(filePath))
            return;
        var bf = new BinaryFormatter();
        var fs = new FileStream(filePath, FileMode.Open);
        var save = (Save)bf.Deserialize(fs);
        fs.Close();
        
        var musicVolume = save.musicVolume;
        var soundsVolume = save.soundsVolume;
        var saveResolutionVariant = save.resolutionVariant;
        var isFullScreen = save.isFullscreen;
        var isRetroWave = save.isRetroWave;
        
        var musicVolumeScript = GM.UI.SettingsMenu.transform.GetChild(2).GetComponent<MusicVol>().audioMixer;
        var soundsVolumeScript = GM.UI.SettingsMenu.transform.GetChild(3).GetComponent<SoundsVol>().audioMixer;
        var retroWaveScript = GM.UI.SettingsMenu.transform.GetChild(8).GetComponent<SetRetroWaveEffect>();
        
        // Music & Sounds
        musicSlider.value = musicVolume;
        // musicVolumeScript.SetFloat("GameVol", Mathf.Log10(musicVolume) * 20);
        soundsSlider.value = soundsVolume;
        // soundsVolumeScript.SetFloat("GameVol", Mathf.Log10(soundsVolume) * 20);
        // Resolution
        resolutionDropdown.value = saveResolutionVariant;
        resolutionDropdown.RefreshShownValue();
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

    public override string ToString()
    {
        return $"resolutionVariant{resolutionVariant}\nisFullscreen {isFullscreen}\nisRetroWave {isRetroWave}\nmusicVolume {musicVolume}\nsoundsVolume {soundsVolume}\n";
    }
}

[System.Serializable]
public class SaveDefaultResolution {
    public int width;
    public int height;
    
    public SaveDefaultResolution(int width, int height) {
        this.width = width;
        this.height = height;
    }
}

