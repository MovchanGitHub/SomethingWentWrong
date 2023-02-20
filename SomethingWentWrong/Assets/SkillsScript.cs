using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsScript : MonoBehaviour
{
    public GameObject skillsWindow;
    public DamagableCharacter playerAsDamagableCharacter;
    public SurvivalManager survivalManager;
    public LightHouse lightHouse;

    // [SerializeField] public int SCORE = 0;
    
    private const int HEALTH_IMPROVE = 5;
    private const int STAMINA_IMPROVE = 1;

    private bool isSkillWindowsActive;
    // Start is called before the first frame update
    void Start()
    {
        skillsWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            skillsWindow.SetActive(isSkillWindowsActive = !isSkillWindowsActive);
        }
    }

    public void ImproveHealth()
    {
        playerAsDamagableCharacter.HP += HEALTH_IMPROVE;
        Debug.Log($"Current health: {playerAsDamagableCharacter.HP}");
        skillsWindow.SetActive(false);
    }

    public void ImproveStamina()
    {
        survivalManager.IncreaseMaxStamina(STAMINA_IMPROVE);
        skillsWindow.SetActive(false);
    }

    public void ImproveLightHouseHealth()
    {
        lightHouse.HP += HEALTH_IMPROVE;
        skillsWindow.SetActive(false);
    }
}
