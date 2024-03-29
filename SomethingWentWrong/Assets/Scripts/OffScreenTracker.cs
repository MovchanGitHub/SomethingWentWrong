using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenTracker : MonoBehaviour
{
    private Transform playerTransform;
    private float distanceFromOrigin;
    public GameObject core;
    public GameObject Arrow;

    void Start()
    {
        playerTransform = GameManager.GM.PlayerMovement.transform;
    }
    
    void Update()
    {
        if (playerTransform == null) Destroy(this);
        distanceFromOrigin = Vector3.Distance(playerTransform.position, core.transform.position);

        Vector3 targScale = Vector3.Lerp(new Vector3(0.1f,0.1f,0.1f), new Vector3(2f,2f,2f), distanceFromOrigin/ 15);
        Arrow.transform.localScale = targScale;

        Vector3 viewPos = Camera.main.WorldToViewportPoint(core.transform.position);
        if (!(viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0))
        {
            Arrow.SetActive(true);

            Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;

            Vector3 targetPositionScreenPoint = GameManager.GM.Camera.WorldToScreenPoint(core.transform.position);

            Vector3 dir = (targetPositionScreenPoint - screenCenter).normalized;

            float angle = Mathf.Atan2(dir.y, dir.x);

            transform.position = screenCenter + new Vector3(Mathf.Cos(angle) * screenCenter.x * 0.85f, Mathf.Sin(angle) * screenCenter.y * 0.85f, 0);

            transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        }
        else
        {
           Arrow.SetActive(false);
        }
    }
}
