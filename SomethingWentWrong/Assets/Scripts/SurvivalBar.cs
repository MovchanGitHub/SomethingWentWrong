using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalBar : MonoBehaviour
{
    private void Start()
    {
        if (SurvivalUIManager.Instance != null)
        {
            SurvivalUIManager.Instance._hungerMeter = transform.GetChild(0).GetComponent<Image>();
            SurvivalUIManager.Instance._thirstMeter = transform.GetChild(1).GetComponent<Image>();
            SurvivalUIManager.Instance._staminaMeter = transform.GetChild(2).GetComponent<Image>();
            SurvivalUIManager.Instance._anoxaemiaMeter = transform.GetChild(3).GetComponent<Image>();
        }
    }
}
