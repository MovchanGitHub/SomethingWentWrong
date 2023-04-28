using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using static GameManager;

public class SkillsScript : MonoBehaviour
{
    private GameObject skillsWindow;
    [SerializeField] PlayerDamagable playerDamagable;
    SurvivalManager survivalManager;
    LightHouse lightHouse;

    GameObject[] variantsButtons;
    private GameObject[] logos;
    GameObject variantMask;
    
    private const int ROCKET_HEALTH_BUFF = 100;
    private const int PLAYER_HEALTH_BUFF = 50;
    private const int MAX_HEALTH = 10;
    private const float MAX_STAMINA = 2f;
    private const float STAMINA_RECOVERY = 1f;
    private const float MAX_ANOXEMIA = 35f;
    private const float ANOXEMIA_ENDURANCE = 0.9f;
    private const float MAX_HUNGER = 30f;
    private const float HUNGER_ENDURANCE = 0.9f;
    private const float MAX_THIRST = 20f;
    private const float THIRST_ENDURANCE = 0.9f;

    private float SLIDER_MARGIN;

    private bool isSkillWindowsActive;
    private Unity.Mathematics.Random random;
    private int[] variants;

    private int currentVariant;
    
    [SerializeField] private float time;
    private float _timeLeft;

    private RectTransform hb;
    private RectTransform rh;
    private RectTransform ph;
    private RectTransform phIncr;
    private int rocketHealthUpgradeCount;
    private int playerHealthUpgradeCount;
    private int summaryUpgradeCount;

    void Start() {
        skillsWindow = GM.UI.SkillsMenu;
        survivalManager = GM.SurvivalManager;
        lightHouse = GM.Rocket;
        
        variantMask = skillsWindow.transform.GetChild(0).gameObject;
        
        variantsButtons = new[] { skillsWindow.transform.GetChild(1).gameObject, skillsWindow.transform.GetChild(2).gameObject, skillsWindow.transform.GetChild(3).gameObject };
        var l = skillsWindow.transform.GetChild(4);
        logos = new GameObject[10];
        for (int i = 0; i < 10; ++i)
            logos[i] = l.GetChild(i).GameObject();
        
        
        random = new Unity.Mathematics.Random();
        uint seed = (uint)Random.Range(1, 1000);
        Debug.Log("seed = " + seed);
        random.InitState(seed);
        
        
        hb = GM.UI.HealthBar.GetComponent<RectTransform>();
        rh = GM.UI.RocketHealthSlider.GetComponent<RectTransform>();
        ph = GM.UI.PlayerHealthSlider.GetComponent<RectTransform>();
        phIncr = GM.UI.PlayerHealthIncreaseSlider.GetComponent<RectTransform>();

        SLIDER_MARGIN = rh.sizeDelta.x / 10;
    }

    public void InitSkills() {
        currentVariant = 1;
        variantMask.gameObject.transform.position = variantsButtons[1].gameObject.transform.position;

        var setOfVariants = new HashSet<int>();
        while (setOfVariants.Count != 3)
            setOfVariants.Add(random.NextInt(0, 9));
        variants = setOfVariants.ToArray();
        
        for (var i = 0; i < 3; ++i) {
            logos[variants[i]].transform.position = variantsButtons[i].transform.position;
            logos[variants[i]].SetActive(true);
            variantsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        _timeLeft = time;
        skillsWindow.transform.GetChild(0).gameObject.GetComponent<Animator>().StartPlayback();
        StartCoroutine(SkillTimeExit());
    }

    public IEnumerator SkillTimeExit() {
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            yield return null;
        }
        if (skillsWindow.activeSelf)
            GetSkill();
        foreach (var logo in logos)
            logo.SetActive(false);
        skillsWindow.SetActive(false);

    }
    
