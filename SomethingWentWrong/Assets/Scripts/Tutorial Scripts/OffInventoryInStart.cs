using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffInventoryInStart : MonoBehaviour
{
    void Start()
    {
        GameManager.GM.InputSystem.openInventoryInput.Disable();
    }
}
