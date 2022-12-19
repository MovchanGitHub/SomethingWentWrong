using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RetroMaskScript : MonoBehaviour
{
    [SerializeField] private float timeToInc;
    [SerializeField] private float rateStep;
    [SerializeField] private float maxY;

    private Vector3 maxScale;
    private float waitTime;
    
    private void Start()
    {
        maxScale = new Vector3(maxY * 2, maxY, 0);
        waitTime = timeToInc * rateStep;
    }

    public IEnumerator Increase()
    {
        float rate = 0;
        while (transform.localScale.y < maxY)
        {
            rate += rateStep;
            transform.localScale = Vector3.Lerp(Vector3.zero, maxScale, rate);
            yield return new WaitForSeconds(waitTime);
        }
    }
    
    public IEnumerator Decrease()
    {
        float rate = 0;
        while (transform.localScale.y > 0)
        {
            rate += rateStep;
            transform.localScale = Vector3.Lerp(maxScale, Vector3.zero, rate);
            yield return new WaitForSeconds(waitTime);
        }
        
        transform.localScale = Vector3.zero;
    }
}
