using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class TutorialPopupSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] popups;

    private int greetingPopup = 0;
    private int walkingPopup = 1;
    private int rocketTrackerPopup = 2;
    private int runningPopup = 3;
    private int basePopup = 4;
    private int collectResourcesPopup = 5;
    private int dashPopup = 6;
    private int inventoryUsePopup = 7;
    private int basicInfoPopup = 10;

    private int currPopup = 0;

    public int CurrPopup
    {
        get => currPopup;
        set
        {
            currPopup = value;
            Debug.Log("currPopup = " + currPopup);
        }
    }

    private float timeBeforeShowNewPopup = 1f;

    private IEnumerator HideFirstWaitShowSecond(int first, int second, float time)
    {
        popups[first].SetActive(false);
        yield return new WaitForSeconds(time);
        popups[second].SetActive(true);
    }

    
    public void OnGreetingButtonClick()
    {
        StartCoroutine(HideFirstWaitShowSecond(greetingPopup, walkingPopup, timeBeforeShowNewPopup));
    }
    
    
    public void OnBasicInfoButtonClick()
    {
        popups[basicInfoPopup].SetActive(false);
    }
    
    
    public void OnRocketTrackerButtonClick()
    {
        popups[rocketTrackerPopup].SetActive(false);
        GM.UI.Pointer.SetActive(true);
        Destroy(GM.Tutorial.Triggers.transform.GetChild(0).gameObject);
    }
    
    
    public void OnRunningButtonClick()
    {
        popups[runningPopup].SetActive(false);
    }
    
    
    public void OnBaseButtonClick()
    {
        StartCoroutine(HideFirstWaitShowSecond(basePopup, collectResourcesPopup, timeBeforeShowNewPopup));
        StartCoroutine(EnableResources());
    }


    private IEnumerator EnableResources()
    {
        yield return new WaitForSeconds(timeBeforeShowNewPopup);
        GM.Tutorial.Environment.SetActive(true);
        GM.Tutorial.Counter.SetActive(true);
    }
    
    
    public void OnWalkingButtonClick()
    {
        StartCoroutine(HideFirstWaitShowSecond(walkingPopup, rocketTrackerPopup, 3 * timeBeforeShowNewPopup));
    }
    
    
    public void OnCollectResourcesButtonClick()
    {
        popups[collectResourcesPopup].SetActive(false);
    }
    
    public void OnInventoryUseButtonClick()
    {
        popups[inventoryUsePopup].SetActive(false);
    }


    public void OnAllRecourcesMined()
    {
        GM.Tutorial.Counter.SetActive(false);
        StartCoroutine(HideFirstWaitShowSecond(dashPopup, inventoryUsePopup, 2 * timeBeforeShowNewPopup));
    }
    
    
    public void OnHalfResourcesMined()
    {
        StartCoroutine(HideFirstWaitShowSecond(collectResourcesPopup, dashPopup, 3 * timeBeforeShowNewPopup));
    }
    
    
    public void OnDashButtonClick()
    {
        popups[dashPopup].SetActive(false);
    }
    
    
    public void PopupTrigger(int popupIndex)
    {
        StartCoroutine(HideFirstWaitShowSecond(popupIndex-1, popupIndex, timeBeforeShowNewPopup));
    }
}
