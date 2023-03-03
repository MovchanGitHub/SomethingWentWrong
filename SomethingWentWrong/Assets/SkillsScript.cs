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
using static GameManager;

public class SkillsScript : MonoBehaviour
{
    GameObject skillsWindow;
    [SerializeField] PlayerDamagable playerDamagable;
    SurvivalManager survivalManager;
    LightHouse lightHouse;

    GameObject[] variantsButtons;
    
    private const int MAX_HEALTH = 5;
    private const float MAX_STAMINA = 1f;
    private const float STAMINA_RECOVERY = 1f;
    private const float MAX_ANOXEMIA = 5f;
    private const float ANOXEMIA_ENDURANCE = 0.9f;
    private const float MAX_HUNGER = 5f;
    private const float HUNGER_ENDURANCE = 0.9f;
    private const float MAX_THIRST = 5f;
    private const float THIRST_ENDURANCE = 0.9f;

    private bool isSkillWindowsActive;
    private Unity.Mathematics.Random random;
    private int[] variants;

    void Start() {
        skillsWindow = GM.UI.SkillsMenu;
        survivalManager = GM.SurvivalManager;
        lightHouse = GM.Rocket;
        variantsButtons = new GameObject[] { skillsWindow.transform.GetChild(0).gameObject, skillsWindow.transform.GetChild(1).gameObject, skillsWindow.transform.GetChild(2).gameObject };

        random = new Unity.Mathematics.Random();
        random.InitState(1851936439U);
        skillsWindow.SetActive(false);
        InitSkills();
    }

    public void InitSkills() {
        var setOfVariants = new HashSet<int>();
        while (setOfVariants.Count != 3)
            setOfVariants.Add(random.NextInt(1, 10));
        variants = setOfVariants.ToArray();
        for (var i = 0; i < 3; ++i) {
            switch (variants[i]) {
                case 1:  variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Максимальное здоровье"; break;
                case 2:  variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Починить ракету"; break;
                case 3:  variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Максимальная выносливость"; break;
                case 4:  variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Выносливость"; break;
                case 5:  variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Кислородная выносливость"; break;
                case 6:  variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Максимальный кислород"; break;
                case 7:  variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Сытость"; break;
                case 8:  variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Максимальная сытость"; break;
                case 9:  variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Жажда"; break;
                case 10: variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Максимальная жажда"; break;
            }
        }
    }
    public void GetSkill(int button) {
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
        playerDamagable.MaxHP += MAX_HEALTH;
        playerDamagable.HP += MAX_HEALTH;
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
        survivalManager.IncreaseAnoxemiaEndurance(ANOXEMIA_ENDURANCE);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxAnoxemia(){
        survivalManager.IncreaseMaxAnoxemia(MAX_ANOXEMIA);
        skillsWindow.SetActive(false);
    }
    public void ImproveHunger(){
        survivalManager.IncreaseHungerEndurance(HUNGER_ENDURANCE);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxHunger(){
        survivalManager.IncreaseMaxHunger(MAX_HUNGER);
        skillsWindow.SetActive(false);
    }
    public void ImproveThrist(){
        survivalManager.IncreaseThirstEndurance(THIRST_ENDURANCE);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxThrist(){
        survivalManager.IncreaseMaxThirst(MAX_THIRST);
        skillsWindow.SetActive(false);
    }
}
