using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenTracker : MonoBehaviour
{
    public GameObject core;

    void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width, Screen.height,0) /2;

        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(core.transform.position);

        Vector3 dir = (targetPositionScreenPoint - screenCenter).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x);

        transform.position = screenCenter + new Vector3(Mathf.Cos(angle) * screenCenter.x * 0.85f, Mathf.Sin(angle) * screenCenter.y * 0.85f, 0);

        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }
}
