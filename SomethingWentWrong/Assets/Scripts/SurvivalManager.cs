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
    public GameObject player;

    public IsometricPlayerMovementController playerController;

    private static SurvivalManager instance;

    public float staminaToRush = 1;

    public bool CanRun() => currentStamina > 0f;
    public bool CanRush() => currentStamina >= staminaToRush;

    public static SurvivalManager Instance
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

    private void Start()
    {
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        currentAnoxaemia = maxAnoxaemia;
        currentStamina = maxStamina;
        playerController = player.GetComponent<IsometricPlayerMovementController>();
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
            SpawnSystemScript.instance.GameOver("Вы умерли");
            //transform.gameObject.SetActive(false);
        }

        //if player runs
        if (playerController.IsRunning)
        {
            currentStamina   -= staminaDepletionRate   * Time.deltaTime;
            currentStaminaDelayCounter = 0;
        }

        //if player runs and...
        if (!playerController.IsRunning && currentStamina < maxStamina)
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
