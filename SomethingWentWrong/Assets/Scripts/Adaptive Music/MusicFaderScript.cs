using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFaderScript : MonoBehaviour
{
    protected void MusicVolumeDownRoot(AudioSource audioIn, int transTime, ref float volumeAuIn)
    {
        audioIn.volume = MathF.Sqrt(volumeAuIn);
        volumeAuIn -= volumeAuIn / transTime;
        if (MathF.Sqrt(volumeAuIn) < 0.01f)
        {
            audioIn.volume = 0;
            volumeAuIn = 0;
        }
    }

    protected void MusicVolumeUpRoot(AudioSource audioIn, int transTime, ref float volumeAuIn)
    {
        if (volumeAuIn == 0) volumeAuIn += 0.0001f;
        audioIn.volume = MathF.Sqrt(volumeAuIn);
        volumeAuIn += volumeAuIn / transTime;
        if (MathF.Sqrt(volumeAuIn) > 0.99f)
        {
            audioIn.volume = 1;
            volumeAuIn = 1;
        }
    }
    
    protected void MusicVolumeDownLinear(AudioSource audioIn, int transTime, ref float volumeAuIn)
    {
        audioIn.volume = volumeAuIn;
        volumeAuIn -= 1f / transTime;
        if (volumeAuIn < 0.001f)
        {
            audioIn.volume = 0;
            volumeAuIn = 0;
        }
    }

    protected void MusicVolumeUpLinear(AudioSource audioIn, int transTime, ref float volumeAuIn)
    {
        audioIn.volume = volumeAuIn;
        volumeAuIn += 1f / transTime;
        if (volumeAuIn > 0.999f)
        {
            audioIn.volume = 1;
            volumeAuIn = 1;
        }
    }

    protected void MusicVolumeDownSqr(AudioSource audioIn, int transTime, ref float volumeAuIn)
    {
        audioIn.volume = volumeAuIn * volumeAuIn;
        volumeAuIn -= 1f / transTime;
        if (volumeAuIn * volumeAuIn < 0.001f)
        {
            audioIn.volume = 0;
            volumeAuIn = 0;
        }
    }
    
    protected void MusicVolumeUpSqr(AudioSource audioIn, int transTime, ref float volumeAuIn)
    {
        audioIn.volume = volumeAuIn * volumeAuIn;
        volumeAuIn += 1f / transTime;
        if (volumeAuIn * volumeAuIn > 0.999f)
        {
            audioIn.volume = 1;
            volumeAuIn = 1;
        }
    }
}
