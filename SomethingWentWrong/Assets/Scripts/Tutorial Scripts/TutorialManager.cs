using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject _triggers;
    [SerializeField] private TutorialPopupSystem _tutorialPopupSystem;
    [SerializeField] private GameObject _tutorialEnvironment;
    [SerializeField] private GameObject _tutorialEnvironment2;
    [SerializeField] private GameObject _counter;

    public bool bombExplained;
    public bool starExplained;
    public bool laserExplained;

    [SerializeField] private GameObject[] weaponPopups;

    private int resourcesToMain = 8;

    private int mainedResources = 0;

    public int MainedResources
    {
        get => mainedResources;
        set
        {
            mainedResources = value;
            _counterText.text = "добыто ресурсов: " + mainedResources + "/" + resourcesToMain;
            if (resourcesToMain == 8)
            {
                if (mainedResources == resourcesToMain / 2)
                    _tutorialPopupSystem.OnHalfResourcesMined();
                else if (mainedResources == resourcesToMain)
                    _tutorialPopupSystem.OnAllResourcesMined();
            }
            else if (resourcesToMain == 6)
            {
                if (mainedResources == resourcesToMain)
                    _tutorialPopupSystem.OnAllWeaponableResourcesMined();
            }
        }
    }

    private TextMeshProUGUI _counterText;
    
    private void Start()
    {
        _counterText = _counter.GetComponent<TextMeshProUGUI>();
    }

    public void ShowWeaponableResources()
    {
        _counter.SetActive(true);
        resourcesToMain = 6;
        MainedResources = 0;
        _tutorialEnvironment2.SetActive(true);
    }

    public GameObject Triggers => _triggers;
    public TutorialPopupSystem PopupSystem => _tutorialPopupSystem;
    public GameObject Environment => _tutorialEnvironment;
    public GameObject Counter => _counter;

    public void ExplainWeapon(int ind)
    {
        if (ind == 0) return;
        
        if (ind == 1 && !bombExplained) bombExplained = true;
        else if (ind == 2 && !starExplained) starExplained = true;
        else if (ind == 3 && !laserExplained) laserExplained = true;
        else return;
        
        
        weaponPopups[0].SetActive(false);
        weaponPopups[1].SetActive(false);
        weaponPopups[2].SetActive(false);
        weaponPopups[3].SetActive(false);
        weaponPopups[ind].SetActive(true);
    }
}
