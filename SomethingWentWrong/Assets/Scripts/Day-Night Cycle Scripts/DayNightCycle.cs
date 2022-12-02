using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public enum DayTime { Sunrise = 0, Day, Sunset, Night, Midnight }

public class DayNightCycle : MonoBehaviour
{
    private Light2D globalLight;
    public float currentTime;
    public float cycleDuration = 10;
    [HideInInspector] public DayTime dayCycle;
    private SpawnSystem spawnSystem;

    private int dayCount;

    public GameObject winMenu;
    
    public Color sunrise;
    public Color day;
    public Color sunset;
    public Color night;
    public Color midnight;

    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
    }

    void Start() 
    {
        currentTime = 0;
        dayCycle = DayTime.Day;
        globalLight.color = sunrise;
        
        if (GameManagerScript.instance.lightHouse)
        {
            spawnSystem = GameManagerScript.instance.lightHouse.transform.GetComponentInChildren<SpawnSystem>();
        }
    }

     void Update()
     {
        currentTime += Time.deltaTime;

        if (currentTime >= cycleDuration)
        {
            dayCount++;
            currentTime = 0;
            dayCycle++;
        }

        if (dayCycle > DayTime.Midnight)
            dayCycle = 0;

        float percent = currentTime / cycleDuration;

        switch (dayCycle)
        {
            case DayTime.Sunrise:
                globalLight.color = Color.Lerp(sunrise, day, percent);
                spawnSystem.spawnEnabled = false;
                break;

            case DayTime.Day:
                globalLight.color = Color.Lerp(day, sunset, percent);
                spawnSystem.spawnEnabled = false;
                if (dayCount == 15)
                    winMenu.SetActive(true);
                break;

            case DayTime.Sunset:
                globalLight.color = Color.Lerp(sunset, night, percent);
                spawnSystem.spawnEnabled = false;
                break;

            case DayTime.Night:
                globalLight.color = Color.Lerp(night, midnight, percent);
                spawnSystem.spawnEnabled = true;
                break;

            case DayTime.Midnight:
                globalLight.color = Color.Lerp(midnight, sunrise, percent);
                spawnSystem.spawnEnabled = true;
                break;
        }
     }
}