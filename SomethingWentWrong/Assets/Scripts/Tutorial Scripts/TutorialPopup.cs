using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private int popupIndex;
    
    private void OnEnable()
    {
        if (popupIndex != 0)
            GameManager.GM.Tutorial.PopupSystem.CurrPopup = popupIndex;
    }
}
