using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class InputSystem : MonoBehaviour
{
    public PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerInput.actions["Sprint"].started += GM.PlayerMovement.Run;
        playerInput.actions["Sprint"].canceled += GM.PlayerMovement.Walk;

        playerInput.actions["Dash"].started += GM.PlayerMovement.Rush;

        playerInput.actions["Fists Attack"].started += GM.PlayerMovement.GetComponentInChildren<AttackPoint>().TryAttack;

        playerInput.actions["Change Weapon"].started += GM.PlayerMovement.GetComponentInChildren<PlayerWeaponScript>().ChangeWeapon;
        playerInput.actions["Weapon Attack"].started += GM.PlayerMovement.GetComponentInChildren<PlayerWeaponScript>().Attack;
        playerInput.actions["Weapon Attack"].canceled += GM.PlayerMovement.GetComponentInChildren<PlayerWeaponScript>().StopAttack;

        playerInput.actions["Open Inventory"].started += GM.InventoryManager.OpenCloseInventory;
        playerInput.actions["Open Encyclopedia"].started += GM.UI.Encyclopedia.OpenCloseEncyclopedia;
        playerInput.actions["Open InGame Menu"].started += GM.UI.GetComponent<InGameMenuScript>().EscapeIsPressed;
    }


    public void BlockPlayerInputs()
    {
        //playerInput.actions["Move"].Disable();
        playerInput.actions["Sprint"].Disable();
        playerInput.actions["Dash"].Disable();
        playerInput.actions["Fists Attack"].Disable();
        playerInput.actions["Change Weapon"].Disable();
        playerInput.actions["Weapon Attack"].Disable();
    }

    public void UnblockPlayerInputs()
    {
        //playerInput.actions["Move"].Disable();
        playerInput.actions["Sprint"].Enable();
        playerInput.actions["Dash"].Enable();
        playerInput.actions["Fists Attack"].Enable();
        playerInput.actions["Change Weapon"].Enable();
        playerInput.actions["Weapon Attack"].Enable();
    }

}
