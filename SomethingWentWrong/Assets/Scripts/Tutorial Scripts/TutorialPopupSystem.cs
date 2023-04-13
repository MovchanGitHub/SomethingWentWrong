using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class TutorialPopupSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] popups;

    private int greetingPopup = 0;
    private int rocketTrackerPopup = 1;
    private int runningButtonPopup = 2;
    private int baseButtonPopup = 3;
    private int basicInfoPopup = 10;
    
    
    public void OnGreetingButtonClick()
    {
        popups[greetingPopup].SetActive(false);
        popups[rocketTrackerPopup].SetActive(true);
    }
    
    
    public void OnBasicInfoButtonClick()
    {
        popups[basicInfoPopup].SetActive(false);
    }
    
    
    public void OnRocketTrackerButtonClick()
    {
        popups[rocketTrackerPopup].SetActive(false);
        GM.UI.Pointer.SetActive(true);
    }
    
    
    public void OnRunningButtonClick()
    {
        popups[runningButtonPopup].SetActive(false);
    }
    
    
    public void OnBaseButtonClick()
    {
        popups[baseButtonPopup].SetActive(false);
    }
}
