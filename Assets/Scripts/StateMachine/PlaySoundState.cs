using UnityEngine;
using System.Collections;

public class PlaySoundState : StateMachineBehaviour 
{
    public string m_SoundName = "";

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UISoundManager.PlaySound(m_SoundName);

        animator.SetTrigger("GotoNextState");
    }
}
