using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShaderLogic : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    private bool upgradeColorId;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    private float timePassed = 0f;
    [SerializeField] private float upgradeTime = 0.75f;
    [SerializeField] private float redTime = 0.5f;
    private int damageNumber = 0;
    private float r, g, b;

    public IEnumerator Upgrade()
    {
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetInt("_Upgrade", 1);
        _renderer.SetPropertyBlock(_propBlock);

        timePassed = 0f;
        while (timePassed < upgradeTime)
        {
            timePassed += 0.1f;
            r = 3 * UnityEngine.Random.Range(0.5f, 0.75f);
            g = 3 * UnityEngine.Random.Range(0.5f, 0.75f);
            b = 3 * UnityEngine.Random.Range(0.5f, 0.75f);
            
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_UpgradeColor", new Color(r, g, b));
            _renderer.SetPropertyBlock(_propBlock);
            
            yield return new WaitForSeconds(0.1f);
        }
        
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_UpgradeColor", new Color(255, 255, 255));
        _propBlock.SetInt("_Upgrade", 0);
        _renderer.SetPropertyBlock(_propBlock);
    }

    public IEnumerator BecomeRed() {
        damageNumber++;
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetInt("_GetDamage", 1);
        _renderer.SetPropertyBlock(_propBlock);

        yield return new WaitForSeconds(redTime);

        damageNumber--;
        if (damageNumber == 0)
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetInt("_GetDamage", 0);
            _renderer.SetPropertyBlock(_propBlock);
        }
    }
}