    public void GetSkill() {
        switch (variants[currentVariant]) {
                case 0: ImproveHealth(); break;
                case 1: ImproveLightHouseHealth(); break;
                case 2: ImproveStamina(); break;
                case 3: ImproveStaminaRecovery(); break;
                case 4: ImproveAnoxemia(); break;
                case 5: ImproveMaxAnoxemia(); break;
                case 6: ImproveHunger(); break;
                case 7: ImproveMaxHunger(); break;
                case 8: ImproveThrist(); break;
                case 9: ImproveMaxThrist(); break;
        }
        skillsWindow.SetActive(false);
        foreach (var logo in logos)
            logo.SetActive(false);
        variantMask.GetComponent<Image>().color = new Color(255, 240, 0, 255);
        StartCoroutine(GM.PlayerMovement.GetComponentInChildren<PlayerShaderLogic>().Upgrade());
    }
    

    public void OnHighLight(int num) {
        variantMask.gameObject.transform.position = variantsButtons[num].gameObject.transform.position;
        currentVariant = num;
        
        variantsButtons[num].GetComponentInChildren<TextMeshProUGUI>().text = SkillInfo(num);
        logos[variants[num]].SetActive(false);
    }

    public void OnStateExit(int num)
    {
        variantsButtons[num].GetComponentInChildren<TextMeshProUGUI>().text = "";
        logos[variants[num]].SetActive(true);
    }

    private string SkillInfo(int num) {
        switch (variants[num]) {
            case 0:  return "Максимальное здоровье";
            case 1:  return "Максимальное здоровье ракеты";
            case 2:  return "Максимальная выносливость";
            case 3:  return "Скорость восстановления выносливости";
            case 4:  return "Уменьшить скорость расхода кислорода";
            case 5:  return "Максимальный уровень кислорода";
            case 6:  return "Уменьшить скорость накопления голода";
            case 7:  return "Максимальный уровень сытости";
            case 8:  return "Уменьшить скорость накопления жажды";
            default: return "Максимальный уровень жажды";
        }
    }

    public void ImproveHealth() {
        playerDamagable.HP += PLAYER_HEALTH_BUFF;
        playerDamagable.MaxHP += MAX_HEALTH;
        ph.sizeDelta = new Vector2(ph.sizeDelta.x + SLIDER_MARGIN, ph.sizeDelta.y);
        phIncr.sizeDelta = new Vector2(phIncr.sizeDelta.x + SLIDER_MARGIN, phIncr.sizeDelta.y);
        playerHealthUpgradeCount++;
        if (playerHealthUpgradeCount > summaryUpgradeCount)
        {
            summaryUpgradeCount++;
            hb.sizeDelta = new Vector2(hb.sizeDelta.x + SLIDER_MARGIN, hb.sizeDelta.y);
        }
    }
    public void ImproveLightHouseHealth() {
        lightHouse.HP += ROCKET_HEALTH_BUFF;
        lightHouse.MaxHP += MAX_HEALTH;
        rh.sizeDelta = new Vector2(rh.sizeDelta.x + SLIDER_MARGIN, rh.sizeDelta.y);
        rocketHealthUpgradeCount++;
        if (rocketHealthUpgradeCount > summaryUpgradeCount)
        {
            summaryUpgradeCount++;
            hb.sizeDelta = new Vector2(hb.sizeDelta.x + SLIDER_MARGIN, hb.sizeDelta.y);
        }
    }
    public void ImproveStamina() => survivalManager.IncreaseMaxStamina(MAX_STAMINA);
    public void ImproveStaminaRecovery() => survivalManager.IncreaseStaminaRecharging(STAMINA_RECOVERY);
    public void ImproveAnoxemia() => survivalManager.IncreaseAnoxemiaEndurance(ANOXEMIA_ENDURANCE);
    public void ImproveMaxAnoxemia() => survivalManager.IncreaseMaxAnoxemia(MAX_ANOXEMIA);
    public void ImproveHunger() => survivalManager.IncreaseHungerEndurance(HUNGER_ENDURANCE);
    public void ImproveMaxHunger() => survivalManager.IncreaseMaxHunger(MAX_HUNGER);
    public void ImproveThrist() => survivalManager.IncreaseThirstEndurance(THIRST_ENDURANCE);
    public void ImproveMaxThrist() => survivalManager.IncreaseMaxThirst(MAX_THIRST);
}
