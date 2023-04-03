using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class OnHighlightVar2 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GM.UI.SkillsMenu.GetComponentInParent<SkillsScript>().OnHighLight(1);
    }
}
