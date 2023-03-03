using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class SurvivalBar : MonoBehaviour
{
    private Image _hungerMeter, _thirstMeter, _staminaMeter, _anoxaemiaMeter;

    private void Start()
    {
        _hungerMeter = transform.GetChild(0).GetComponent<Image>();
        _thirstMeter = transform.GetChild(1).GetComponent<Image>();
        _staminaMeter = transform.GetChild(2).GetComponent<Image>();
        _anoxaemiaMeter = transform.GetChild(3).GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        _hungerMeter.fillAmount = GM.SurvivalManager.HungerPercent;
        _thirstMeter.fillAmount = GM.SurvivalManager.ThirstPercent;
        _staminaMeter.fillAmount = GM.SurvivalManager.StaminaPercent;
        _anoxaemiaMeter.fillAmount = GM.SurvivalManager.AnoxaemiaPercent;
    }
}
