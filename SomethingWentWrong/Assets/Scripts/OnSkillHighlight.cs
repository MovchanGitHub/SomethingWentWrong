using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManager;

public class OnSkillHighlight : StateMachineBehaviour {
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
        GM.UI.SkillsMenu.GetComponentInParent<SkillsScript>().OnHighLight(int.Parse(animator.name[^1].ToString()));
}
