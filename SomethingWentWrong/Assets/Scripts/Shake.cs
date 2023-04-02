using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Animator camAnim;

    public void CamShake()
    {
        camAnim.enabled = true;
        camAnim.SetTrigger("shake");
        StartCoroutine(StopShake());
    }

    private IEnumerator StopShake()
    {
        yield return new WaitForSeconds(0.25f);
        camAnim.enabled = false;
    }
}

