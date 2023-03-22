using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class SurvivalBar : MonoBehaviour
{
    private Image _hungerMeter, _thirstMeter, _staminaMeter, _anoxaemiaMeter;
    private Image _hungerMeterIncrease, _thirstMeterIncrease, _anoxaemiaMeterIncrease;

    private List<Coroutine> increasmentCoroutines;

    private void Awake()
    {
        increasmentCoroutines = new List<Coroutine>();
    }

    private void Start()
    {
        _hungerMeterIncrease = transform.GetChild(0).GetComponent<Image>();
        _hungerMeter = transform.GetChild(1).GetComponent<Image>();
        _thirstMeterIncrease = transform.GetChild(2).GetComponent<Image>();
        _thirstMeter = transform.GetChild(3).GetComponent<Image>();
        _staminaMeter = transform.GetChild(4).GetComponent<Image>();
        _anoxaemiaMeterIncrease = transform.GetChild(5).GetComponent<Image>();
        _anoxaemiaMeter = transform.GetChild(6).GetComponent<Image>();
    }

    private void Update()
    {
        _hungerMeter.fillAmount = GM.SurvivalManager.HungerPercent;
        _thirstMeter.fillAmount = GM.SurvivalManager.ThirstPercent;
        _staminaMeter.fillAmount = GM.SurvivalManager.StaminaPercent;
        _anoxaemiaMeter.fillAmount = GM.SurvivalManager.AnoxaemiaPercent;
    }

    public void ShowIncreasmentFromFood (ItemTypeFood item)
    {
        Debug.Log(12);
        increasmentCoroutines.Add(StartCoroutine(UpdateIncreasmentFromFood(item)));
        Debug.Log(21);
    }
    private IEnumerator UpdateIncreasmentFromFood(ItemTypeFood item)
    {
        while (true)
        {
            _hungerMeterIncrease.fillAmount = (float)item.satiationEffect / 100 + _hungerMeter.fillAmount;
            _thirstMeterIncrease.fillAmount = (float)item.slakingOfThirstEffect / 100 + _thirstMeter.fillAmount;
            _anoxaemiaMeterIncrease.fillAmount = (float)item.oxygenRecovery / 100 + _anoxaemiaMeter.fillAmount;
            yield return new WaitForEndOfFrame();
        }
    }

    public void RemoveIncreasmentFromFood()
    {
        StopCoroutine(increasmentCoroutines[0]);
        increasmentCoroutines.RemoveAt(0);
        Debug.Log(3);
        _hungerMeterIncrease.fillAmount = 0;
        _thirstMeterIncrease.fillAmount = 0;
        _anoxaemiaMeterIncrease.fillAmount = 0;
    }
}
