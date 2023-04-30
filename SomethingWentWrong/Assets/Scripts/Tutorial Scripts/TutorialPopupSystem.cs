using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    
    //extra popups
    private int survivalBarPopup = 18;
    private int hpBarPopup = 19;
    private int weaponBarPopup = 20;

    private int currPopup = 0;

    public int CurrPopup
    {
        get => currPopup;
        set
        {
            currPopup = value; 
            //Debug.Log("currPopup = " + currPopup);
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
        StartCoroutine(ShowHPBar());
    }
    
    private IEnumerator ShowHPBar()
    {
        StartCoroutine(HideFirstWaitShowSecond(basePopup, hpBarPopup, timeBeforeShowNewPopup));
        yield return new WaitForSeconds(timeBeforeShowNewPopup);
        GM.Tutorial.hpBar.SetActive(true);
    }
    
    public void OnWeaponBarPopupButtonClicked()
    {
        StartCoroutine(CloseWeaponBarPopup());
    }

    private IEnumerator CloseWeaponBarPopup()
    {
        StartCoroutine(HideFirstWaitShowSecond(weaponBarPopup, weaponablePopup, 2 * timeBeforeShowNewPopup));
        yield return new WaitForSeconds(2 * timeBeforeShowNewPopup);
        GM.Tutorial.ShowWeaponableResources();
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
        StartCoroutine(ShowSurvivalBar());
    }


    private IEnumerator ShowSurvivalBar()
    {
        StartCoroutine(HideFirstWaitShowSecond(dashPopup, survivalBarPopup, timeBeforeShowNewPopup));
        yield return new WaitForSeconds(timeBeforeShowNewPopup);
        GM.Tutorial.surivalBar.SetActive(true);
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
        yield return new WaitForSeconds(1f);
        GM.InventoryManager.clearInventory();
        yield return new WaitForSeconds(15f);
        GM.Tutorial.spawnedEnemies[0].SetActive(true);
        GM.Tutorial.Counter.SetActive(true);
        GM.Tutorial.KilledEnemies = 0;
        GM.Tutorial.enemiesToKill = 2;
    }
    

    public IEnumerator SpawnEnemies2()
    {
        yield return new WaitForSeconds(1f);
        GM.InventoryManager.clearInventory();
        popups[starDetailedPopup].SetActive(false);
        popups[laserDetailedPopup].SetActive(true);
        GM.Tutorial.weaponInfinityPlants[0].SetActive(false);
        GM.Tutorial.weaponInfinityPlants[1].SetActive(true);
        yield return new WaitForSeconds(15f);
        GM.Tutorial.spawnedEnemies[1].SetActive(true);
        GM.Tutorial.Counter.SetActive(true);
        GM.Tutorial.KilledEnemies = 0;
        GM.Tutorial.enemiesToKill = 2;
    }

    
    public IEnumerator SpawnEnemies3()
    {
        yield return new WaitForSeconds(1f);
        GM.InventoryManager.clearInventory();
        popups[laserDetailedPopup].SetActive(false);
        popups[bombDetailedPopup].SetActive(true);
        GM.Tutorial.weaponInfinityPlants[1].SetActive(false);
        GM.Tutorial.weaponInfinityPlants[2].SetActive(true);
        yield return new WaitForSeconds(15f);
        GM.Tutorial.spawnedEnemies[2].SetActive(true);
        GM.Tutorial.Counter.SetActive(true);
        GM.Tutorial.enemiesToKill = 3;
        GM.Tutorial.KilledEnemies = 0;
    }

    
    public IEnumerator OnAllEnemiesKilled()
    {
        yield return new WaitForSeconds(1f);
        GM.Tutorial.OnLearnedWeapon();
    }

    private void InventoryInsertSpaceTrash()
    {
        GM.InventoryManager.clearInventory();
        GM.InventoryManager.insertItem(GM.Tutorial.spaceTrash[0]);
        GM.InventoryManager.insertItem(GM.Tutorial.spaceTrash[1]);
        GM.InventoryManager.insertItem(GM.Tutorial.spaceTrash[0]);
        GM.InventoryManager.insertItem(GM.Tutorial.spaceTrash[0]);
        GM.Tutorial.checkForErasing = true;
    }


    private IEnumerator ShowInventoryUsePopup()
    {
        StartCoroutine(HideFirstWaitShowSecond(survivalBarPopup, inventoryUsePopup, 2 * timeBeforeShowNewPopup));
        yield return new WaitForSeconds(2 * timeBeforeShowNewPopup);
        Debug.Log("разрешить открывать инвентарь на Tab");
        GM.Tutorial.checkForEating = true;
        GM.Tutorial.CheckForEatingActive();
    }

    public IEnumerator HideInventoryUsePopup()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(HideFirstWaitShowSecond(inventoryUsePopup, inventoryErasePopup, 2 * timeBeforeShowNewPopup));
        yield return new WaitForSeconds(2 * timeBeforeShowNewPopup);
        GM.Tutorial.CheckForErasingActive();
        InventoryInsertSpaceTrash();
    }
    
    public IEnumerator HideInventoryErasePopup()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(HideFirstWaitShowSecond(inventoryErasePopup, weaponBarPopup, 2 * timeBeforeShowNewPopup));
        yield return new WaitForSeconds(2 * timeBeforeShowNewPopup);
        GM.Tutorial.surivalBar.SetActive(false);
        GM.Tutorial.weaponBar.SetActive(true);
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

    
    public void OnSurvivalBarButtonClick()
    {
        StartCoroutine(ShowInventoryUsePopup());
    }
    
    
    public void OnHPBarButtonClick()
    {
        StartCoroutine(HideFirstWaitShowSecond(hpBarPopup, collectResourcesPopup, timeBeforeShowNewPopup));
        StartCoroutine(EnableResources());
    }
}
