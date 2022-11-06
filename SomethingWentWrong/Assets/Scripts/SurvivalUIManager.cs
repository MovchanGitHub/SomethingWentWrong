using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalUIManager : MonoBehaviour
{
    [SerializeField] private SurvivalManager _survivalManager;
    [SerializeField] private Image _hungerMeter, _thirstMeter, _staminaMeter, _anoxaemiaMeter;

    private void FixedUpdate()
    {
        _hungerMeter.fillAmount = _survivalManager.HungerPercent;
        _thirstMeter.fillAmount = _survivalManager.ThirstPercent;
        _staminaMeter.fillAmount = _survivalManager.StaminaPercent;
        _anoxaemiaMeter.fillAmount = _survivalManager.AnoxaemiaPercent;
    }
}
