using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;
using UnityEngine.InputSystem;

public class ChooseSkillWithoutShooting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private InputAction attackInput;

    private void Start()
    {
		attackInput = GM.InputSystem.playerInput.actions["Weapon Attack"];
    }

    public void OnPointerEnter(PointerEventData eventData)
	{
		attackInput.Disable();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		attackInput.Enable();
	}

    public void OnDisable()
    {
		attackInput.Enable();
	}
}
