using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using static GameManager;

public class SettingsScript : MonoBehaviour
{

    GameObject settingsMenu;
    InGameMenuScript pauseScript;
    
    public bool isOpened;
    
    public TMPro.TMP_Dropdown dropdown;

    private string filePath;
    private int baseWidth;
    private int baseHeight;
    private int resolutionVariant;
    
    
    private void Awake()
    {
        pauseScript = GetComponent<InGameMenuScript>();
        LoadSettings();
    }

    private void Start()
    {
        baseWidth = Screen.width;
        baseHeight = Screen.height;
        settingsMenu = GM.UI.SettingsMenu;
        
        // GM.UI.SettingsMenu.transform.GetChild(4).GetComponent<Toggle>().isOn = Screen.fullScreen;
        settingsMenu.SetActive(false);
        dropdown = GM.UI.SettingsMenu.GetComponentInChildren<TMPro.TMP_Dropdown>();

        filePath = Application.persistentDataPath + "/save.gamesave";
        // Debug.Log(filePath); // uncomment to show save path
        LoadSettings();
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
        LoadSettings();
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

    public void SetFullscreen(bool is_fullscreen)
    {
        Screen.SetResolution(Screen.width, Screen.height, is_fullscreen);
        Debug.Log($"Is full screen - {is_fullscreen}");
        SaveSettings();
    }


    public void SaveSettings()
    {
        Debug.Log("Saveing settings");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);

        // var GM.UI.SettingsMenu.transform.GetChild(2).GetComponent<Slider>().value;
        // GM.UI.SettingsMenu.transform.GetChild(3).GetComponent<Slider>().value;
        Save save = new Save(resolutionVariant, Screen.fullScreen, 100, 100);
        bf.Serialize(fs, save);
        fs.Close();
    }

    public void LoadSettings()
    {
        if (!File.Exists(filePath))
            return;
        Debug.Log("Loading settings");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open);

        Save save = (Save)bf.Deserialize(fs);
        fs.Close();
        
        Debug.Log(save.resolutionVariant);
        Debug.Log(save.isFullscreen);
        Debug.Log(save.musicVolume);
        Debug.Log(save.soundsVolume);
        GM.UI.SettingsMenu.transform.GetChild(2).GetComponent<Slider>().value = save.musicVolume;
        GM.UI.SettingsMenu.transform.GetChild(3).GetComponent<Slider>().value = save.soundsVolume;
        GM.UI.SettingsMenu.transform.GetChild(4).GetComponent<Toggle>().isOn = save.isFullscreen;
        GM.UI.SettingsMenu.transform.GetChild(6).GetComponent<TMPro.TMP_Dropdown>().value = save.resolutionVariant;
        GM.UI.SettingsMenu.transform.GetChild(6).GetComponent<TMPro.TMP_Dropdown>().RefreshShownValue();
        SetResolution(save.resolutionVariant);
        SetFullscreen(save.isFullscreen);
    }
}

[System.Serializable]
public class Save
{
    public int resolutionVariant;
    public bool isFullscreen;
    public float musicVolume;
    public float soundsVolume;
    

    public Save(int resolutionVariant, bool isFullscreen, float musicVolume, float soundsVolume)
    {
        this.resolutionVariant = resolutionVariant;
        this.isFullscreen = isFullscreen;
        this.musicVolume = musicVolume;
        this.soundsVolume = soundsVolume;
    }
}