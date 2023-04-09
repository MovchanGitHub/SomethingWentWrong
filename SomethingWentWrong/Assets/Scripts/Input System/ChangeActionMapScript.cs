using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeActionMapScript : MonoBehaviour
{
    private string[] actionMapsNames = new string[] { "Usual", "Alternative" };
    private int curMapInd = 0;

    private void ChangeActionMap()
    {
        curMapInd = (curMapInd + 1) % 2;
        GameManager.GM.InputSystem.playerInput.SwitchCurrentActionMap(actionMapsNames[curMapInd]);
    }
}
