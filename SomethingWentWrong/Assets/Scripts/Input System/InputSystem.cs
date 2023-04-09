using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static GameManager;

public class InputSystem : MonoBehaviour
{
    public PlayerInput playerInput;

    public InputAction openInventoryInput;
    public InputAction openEncyclopediaInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        openInventoryInput = playerInput.actions["Open Inventory"];
        //openEncyclopediaInput = playerInput.actions["Open Encyclopedia"];
    }

    private Dictionary<string, Action<InputAction.CallbackContext>> inputActions = new ();

    private void Start()
    {
        inputActions["PlayerRun"] = GM.PlayerMovement.Run;
        inputActions["PlayerWalk"] = GM.PlayerMovement.Walk;
        inputActions["PlayerRush"] = GM.PlayerMovement.Rush;
        inputActions["PlayerAttack"] = GM.PlayerMovement.GetComponentInChildren<AttackPoint>().TryAttack;
        inputActions["PlayerChangeWeapon"] = GM.PlayerMovement.GetComponentInChildren<PlayerWeaponScript>().ChangeWeapon;
        inputActions["PlayerShoot"] = GM.PlayerMovement.GetComponentInChildren<PlayerWeaponScript>().Attack;
        inputActions["PlayerStopShooting"] = GM.PlayerMovement.GetComponentInChildren<PlayerWeaponScript>().StopAttack;
        inputActions["InventoryClose"] = GM.InventoryManager.OpenCloseInventory;
        //inputActions["EncyclopediaOpen"] = GM.UI.Encyclopedia.OpenCloseEncyclopedia;
        inputActions["MenuOpen"] = GM.UI.GetComponent<InGameMenuScript>().EscapeIsPressed;
        
        playerInput.actions["Sprint"].started += inputActions["PlayerRun"];
        playerInput.actions["Sprint"].canceled += inputActions["PlayerWalk"];
        playerInput.actions["Dash"].started += inputActions["PlayerRush"];
        playerInput.actions["Fists Attack"].started += inputActions["PlayerAttack"];
        playerInput.actions["Change Weapon"].started += inputActions["PlayerChangeWeapon"];
        playerInput.actions["Weapon Attack"].started += inputActions["PlayerShoot"];
        playerInput.actions["Weapon Attack"].canceled += inputActions["PlayerStopShooting"];
        playerInput.actions["Open Inventory"].started += inputActions["InventoryClose"];
        //playerInput.actions["Open Encyclopedia"].started += inputActions["EncyclopediaOpen"];
        playerInput.actions["Open InGame Menu"].started += inputActions["MenuOpen"];
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

    public void OnDestroy()
    {
        playerInput.actions["Sprint"].started -= inputActions["PlayerRun"];
        playerInput.actions["Sprint"].canceled -= inputActions["PlayerWalk"];
        
        playerInput.actions["Dash"].started -= inputActions["PlayerRush"];
        
        playerInput.actions["Fists Attack"].started -= inputActions["PlayerAttack"];
        
        playerInput.actions["Change Weapon"].started -= inputActions["PlayerChangeWeapon"];
        playerInput.actions["Weapon Attack"].started -= inputActions["PlayerShoot"];
        playerInput.actions["Weapon Attack"].canceled -= inputActions["PlayerStopShooting"];
        
        playerInput.actions["Open Inventory"].started -= inputActions["InventoryClose"];
        //playerInput.actions["Open Encyclopedia"].started -= inputActions["EncyclopediaOpen"];
        playerInput.actions["Open InGame Menu"].started -= inputActions["MenuOpen"];
    }
}
