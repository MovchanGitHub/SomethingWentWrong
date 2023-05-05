using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class SurvivalManager : MonoBehaviour
{
    //голод
    [Header("Hunger")] 
    [SerializeField] private float maxHunger;
    public float MaxHunger { get => maxHunger; }
    [SerializeField] private float hungerDepletionRate;
    private float currentHunger;
    public float CurrentHunger
    {
        get { return currentHunger; }
        set
        {
            currentHunger = value;
            if (currentHunger < 0)
                currentHunger = 0;
            else if (currentHunger > MaxHunger)
                currentHunger = MaxHunger;
        }
    }
    public float HungerPercent => currentHunger / maxHunger;
    public int hungerDamage = 2;

    //жажда
    [Header("Thirst")] 
    [SerializeField] private float maxThirst;
    public float MaxThirst { get => maxThirst; }
    [SerializeField] private float thirstDepletionRate;
    private float currentThirst;
    public float CurrentThirst
    {
        get { return currentThirst; }
        set
        {
            currentThirst = value;
            if (currentThirst < 0)
                currentThirst = 0;
            else if (currentThirst > MaxThirst)
                currentThirst = MaxThirst;
        }
    }
    public float ThirstPercent => currentThirst / maxThirst;
    public int thirstDamage = 3;

    //кислородное голодание
    [Header("Anoxaemia")] [SerializeField] private float maxAnoxaemia;
    public float MaxAnoxaemia { get => maxAnoxaemia; }

    [SerializeField] private float anoxaemiaDepletionRate;
    private float currentAnoxaemia;
    public float CurrentAnoxaemia
    {
        get { return currentAnoxaemia; }
        set
        {
            currentAnoxaemia = value;
            if (currentAnoxaemia < 0)
                currentAnoxaemia = 0;
            else if (currentAnoxaemia > MaxAnoxaemia)
                currentAnoxaemia = MaxAnoxaemia;
        }
    }
    public float AnoxaemiaPercent => currentAnoxaemia / maxAnoxaemia;
    public int anoxaemiaDamage = 5;

    
    [Header("Stamina")] 
    [SerializeField] private float maxStamina;
    public float MaxStamina { get => maxStamina; }
    [SerializeField] private float staminaDepletionRate;
    [SerializeField] private float staminaRechargeRate;
    [SerializeField] private float staminaRechargeDelay;
    private float currentStamina;
    private float currentStaminaDelayCounter;
    public float StaminaPercent => currentStamina / maxStamina;

    public SurvivalPlayerDamage survivalPlayerDamage;

    public int LoseHpEffect
    {
        get
        {
            int loseHpEffect = 0;
            if (currentHunger < 0.01f)    loseHpEffect += hungerDamage;
            if (currentThirst < 0.01f)    loseHpEffect += thirstDamage;
            if (currentAnoxaemia < 0.01f) loseHpEffect += anoxaemiaDamage;

            return loseHpEffect;
        }
    }


    public float staminaToRush = 1;

    public bool CanRun() => currentStamina > 0f;
    public bool CanRush() => currentStamina >= staminaToRush;

    private void Start()
    {
        survivalPlayerDamage = GetComponent<SurvivalPlayerDamage>();
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        currentAnoxaemia = maxAnoxaemia;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        CurrentHunger    -= hungerDepletionRate    * Time.deltaTime;
        CurrentThirst    -= thirstDepletionRate    * Time.deltaTime;
        CurrentAnoxaemia -= anoxaemiaDepletionRate * Time.deltaTime;

        // if (currentHunger <= 0 || currentThirst <= 0 || currentAnoxaemia <= 0)
        // {
        //     //player dies :/
        //     if (currentHunger <= 0)
        //         GM.GameOver("Вы умерли от голода");
        //     if (currentThirst <= 0)
        //         GM.GameOver("Вы умерли от жажды");
        //     if (currentAnoxaemia <= 0)
        //         GM.GameOver("Вы умерли от нехватки кислорода");
        //     //transform.gameObject.SetActive(false);
        // }

        //if player runs
        if (GM.PlayerMovement.IsRunning && GM.PlayerMovement.IsMoving)
        {
            currentStamina   -= staminaDepletionRate   * Time.deltaTime;
            currentStaminaDelayCounter = 0;
        }

        //if player runs and...
        if (!GM.PlayerMovement.IsRunning && currentStamina < maxStamina)
        {
            if (currentStaminaDelayCounter < staminaRechargeDelay)
            {
                currentStaminaDelayCounter += Time.deltaTime;
            }
            else
            {
                currentStamina += staminaRechargeRate * Time.deltaTime;
                if (currentStamina > maxStamina)
                    currentStamina = maxStamina;
            }
        }
    }

    public void SetDefault()
    {
        currentHunger = maxHunger;
        currentStamina = maxStamina;
        currentThirst = maxThirst;
        currentAnoxaemia = maxAnoxaemia;
    }

    //public void ReplenishHunger(float hungerAmount)
    //{
    //    currentHunger += hungerAmount;
    //    if (currentHunger > maxHunger)
    //        currentHunger = maxHunger;
    //    else if (currentHunger < 0)
    //        currentHunger = 0;
    //}

    //public void ReplenishThirst(float thirstAmount)
    //{
    //    currentThirst += thirstAmount;
    //    if (currentThirst > maxThirst)
    //        currentThirst = maxThirst;
    //    else if (currentThirst < 0)
    //        currentThirst = 0;
    //}

    //public void ReplenishAnoxaemia(float anoxaemiaAmount)
    //{
    //    currentAnoxaemia += anoxaemiaAmount;
    //    if (currentAnoxaemia > maxAnoxaemia)
    //        currentAnoxaemia = maxAnoxaemia;
    //    else if (currentAnoxaemia < 0)
    //        currentAnoxaemia = 0;
    //}
    
    public void ReplenishStamina(float staminaAmount)
    {
        currentStamina += staminaAmount;
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
    }

    public void IncreaseMaxStamina(float value)  => maxStamina += value;
    public void IncreaseStaminaRecharging(float value) => staminaRechargeRate += value;
    public void IncreaseMaxAnoxemia(float value) => maxAnoxaemia += value;
    public void IncreaseAnoxemiaEndurance(float value) => anoxaemiaDepletionRate *= value;
    public void IncreaseMaxHunger(float value) => maxHunger += value;
    public void IncreaseHungerEndurance(float value) => hungerDepletionRate *= value;
    public void IncreaseMaxThirst(float value) => maxThirst += value;
    public void IncreaseThirstEndurance(float value) => thirstDepletionRate *= value;
}
