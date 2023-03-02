using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;


public enum DayTime { Sunrise = 0, Day, Sunset, Night, Midnight }

public class DayNightCycle : MonoBehaviour
{
    private Light2D globalLight;
    public float currentTime;
    public float cycleDuration = 10;
    [HideInInspector] public DayTime dayCycle;
    private SpawnSystem spawnSystem;

    private int dayCount;

    //public RetroMaskScript retroMask;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject skillWindow;

    [SerializeField] private Color sunriseColor;
    [SerializeField] private Color dayColor;
    [SerializeField] private Color sunsetColor;
    [SerializeField] private Color nightColor;
    [SerializeField] private Color midnightColor;

    [SerializeField] private float sunriseIntensity;
    [SerializeField] private float dayIntensity;
    [SerializeField] private float sunsetIntensity;
    [SerializeField] private float nightIntensity;
    [SerializeField] private float midnightIntensity;

    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
    }

    void Start() 
    {
        spawnSystem = GameManager.GM.Rocket.GetComponentInChildren<SpawnSystem>();

        currentTime = 0;
        dayCycle = DayTime.Day;
        globalLight.color = sunriseColor;
        globalLight.intensity = sunriseIntensity;
    }

     void Update()
     {
        currentTime += Time.deltaTime;

        if (currentTime >= cycleDuration)
        {
            dayCount++;
            currentTime = 0;
            
            dayCycle++;
            if (dayCycle > DayTime.Midnight)
                dayCycle = 0;
            
            switch (dayCycle)
            {
                case DayTime.Sunrise:
                    // Обработка пережитого дня
                    skillWindow.GetComponentInParent<SkillsScript>().InitSkills();
                    skillWindow.SetActive(true);
                    //StartCoroutine(retroMask.Decrease());
                    spawnSystem.spawnEnabled = false;
                    break;

                case DayTime.Day:
                    if (dayCount == 15)
                        winScreen.SetActive(true);
                    break;

                case DayTime.Sunset:
                    break;

                case DayTime.Night:
                    //StartCoroutine(retroMask.Increase());
                    spawnSystem.spawnEnabled = true;
                    break;

                case DayTime.Midnight:
                    break;
            }
        }

        float percent = currentTime / cycleDuration;

        switch (dayCycle)
        {
            case DayTime.Sunrise:
                globalLight.color = Color.Lerp(sunriseColor, dayColor, percent);
                globalLight.intensity = Mathf.Lerp(sunriseIntensity, dayIntensity, percent);
                break;

            case DayTime.Day:
                globalLight.color = Color.Lerp(dayColor, sunsetColor, percent);
                globalLight.intensity = Mathf.Lerp(dayIntensity, sunsetIntensity, percent);
                break;

            case DayTime.Sunset:
                globalLight.color = Color.Lerp(sunsetColor, nightColor, percent);
                globalLight.intensity = Mathf.Lerp(sunsetIntensity, nightIntensity, percent);
                break;

            case DayTime.Night:
                globalLight.color = Color.Lerp(nightColor, midnightColor, percent);
                globalLight.intensity = Mathf.Lerp(nightIntensity, midnightIntensity, percent);
                break;

            case DayTime.Midnight:
                globalLight.color = Color.Lerp(midnightColor, sunriseColor, percent);
                globalLight.intensity = Mathf.Lerp(midnightIntensity, sunriseIntensity, percent);
                break;
        }
     }
}