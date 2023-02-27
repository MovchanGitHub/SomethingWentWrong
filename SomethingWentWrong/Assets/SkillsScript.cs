using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsScript : MonoBehaviour
{
    public GameObject skillsWindow;
    public DamagableCharacter playerAsDamagableCharacter;
    public SurvivalManager survivalManager;
    public LightHouse lightHouse;
    
    private const int HEALTH_IMPROVE = 5;
    private const float STAMINA_IMPROVE = 1f;
    private const float STAMINA_RECOVERY = 1f;
    private const float MaxAnoxemia = 1f;
    private const float AnoxemiaEndurance = 1f;
    private const float MaxHunger = 1f;
    private const float HungerEndurance = 1f;
    private const float MaxThirst = 1f;
    private const float ThirstEndurance = 1f;

    private bool isSkillWindowsActive;
    void Start()
    {
        skillsWindow.SetActive(false);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            skillsWindow.SetActive(isSkillWindowsActive = !isSkillWindowsActive);
        }
    }

    public void ImproveHealth() {
        playerAsDamagableCharacter.HP += HEALTH_IMPROVE;
        Debug.Log($"Current health: {playerAsDamagableCharacter.HP}");
        skillsWindow.SetActive(false);
    }
    public void ImproveLightHouseHealth(){
        lightHouse.HP += HEALTH_IMPROVE;
        skillsWindow.SetActive(false);
    }
    public void ImproveStamina(){
        survivalManager.IncreaseMaxStamina(STAMINA_IMPROVE);
        skillsWindow.SetActive(false);
    }
    public void ImproveStaminaRecovery(){
        survivalManager.IncreaseStaminaRecharging(STAMINA_RECOVERY);
        skillsWindow.SetActive(false);
    }
    public void ImproveAnoxemia(){
        survivalManager.IncreaseMaxAnoxemia(MaxAnoxemia);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxAnoxemia(){
        survivalManager.IncreaseAnoxemiaEndurance(AnoxemiaEndurance);
        skillsWindow.SetActive(false);
    }
    public void ImproveHunger(){
        survivalManager.IncreaseMaxHunger(MaxHunger);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxHunger(){
        survivalManager.IncreaseHungerEndurance(HungerEndurance);
        skillsWindow.SetActive(false);
    }
    public void ImproveThrist(){
        survivalManager.IncreaseMaxThirst(MaxThirst);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxThrist(){
        survivalManager.IncreaseThirstEndurance(ThirstEndurance);
        skillsWindow.SetActive(false);
    }
}
