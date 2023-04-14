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
    [SerializeField] private GameObject _counter;
    [SerializeField] private GameObject _inventoryUsePopup;
    
    private int resourcesToMain = 8;

    private int mainedResources = 0;

    public int MainedResources
    {
        get => mainedResources;
        set
        {
            mainedResources = value;
            _counterText.text = "добыто ресурсов: " + mainedResources + "/" + resourcesToMain;
            if (mainedResources == resourcesToMain)
            {
                _counter.SetActive(false);
                _inventoryUsePopup.SetActive(true);
            }
        }
    }

    private TextMeshProUGUI _counterText;
    
    private void Start()
    {
        _counterText = _counter.GetComponent<TextMeshProUGUI>();
    }

    public GameObject Triggers => _triggers;
    public TutorialPopupSystem PopupSystem => _tutorialPopupSystem;
    public GameObject Environment => _tutorialEnvironment;
    public GameObject Counter => _counter;
}
