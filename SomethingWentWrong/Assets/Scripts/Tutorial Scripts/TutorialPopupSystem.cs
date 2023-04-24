using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class TutorialPopupSystem : MonoBehaviour
{
    public GameObject[] popups;

    private int greetingPopup = 0;
    private int walkingPopup = 1;
    private int rocketTrackerPopup = 2;
    private int runningPopup = 3;
    private int basePopup = 4;
    private int collectResourcesPopup = 5;
    private int dashPopup = 6;
    private int inventoryUsePopup = 7;
    private int inventoryErasePopup = 8;
    private int weaponablePopup = 9;
    private int bombPopup = 10;
    private int starPopup = 11;
    private int laserPopup = 12;
    private int starDetailedPopup = 13;
    private int laserDetailedPopup = 14;
    private int bombDetailedPopup = 15;
    public int bombDeathPopup1 = 16;
    private int bombDeathPopup2 = 17;

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

    public GameObject CurrentPopupObject
    {
        get => popups[currPopup];
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
    
    
    // public void OnBasicInfoButtonClick()
    // {
    //     popups[basicInfoPopup].SetActive(false);
    // }
    
    
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


    public void OnAllResourcesMined()
    {
        GM.Tutorial.Counter.SetActive(false);
        StartCoroutine(HideFirstWaitShowSecond(dashPopup, inventoryUsePopup, 2 * timeBeforeShowNewPopup));
        StartCoroutine(ShowInventoryErasePopup());
    }
    

    public void OnAllWeaponableResourcesMined()
    {
        GM.Tutorial.Counter.SetActive(false);
        popups[laserPopup].SetActive(false);
        popups[starPopup].SetActive(false);
        popups[bombPopup].SetActive(false);
        popups[starDetailedPopup].SetActive(true);
        GM.Tutorial.weaponInfinityPlants[0].SetActive(true);
        StartCoroutine(SpawnEnemies1());
    }


    private IEnumerator SpawnEnemies1()
    {
        yield return new WaitForSeconds(20f);
        GM.Tutorial.spawnedEnemies[0].SetActive(true);
        GM.Tutorial.Counter.SetActive(true);
        GM.Tutorial.KilledEnemies = 0;
    }
    

    public IEnumerator SpawnEnemies2()
    {
        popups[starDetailedPopup].SetActive(false);
        popups[laserDetailedPopup].SetActive(true);
        GM.Tutorial.weaponInfinityPlants[0].SetActive(false);
        GM.Tutorial.weaponInfinityPlants[1].SetActive(true);
        yield return new WaitForSeconds(20f);
        GM.Tutorial.spawnedEnemies[1].SetActive(true);
        GM.Tutorial.Counter.SetActive(true);
        GM.Tutorial.KilledEnemies = 0;
        GM.Tutorial.enemiesToKill = 2;
    }

    
    public IEnumerator SpawnEnemies3()
    {
        popups[laserDetailedPopup].SetActive(false);
        popups[bombDetailedPopup].SetActive(true);
        GM.Tutorial.weaponInfinityPlants[1].SetActive(false);
        GM.Tutorial.weaponInfinityPlants[2].SetActive(true);
        yield return new WaitForSeconds(20f);
        GM.Tutorial.spawnedEnemies[2].SetActive(true);
        GM.Tutorial.Counter.SetActive(true);
        GM.Tutorial.KilledEnemies = 0;
        GM.Tutorial.enemiesToKill = 4;
    }

    
    public IEnumerator OnAllEnemiesKilled()
    {
        yield return new WaitForSeconds(1f);
        GM.Tutorial.OnLearnedWeapon();
    }


    private IEnumerator ShowInventoryErasePopup()
    {
        yield return new WaitForSeconds(10f);
        StartCoroutine(HideFirstWaitShowSecond(inventoryUsePopup, inventoryErasePopup, 2 * timeBeforeShowNewPopup));
        yield return new WaitForSeconds(18f);
        StartCoroutine(HideFirstWaitShowSecond(inventoryErasePopup, weaponablePopup, 2 * timeBeforeShowNewPopup));
        yield return new WaitForSeconds(2 * timeBeforeShowNewPopup);
        GM.Tutorial.ShowWeaponableResources();
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


    public void OnInventoryEraseButtonClick()
    {
        popups[inventoryErasePopup].SetActive(false);
        
    }
    
    
    public void OnWeaponableButtonClick()
    {
        popups[weaponablePopup].SetActive(false);
    }


    public void OnLaserButtonClick()
    {
        popups[laserPopup].SetActive(false);
    }
    

    public void OnStarButtonClick()
    {
        popups[starPopup].SetActive(false);
    }
    

    public void OnBombButtonClick()
    {
        popups[bombPopup].SetActive(false);
    }
    

    public void OnBombDetailedButtonClick()
    {
        popups[bombDetailedPopup].SetActive(false);
    }
    

    public void OnStarDetailedButtonClick()
    {
        popups[starDetailedPopup].SetActive(false);
    }
    

    public void OnLaserDetailedButtonClick()
    {
        popups[laserDetailedPopup].SetActive(false);
    }

    public void OnBombDeathAcceptButtonClick()
    {
        popups[bombDeathPopup1].SetActive(false);
        popups[bombDeathPopup2].SetActive(false);
        GM.Tutorial.LastTextAppearence();
    }
    
    
    public void OnBombDeathDeclineButtonClick()
    {
        StartCoroutine(HideFirstWaitShowSecond(bombDeathPopup1, bombDeathPopup2, timeBeforeShowNewPopup / 2));
    }
}
