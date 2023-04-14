using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialRecourseDestroyTrigger : MonoBehaviour
{
    private void OnDestroy()
    {
        GameManager.GM.Tutorial.MainedResources++;
    }
}
