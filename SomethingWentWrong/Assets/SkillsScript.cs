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
    GameObject variantMask;
    
    private const int MAX_HEALTH = 10;
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

    private int currentVariant;
    
    [SerializeField] private float time;
    private float _timeLeft = 0f;

    private RectTransform hb;
    private RectTransform rh;
    private RectTransform ph;
    private int rocketHealthUpgradeCount = 0;
    private int playerHealthUpgradeCount = 0;
    private int summaryUpgradeCount = 0;

    void Start() {
        skillsWindow = GM.UI.SkillsMenu;
        survivalManager = GM.SurvivalManager;
        lightHouse = GM.Rocket;
        
        variantMask = skillsWindow.transform.GetChild(0).gameObject;
        variantsButtons = new GameObject[] { skillsWindow.transform.GetChild(1).gameObject, skillsWindow.transform.GetChild(2).gameObject, skillsWindow.transform.GetChild(3).gameObject };

        
        random = new Unity.Mathematics.Random();
        random.InitState(1851936439U);
        
        skillsWindow.SetActive(false);
        InitSkills();
        
        hb = GM.UI.HealthBar.GetComponent<RectTransform>();
        rh = GM.UI.RocketHealthSlider.GetComponent<RectTransform>();
        ph = GM.UI.PlayerHealthSlider.GetComponent<RectTransform>();
    }

    public void InitSkills()
    {
        currentVariant = 2;
        variantMask.gameObject.transform.position = variantsButtons[1].gameObject.transform.position;

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
        _timeLeft = time;
        StartCoroutine(SkillTime());
    }

    public IEnumerator SkillTime()
    {
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            yield return null;
        }
        skillsWindow.SetActive(false);
    }
    
    public void GetSkill() {
        switch (variants[currentVariant - 1]) {
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
        StartCoroutine(GM.PlayerMovement.GetComponentInChildren<PlayerShaderLogic>().Upgrade());
    }
    

    public void OnHighLight(int var)
    {
        switch (var)
        {
            case 1: 
                variantMask.gameObject.transform.position = variantsButtons[0].gameObject.transform.position;
                currentVariant = 1;
                break;
            case 2: 
                variantMask.gameObject.transform.position = variantsButtons[1].gameObject.transform.position;
                currentVariant = 2;
                break;
            case 3: 
                variantMask.gameObject.transform.position = variantsButtons[2].gameObject.transform.position;
                currentVariant = 3;
                break;
        }
    }

    // Skill debug
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //    ImproveHealth();
        //if (Input.GetKeyDown(KeyCode.Y))
        //    ImproveLightHouseHealth();
    }
    

    
    public void ImproveHealth() {
        playerDamagable.MaxHP += MAX_HEALTH;
        playerDamagable.HP += MAX_HEALTH;
        skillsWindow.SetActive(false);
        ph.sizeDelta = new Vector2(ph.sizeDelta.x + 5, ph.sizeDelta.y);
        playerHealthUpgradeCount++;
        if (playerHealthUpgradeCount > summaryUpgradeCount)
        {
            summaryUpgradeCount++;
            hb.sizeDelta = new Vector2(hb.sizeDelta.x + 5, hb.sizeDelta.y);
        }
    }
    public void ImproveLightHouseHealth() {
        lightHouse.HP += MAX_HEALTH;
        lightHouse.MaxHP += MAX_HEALTH;
        skillsWindow.SetActive(false);
        rh.sizeDelta = new Vector2(rh.sizeDelta.x + 5, rh.sizeDelta.y);
        rocketHealthUpgradeCount++;
        if (rocketHealthUpgradeCount > summaryUpgradeCount)
        {
            summaryUpgradeCount++;
            hb.sizeDelta = new Vector2(hb.sizeDelta.x + 5, hb.sizeDelta.y);
        }
    }
    public void ImproveStamina() {
        survivalManager.IncreaseMaxStamina(MAX_STAMINA);
        skillsWindow.SetActive(false);
    }
    public void ImproveStaminaRecovery() {
        survivalManager.IncreaseStaminaRecharging(STAMINA_RECOVERY);
        skillsWindow.SetActive(false);
    }
    public void ImproveAnoxemia() {
        survivalManager.IncreaseAnoxemiaEndurance(ANOXEMIA_ENDURANCE);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxAnoxemia() {
        survivalManager.IncreaseMaxAnoxemia(MAX_ANOXEMIA);
        skillsWindow.SetActive(false);
    }
    public void ImproveHunger() {
        survivalManager.IncreaseHungerEndurance(HUNGER_ENDURANCE);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxHunger() {
        survivalManager.IncreaseMaxHunger(MAX_HUNGER);
        skillsWindow.SetActive(false);
    }
    public void ImproveThrist() {
        survivalManager.IncreaseThirstEndurance(THIRST_ENDURANCE);
        skillsWindow.SetActive(false);
    }
    public void ImproveMaxThrist() {
        survivalManager.IncreaseMaxThirst(MAX_THIRST);
        skillsWindow.SetActive(false);
    }
}
