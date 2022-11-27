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

    private int dayCount;

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

        dayCount = -1;
    }

     void Update()
     {
        currentTime += Time.deltaTime;

        if (currentTime >= cycleDuration) 
        {
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
                break;
            
            case DayTime.Day:
                globalLight.color = Color.Lerp(day, sunset, percent);
                dayCount++;
                //if (dayCount == 3)
                    //SetActive победное окно
                break;
            
            case DayTime.Sunset:
                globalLight.color = Color.Lerp(sunset, night, percent);
                break;
            
            case DayTime.Night:
                globalLight.color = Color.Lerp(night, midnight, percent);        
                break;
            
            case DayTime.Midnight:
                globalLight.color = Color.Lerp(midnight, sunrise, percent);     
                break;
        }
     }
}