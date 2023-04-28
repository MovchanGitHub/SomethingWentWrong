using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class MainButtons : MonoBehaviour
{
    private MainMenuScript mainMenu;
    private GameObject settings;
    public GameObject aboutGame;
    private Button[] buttons;
    public Animator backAnimator;
    
    private GameObject loadingScreen;
    private Slider progressSlider;
    private TextMeshProUGUI progressText;
     
    private void Start() {
        mainMenu = GetComponentInParent<MainMenuScript>();
        settings = GM.UI.SettingsMenu;
        buttons = GetComponentsInChildren<Button>();
        loadingScreen = GM.UI.LoadingScreen;
        progressSlider = loadingScreen.GetComponentInChildren<Slider>();
        progressText = loadingScreen.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void OnButtonLoadScene(string sceneName) {
        Debug.Log($"Loading scene {sceneName}");
        StartCoroutine(LoadAsync(sceneName));
    }

   
    
    public void OnContinueButton() {
        StartCoroutine(LoadAsync("Level One"));

    }
    
    public void OnButtonExit() {
        Debug.Log("Quit application");
        Application.Quit();
    }

    public void OnButtonSettings() {
        foreach (var button in buttons)
        {
            button.animator.Update(1);
        }
        
        settings.SetActive(true);
        mainMenu.ShowHideMenu();
    }
    public void OnButtonAboutGame() {
        foreach (var button in buttons)
        {
            button.animator.Update(1);
        }

        aboutGame.SetActive(true);
        mainMenu.ShowHideMenu();
    }
    public void OnButtonAboutGameBack() {
        aboutGame.SetActive(false);
        mainMenu.ShowHideMenu();
    }

    public void OnButtonBack() {
        backAnimator.Update(1);
        settings.SetActive(false);
        GM.UI.GetComponent<SettingsScript>().SaveSettings();
        mainMenu.ShowHideMenu();
    }
    
    IEnumerator LoadAsync(string sceneName) {
        loadingScreen.SetActive(true);
        
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        var progress = 0f;
        while (!asyncOperation.isDone) {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time. deltaTime);
            progressSlider.value = progress;
            progressText.text = (int)(progress * 100) + "%";
            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                progressText.text =  "100%";
                yield return new WaitForSecondsRealtime(1);
                asyncOperation. allowSceneActivation = true;
            }
            yield return null;
        }
    }
    
    
}