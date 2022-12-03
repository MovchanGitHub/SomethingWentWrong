using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalUIManager : MonoBehaviour
{
    //[SerializeField] private SurvivalManager _survivalManager;
    public Image _hungerMeter, _thirstMeter, _staminaMeter, _anoxaemiaMeter;

    private static SurvivalUIManager instance;


    public static SurvivalUIManager Instance
    {
        get { return instance; }

        private set { instance = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        _hungerMeter.fillAmount = SurvivalManager.Instance.HungerPercent;
        _thirstMeter.fillAmount = SurvivalManager.Instance.ThirstPercent;
        _staminaMeter.fillAmount = SurvivalManager.Instance.StaminaPercent;
        _anoxaemiaMeter.fillAmount = SurvivalManager.Instance.AnoxaemiaPercent;
    }
}
