using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class SurvivalBar : MonoBehaviour
{
    private Image _hungerMeter, _thirstMeter, _staminaMeter, _anoxaemiaMeter;
    private Image _hungerMeterIncrease, _thirstMeterIncrease, _anoxaemiaMeterIncrease;
    private Image _hungerMeterDecrease, _thirstMeterDecrease, _anoxaemiaMeterDecrease;
    private float hungerDecreaseAmount = 0, thirstDecreaseAmount = 0, anoxaemiaDecreaseAmount = 0;
    private float HungerDecreaseAmount
    {
        get { return hungerDecreaseAmount; }
        set 
        { 
            if (value < 0) hungerDecreaseAmount = value / GM.SurvivalManager.MaxHunger;
            else hungerDecreaseAmount = 0;
        }
    }
    private float ThirstDecreaseAmount
    {
        get { return thirstDecreaseAmount; }
        set
        {
            if (value < 0) thirstDecreaseAmount = value / GM.SurvivalManager.MaxThirst;
            else thirstDecreaseAmount = 0;
        }
    }
    private float AnoxaemiaDecreaseAmount
    {
        get { return anoxaemiaDecreaseAmount; }
        set
        {
            if (value < 0) anoxaemiaDecreaseAmount = value / GM.SurvivalManager.MaxAnoxaemia;
            else anoxaemiaDecreaseAmount = 0;
        }
    }


    private void Start()
    {
        _hungerMeterIncrease = transform.GetChild(0).GetComponent<Image>();
        _hungerMeterDecrease = transform.GetChild(1).GetComponent<Image>();
        _hungerMeter = transform.GetChild(2).GetComponent<Image>();
        _thirstMeterIncrease = transform.GetChild(3).GetComponent<Image>();
        _thirstMeterDecrease = transform.GetChild(4).GetComponent<Image>();
        _thirstMeter = transform.GetChild(5).GetComponent<Image>();
        _staminaMeter = transform.GetChild(6).GetComponent<Image>();
        _anoxaemiaMeterIncrease = transform.GetChild(7).GetComponent<Image>();
        _anoxaemiaMeterDecrease = transform.GetChild(8).GetComponent<Image>();
        _anoxaemiaMeter = transform.GetChild(9).GetComponent<Image>();
    }

    private void Update()
    {
        _hungerMeter.fillAmount = GM.SurvivalManager.HungerPercent + HungerDecreaseAmount;
        _thirstMeter.fillAmount = GM.SurvivalManager.ThirstPercent + ThirstDecreaseAmount;
        _staminaMeter.fillAmount = GM.SurvivalManager.StaminaPercent;
        _anoxaemiaMeter.fillAmount = GM.SurvivalManager.AnoxaemiaPercent + AnoxaemiaDecreaseAmount;
    }

    public void ShowEffectsFromFood (ItemTypeFood item)
    {
        RemoveEffectsFromFood();
        StartCoroutine(UpdateIncreasmentFromFood(item));
    }

    private IEnumerator UpdateIncreasmentFromFood(ItemTypeFood item)
    {
        HungerDecreaseAmount = item.satiationEffect;
        ThirstDecreaseAmount = item.slakingOfThirstEffect;
        AnoxaemiaDecreaseAmount = item.oxygenRecovery;
        while (true)
        {
            if (item.satiationEffect > 0)
                _hungerMeterIncrease.fillAmount = item.satiationEffect / GM.SurvivalManager.MaxHunger + _hungerMeter.fillAmount;
            else if (item.satiationEffect < 0)
                _hungerMeterDecrease.fillAmount = GM.SurvivalManager.HungerPercent;

            if (item.slakingOfThirstEffect > 0)
                _thirstMeterIncrease.fillAmount = item.slakingOfThirstEffect / GM.SurvivalManager.MaxThirst + _thirstMeter.fillAmount;
            else if (item.slakingOfThirstEffect < 0)
                _thirstMeterDecrease.fillAmount = GM.SurvivalManager.ThirstPercent;

            if (item.oxygenRecovery > 0)
                _anoxaemiaMeterIncrease.fillAmount = item.oxygenRecovery / GM.SurvivalManager.MaxAnoxaemia + _anoxaemiaMeter.fillAmount;
            else if (item.oxygenRecovery < 0)
                _anoxaemiaMeterDecrease.fillAmount = GM.SurvivalManager.AnoxaemiaPercent;

            yield return new WaitForEndOfFrame();
        }
    }

    public void RemoveEffectsFromFood()
    {
        StopAllCoroutines();

        _hungerMeterIncrease.fillAmount = 0;
        _thirstMeterIncrease.fillAmount = 0;
        _anoxaemiaMeterIncrease.fillAmount = 0;
        _hungerMeterDecrease.fillAmount = 0;
        _thirstMeterDecrease.fillAmount = 0;
        _anoxaemiaMeterDecrease.fillAmount = 0;

        hungerDecreaseAmount = 0;
        thirstDecreaseAmount = 0;
        anoxaemiaDecreaseAmount = 0;
    }
}
