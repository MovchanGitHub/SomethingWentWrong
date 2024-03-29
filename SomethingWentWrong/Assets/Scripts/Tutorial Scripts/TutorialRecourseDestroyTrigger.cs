using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialRecourseDestroyTrigger : MonoBehaviour
{
    [SerializeField] private int weaponIndex;
    
    private void OnDestroy()
    {
        GameManager.GM.Tutorial.MainedResources++;
        GameManager.GM.Tutorial.ExplainWeapon(weaponIndex);
    }
}
