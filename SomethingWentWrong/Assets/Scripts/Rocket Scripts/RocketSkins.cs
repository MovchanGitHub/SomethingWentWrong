using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSkins : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public Sprite[] skins;
    public int[] thresholdsHP;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeSpriteAccordingToHP(int newHP)
    {
        int i = 0;
        while (i < thresholdsHP.Length && thresholdsHP[i] <= newHP) ++i;
        //Debug.Log("rocketSkin, i = " + i);
        _spriteRenderer.sprite = skins[i];
    }
}
