using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShaderLogic : MonoBehaviour
{
    [SerializeField] private List<Renderer> _renderers;
    private List<MaterialPropertyBlock> _propBlocks;

    private float fade = 1f;
    
    private void Start()
    {
        _propBlocks = new List<MaterialPropertyBlock>();
        for (int i = 0; i < _renderers.Count; ++i)
            _propBlocks.Add(new MaterialPropertyBlock());
    }

    public void EnemyLaserDieShader()
    {
        StartCoroutine(FadeDecreasing(0.015f));
    }
    
    public void EnemyBombDieShader()
    {
        for (int i = 0; i < _renderers.Count; ++i)
        {
            _renderers[i].GetPropertyBlock(_propBlocks[i]);
            _propBlocks[i].SetInteger("_BombDeath", 1);
            _renderers[i].SetPropertyBlock(_propBlocks[i]);
        }
        
        StartCoroutine(FadeDecreasing(0.04f));
    }

    public void ChangeDissolvingColor(Color color)
    {
        for (int i = 0; i < _renderers.Count; ++i)
        {
            _renderers[i].GetPropertyBlock(_propBlocks[i]);
            _propBlocks[i].SetColor("_BorderColor", color);
            _renderers[i].SetPropertyBlock(_propBlocks[i]);
        }
    }

    private IEnumerator FadeDecreasing(float fadeDecrement)
    {
        while (fade > 0f)
        {
            fade -= fadeDecrement;
            
            for (int i = 0; i < _renderers.Count; ++i)
            {
                _renderers[i].GetPropertyBlock(_propBlocks[i]);
                _propBlocks[i].SetFloat("_Fade", fade);
                _renderers[i].SetPropertyBlock(_propBlocks[i]);
            }
            
            yield return new WaitForSeconds(0.05f);
        }
    }
}
