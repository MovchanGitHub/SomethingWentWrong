using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyDestroyTrigger : MonoBehaviour
{
    private void OnDestroy()
    {
        GameManager.GM.Tutorial.KilledEnemies++;
    }
}
