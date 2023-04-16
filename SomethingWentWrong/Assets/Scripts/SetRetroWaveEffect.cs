using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRetroWaveEffect : MonoBehaviour
{
    public Material landscapeMaterial;

    private bool retroWaveEffect = true;
    
    public bool RetroWaveEffect
    {
        set
        {
            retroWaveEffect = value;
            int n = retroWaveEffect ? 1 : 0;
            landscapeMaterial.SetInt("_RetroWaveEffect", n);
        }
    }

    public void ChangeRetroWaveEffect()
    {
        RetroWaveEffect = !retroWaveEffect;
    }
}
