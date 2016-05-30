using UnityEngine;
using System.Collections;

public class FadeMusicVolumeBehaviourState : StateMachineBehaviour 
{
    public enum Fade
    {
        Low,
        Normal,
    }

    public Fade m_FadeTo = Fade.Normal;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch(m_FadeTo)
        {
            case Fade.Low:
                MusicManager.FadeMusicLow();
                break;

            case Fade.Normal:
                MusicManager.FadeMusicNormal();
                break;
        }        

        animator.SetTrigger("GotoNextState");
    }
}
