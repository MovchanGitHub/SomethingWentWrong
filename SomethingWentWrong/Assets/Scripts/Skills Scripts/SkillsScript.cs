using System;
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
    
    private const int PLAYER_HEALTH_BUFF = 25;
    private const int ROCKET_HEALTH_BUFF = 60;
    private const int MAX_HEALTH_IMPROVE = 10;
    private const int MAX_ROCKET_HEALTH_IMPROVE = 30;
    private const float MAX_STAMINA = 2f;
    private const float STAMINA_RECOVERY = 1f;
    private const float MAX_ANOXEMIA = 35f;
    private const float ANOXEMIA_ENDURANCE = 0.9f;
    private const float MAX_HUNGER = 30f;
    private const float HUNGER_ENDURANCE = 0.9f;
    private const float MAX_THIRST = 20f;
    private const float THIRST_ENDURANCE = 0.9f;
    public float SPEED_IMPROVE = 0.1f;
    private const int FISTS_DAMAGE = 1;

    private float SLIDER_MARGIN;

    private bool isSkillWindowsActive;
    private Unity.Mathematics.Random random;
    private List<int> accessedSkills;
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
        logos = new GameObject[12];
        for (int i = 0; i < 12; ++i)
            logos[i] = l.GetChild(i).GameObject();
        
        
        random = new Unity.Mathematics.Random();
        uint seed = (uint)Random.Range(1, 1000);
        Debug.Log("seed = " + seed);
        random.InitState(seed);
        
        accessedSkills = Enumerable.Range(0, 12).ToList();
        
        
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
            setOfVariants.Add(accessedSkills[random.NextInt(0, accessedSkills.Count - 1)]);
        variants = setOfVariants.ToArray();
        // Debug.Log(String.Join(" ", accessedSkills));
        // Debug.Log(String.Join(" ", variants));
        
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

    private void GetSkill() {
        switch (variants[currentVariant]) {
                case 0: ImproveHealth(); Debug.Log($"Здоровье увеличено на {MAX_HEALTH_IMPROVE}"); break;
                case 1: ImproveLightHouseHealth(); Debug.Log($"Прочность ракеты увеличена на {MAX_HEALTH_IMPROVE}"); break;
                case 2: ImproveStamina(); Debug.Log($"Максимальный уровень выносливости увеличен на {MAX_STAMINA}"); break;
                case 3: ImproveStaminaRecovery(); Debug.Log($"Скорость восстановления выносливости увеличена на {STAMINA_RECOVERY}"); break;
                case 4: ImproveAnoxemia(); Debug.Log($"Скорость расхода кислорода уменьшен на {ANOXEMIA_ENDURANCE}"); break;
                case 5: ImproveMaxAnoxemia(); Debug.Log($"Максимальный уровень кислорода увеличен на {MAX_ANOXEMIA}"); break;
                case 6: ImproveHunger(); Debug.Log($"Расход сытости уменьшен на {HUNGER_ENDURANCE}"); break;
                case 7: ImproveMaxHunger(); Debug.Log($"Максимальная сытость увеличена на {MAX_HUNGER}"); break;
                case 8: ImproveThrist(); Debug.Log($"Скорость расхода жажды уменьшена на {THIRST_ENDURANCE}"); break;
                case 9: ImproveMaxThrist(); Debug.Log($"Максимальный уровень жажды увеличен {MAX_THIRST}"); break;
                case 10: ImproveFistsDamage(); Debug.Log($"Урон в ближнем бою увеличен на {FISTS_DAMAGE}"); break;
                case 11: ImproveSpeed(); Debug.Log($"Скорость бега увеличена на {SPEED_IMPROVE}"); break;
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

    private string SkillInfo(int num)
    {
        return variants[num] switch
        {
            0 => "Максимальное здоровье",
            1 => "Максимальная прочность ракеты",
            2 => "Максимальная выносливость",
            3 => "Скорость восстановления выносливости",
            4 => "Уменьшить скорость расхода кислорода",
            5 => "Максимальный уровень кислорода",
            6 => "Уменьшить скорость накопления голода",
            7 => "Максимальный уровень сытости",
            8 => "Уменьшить скорость накопления жажды",
            9 => "Максимальный уровень жажды",
            10 => "Увеличить урон ближнего боя",
            _ => "Увеличить скорость бега"
        };
    }

    public void ImproveHealth() {
        playerDamagable.MaxHP += MAX_HEALTH_IMPROVE;
        playerDamagable.HP += PLAYER_HEALTH_BUFF;
        ph.sizeDelta = new Vector2(ph.sizeDelta.x + SLIDER_MARGIN, ph.sizeDelta.y);
        phIncr.sizeDelta = new Vector2(phIncr.sizeDelta.x + SLIDER_MARGIN, phIncr.sizeDelta.y);
        if (++playerHealthUpgradeCount == 5)
            accessedSkills.Remove(0);
        
        if (playerHealthUpgradeCount <= summaryUpgradeCount) return;
        summaryUpgradeCount++;
        hb.sizeDelta = new Vector2(hb.sizeDelta.x + SLIDER_MARGIN, hb.sizeDelta.y);
    }
    public void ImproveLightHouseHealth() {
        lightHouse.MaxHP += MAX_ROCKET_HEALTH_IMPROVE;
        lightHouse.HP += ROCKET_HEALTH_BUFF;
        rh.sizeDelta = new Vector2(rh.sizeDelta.x + SLIDER_MARGIN, rh.sizeDelta.y);
        if (++rocketHealthUpgradeCount == 5)
            accessedSkills.Remove(1);
        
        if (rocketHealthUpgradeCount <= summaryUpgradeCount) return;
        summaryUpgradeCount++;
        hb.sizeDelta = new Vector2(hb.sizeDelta.x + SLIDER_MARGIN, hb.sizeDelta.y);
    }
    public void ImproveStamina() => survivalManager.IncreaseMaxStamina(MAX_STAMINA);
    public void ImproveStaminaRecovery() => survivalManager.IncreaseStaminaRecharging(STAMINA_RECOVERY);
    public void ImproveAnoxemia() => survivalManager.IncreaseAnoxemiaEndurance(ANOXEMIA_ENDURANCE);
    public void ImproveMaxAnoxemia() => survivalManager.IncreaseMaxAnoxemia(MAX_ANOXEMIA);
    public void ImproveHunger() => survivalManager.IncreaseHungerEndurance(HUNGER_ENDURANCE);
    public void ImproveMaxHunger() => survivalManager.IncreaseMaxHunger(MAX_HUNGER);
    public void ImproveThrist() => survivalManager.IncreaseThirstEndurance(THIRST_ENDURANCE);
    public void ImproveMaxThrist() => survivalManager.IncreaseMaxThirst(MAX_THIRST);
    public void ImproveFistsDamage() => GM.AttackPoint.Damage += FISTS_DAMAGE;
    public void ImproveSpeed() => GM.PlayerMovement.ImproveSpeed();
}
