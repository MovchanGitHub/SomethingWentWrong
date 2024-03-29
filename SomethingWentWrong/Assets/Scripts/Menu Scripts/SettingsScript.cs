using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
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
        filePath = Application.persistentDataPath + "/settings.gamesave";
        var defaultResolutionPath = Application.persistentDataPath + "/default_resolution.gamesave";
        baseWidth = Screen.width;
        baseHeight = Screen.height;
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
        
        LoadSettings();
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
        pauseScript.ShowHideMenu();
    }

    public void SetResolution(int value)
    {
        resolutionVariant = value;
        if (value == 0)
        {
            Screen.SetResolution(baseWidth, baseHeight, Screen.fullScreen);
            if (SceneManager.GetActiveScene().name == "Level One")
            {
                if (baseWidth / baseHeight < 2)
                    GM.UI.Encyclopedia.EncyclopediaScript.aspectRatioFitter.aspectRatio = (float)baseWidth / baseHeight;
                else
                    GM.UI.Encyclopedia.EncyclopediaScript.aspectRatioFitter.aspectRatio = (float)16 / 9;
            }
            return;
        }
        var options = resolutionDropdown.options;
        var match = Regex.Match(options[value].text, @"(\d+)X(\d+)");
        Screen.SetResolution(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), Screen.fullScreen);
        if (SceneManager.GetActiveScene().name == "Level One")
        {
            if (float.Parse(match.Groups[1].Value) / float.Parse(match.Groups[2].Value) < 2)
                GM.UI.Encyclopedia.EncyclopediaScript.aspectRatioFitter.aspectRatio = float.Parse(match.Groups[1].Value) / float.Parse(match.Groups[2].Value);
            else
                GM.UI.Encyclopedia.EncyclopediaScript.aspectRatioFitter.aspectRatio = (float)16 / 9;
        }
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
        
        var bf = new BinaryFormatter();
        var fs = new FileStream(filePath, FileMode.Create);
        
        var save = new Save(
            music, 
            sounds,
            resolutionVariant, 
            fullscreenToggle.isOn,
            retroWaveToggle.isOn
            );
        bf.Serialize(fs, save);
        fs.Close();
    }

    public void LoadSettings()
    {
        if (!File.Exists(filePath)) return;
        var (bf, fs) = (new BinaryFormatter(), new FileStream(filePath, FileMode.Open)) ;
        var save = (Save)bf.Deserialize(fs);
        fs.Close();
        
        // Music & Sounds
        musicSlider.value = save.musicVolume;
        soundsSlider.value = save.soundsVolume;
        
        // Resolution
        var saveResolutionVariant = save.resolutionVariant;
        resolutionDropdown.value = saveResolutionVariant;
        resolutionDropdown.RefreshShownValue();
        SetResolution(saveResolutionVariant);
        
        // Fullscreen
        fullscreenToggle.isOn = save.isFullscreen;
        
        // RetroWave
        retroWaveToggle.isOn = save.isRetroWave;
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

[System.Serializable]
public class SaveDefaultResolution {
    public int width;
    public int height;
    
    public SaveDefaultResolution(int width, int height) {
        this.width = width;
        this.height = height;
    }
}

