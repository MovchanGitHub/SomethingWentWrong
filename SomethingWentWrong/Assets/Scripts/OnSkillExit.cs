using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class OnSkillExit : StateMachineBehaviour {
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
        GM.UI.SkillsMenu.GetComponentInParent<SkillsScript>().OnStateExit(int.Parse(animator.name[^1].ToString()));
}