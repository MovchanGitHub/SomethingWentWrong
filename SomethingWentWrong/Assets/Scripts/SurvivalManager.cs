using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalManager : MonoBehaviour
{
    //голод
    [Header("Hunger")] 
    [SerializeField] private float maxHunger;
    [SerializeField] private float hungerDepletionRate;
    private float currentHunger;
    public float HungerPercent => currentHunger / maxHunger;
    
    //жажда
    [Header("Thirst")] 
    [SerializeField] private float maxThirst;
    [SerializeField] private float thirstDepletionRate;
    private float currentThirst;
    public float ThirstPercent => currentThirst / maxThirst;

    //кислородное голодание
    [Header("Anoxaemia")] 
    [SerializeField] private float maxAnoxaemia;
    [SerializeField] private float anoxaemiaDepletionRate;
    private float currentAnoxaemia;
    public float AnoxaemiaPercent => currentAnoxaemia / maxAnoxaemia;
    
    [Header("Stamina")] 
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaDepletionRate;
    [SerializeField] private float staminaRechargeRate;
    [SerializeField] private float staminaRechargeDelay;
    private float currentStamina;
    private float currentStaminaDelayCounter;
    public float StaminaPercent => currentStamina / maxStamina;

    [Header("Player References")] 
    [SerializeField] private GameObject player;

    private void Start()
    {
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        currentAnoxaemia = maxAnoxaemia;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        currentHunger    -= hungerDepletionRate    * Time.deltaTime;
        currentThirst    -= thirstDepletionRate    * Time.deltaTime;
        currentAnoxaemia -= anoxaemiaDepletionRate * Time.deltaTime;

        if (currentHunger <= 0 || currentThirst <= 0 || currentAnoxaemia <= 0)
        {
            //player dies :/
            currentHunger = 0;
            currentThirst = 0;
            currentAnoxaemia = 0;
        }

        //if player runs
        if (false)
        {
            currentStamina   -= staminaDepletionRate   * Time.deltaTime;
            currentStaminaDelayCounter = 0;
        }
        
        //if player runs and...
        if (false && currentStamina < maxStamina)
        {
            if (currentStaminaDelayCounter < staminaRechargeDelay)
            {
                currentStaminaDelayCounter += Time.deltaTime;
            }

            if (currentStaminaDelayCounter >= staminaRechargeDelay)
            {
                currentStamina += staminaRechargeRate * Time.deltaTime;
                if (currentStamina > maxStamina)
                    currentStamina = maxStamina;
            }
        }
    }

    public void ReplenishHunger(float hungerAmount)
    {
        currentHunger += hungerAmount;
        if (currentHunger > maxHunger)
            currentHunger = maxHunger;
    }

    public void ReplenishThirst(float thirstAmount)
    {
        currentThirst += thirstAmount;
        if (currentThirst > maxThirst)
            currentThirst = maxThirst;
    }

    public void ReplenishAnoxaemia(float anoxaemiaAmount)
    {
        currentAnoxaemia += anoxaemiaAmount;
        if (currentAnoxaemia > maxAnoxaemia)
            currentAnoxaemia = maxAnoxaemia;
    }
}