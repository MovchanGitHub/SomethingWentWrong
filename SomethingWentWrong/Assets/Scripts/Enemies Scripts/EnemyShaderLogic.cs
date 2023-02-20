using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShaderLogic : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    private float fade = 1f;
    
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    public void EnemyLaserDieShader()
    {
        StartCoroutine(FadeDecreasing());
    }

    public void ChangeDissolvingColor(Color color)
    {
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_BorderColor", color);
        _renderer.SetPropertyBlock(_propBlock);
    }

    private IEnumerator FadeDecreasing()
    {
        while (fade > 0f)
        {
            fade -= 0.015f;
            
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat("_Fade", fade);
            _renderer.SetPropertyBlock(_propBlock);
            
            yield return new WaitForSeconds(0.05f);
        }
    }
}
