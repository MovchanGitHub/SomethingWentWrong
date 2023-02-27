using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;
using Random = UnityEngine.Random;

public class SkillsScript : MonoBehaviour
{
    public GameObject skillsWindow;
    public PlayerDamagable playerDamagable;
    public SurvivalManager survivalManager;
    public LightHouse lightHouse;

    public GameObject[] variantsButtons;
    
    private const int MAX_HEALTH = 5;
    private const float MAX_STAMINA = 1f;
    private const float STAMINA_RECOVERY = 1f;
    private const float MAX_ANOXEMIA = 1f;
    private const float ANOXEMIA_ENDURANCE = 1f;
    private const float MAX_HUNGER = 1f;
    private const float HUNGER_ENDURANCE = 1f;
    private const float MAX_THIRST = 1f;
    private const float THIRST_ENDURANCE = 1f;

    private bool isSkillWindowsActive;
    private Unity.Mathematics.Random random;
    private int[] variants; 
    void Start() {
        random = new Unity.Mathematics.Random();
        random.InitState(1851936439U);
        skillsWindow.SetActive(false);
        InitSkills();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            skillsWindow.SetActive(isSkillWindowsActive = !isSkillWindowsActive);
        }
    }

    public void InitSkills() {
        var setOfVariants = new HashSet<int>();
        while (setOfVariants.Count != 3)
            setOfVariants.Add(random.NextInt(1, 10));
        variants = setOfVariants.ToArray();
        for (var i = 0; i < 3; ++i) {
            switch (variants[i]) {
                case 1: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ImproveHealth"; break;
                case 2: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ImproveLightHouseHealth"; break;
                case 3: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ImproveStamina"; break;
                case 4: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ImproveStaminaRecovery"; break;
                case 5: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ImproveAnoxemia"; break;
                case 6: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ImproveMaxAnoxemia"; break;
                case 7: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ImproveHunger"; break;
                case 8: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ImproveMaxHunger"; break;
                case 9: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ImproveThrist"; break;
                case 10: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ImproveMaxThrist"; break;
            }
        }
    }
    public void GetSkill(int button) {
        Debug.Log($"{variants[button]} ");
        switch (variants[button]) {
                case 1: ImproveHealth(); break;
                case 2: ImproveLightHouseHealth(); break;
                case 3: ImproveStamina(); break;
                case 4: ImproveStaminaRecovery(); break;
                case 5: ImproveAnoxemia(); break;
                case 6: ImproveMaxAnoxemia(); break;
                case 7: ImproveHunger(); break;
                case 8: ImproveMaxHunger(); break;
                case 9: ImproveThrist(); break;
                case 10: ImproveMaxThrist(); break;
        }
    }

    public void ImproveHealth(){
        playerDamagable.MaxHp += MAX_HEALTH;
        Debug.Log($"Max HP: {playerDamagable.MaxHp}");
        skillsWindow.SetActive(false);
    }
    public void ImproveLightHouseHealth(){
        lightHouse.HP += MAX_HEALTH;
        skillsWindow.SetActive(false);
    }
    public void ImproveStamina(){
        survivalManager.IncreaseMaxStamina(MAX_STAMINA);
        skillsWindow.SetActive(false);
    }
    public void ImproveStaminaRecovery(){
        survivalManager.IncreaseStaminaRecharging(STAMINA_RECOVERY);
        skillsWindow.SetActive(false);
    }
    public void ImproveAnoxemia(){
        survivalManager.IncreaseMaxAnoxemia(MAX_ANOXEMIA);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxAnoxemia(){
        survivalManager.IncreaseAnoxemiaEndurance(ANOXEMIA_ENDURANCE);
        skillsWindow.SetActive(false);
    }
    public void ImproveHunger(){
        survivalManager.IncreaseMaxHunger(MAX_HUNGER);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxHunger(){
        survivalManager.IncreaseHungerEndurance(HUNGER_ENDURANCE);
        skillsWindow.SetActive(false);
    }
    public void ImproveThrist(){
        survivalManager.IncreaseMaxThirst(MAX_THIRST);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxThrist(){
        survivalManager.IncreaseThirstEndurance(THIRST_ENDURANCE);
        skillsWindow.SetActive(false);
    }
}
