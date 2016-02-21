using UnityEngine;
using System.Collections;

public class PatternDoneState : StateMachineBehaviour 
{
    CraftPatternPlayer m_patternPlayer = null;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_patternPlayer = animator.GetComponent<CraftPatternPlayer>();
        m_patternPlayer.SignalEnd();
    }
}
