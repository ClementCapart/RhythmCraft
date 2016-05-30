using UnityEngine;
using System.Collections;

public class DisplayTextBehaviourState : StateMachineBehaviour 
{
    [TextArea(3,10)]
    public string  m_TextToDisplay = "";
    public float m_DisplaySpeed = 50.0f;
    public string m_speechBubbleBlackboardId = "SpeechBubble";

    public Buttons m_ButtonToPress = Buttons.A;
    public bool m_DestroyBubbleOnEnd = false;

    private SpeechUIElement m_speechUIElement;

    private SpeechUIElement CreateSpeechBubble()
    {
        return ContextualPopupManager.CreateSpeechBubble();
    }

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if(!StateMachineBlackboard.Get<SpeechUIElement>(animator, m_speechBubbleBlackboardId, out m_speechUIElement))
        {
            m_speechUIElement = CreateSpeechBubble();
            StateMachineBlackboard.AddObject(animator, m_speechBubbleBlackboardId, m_speechUIElement);
        }

        m_speechUIElement.DisplayText(m_TextToDisplay, m_DisplaySpeed);
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if(m_speechUIElement.GetState() == SpeechUIElement.State.TextDisplayed)
        {
            if(m_ButtonToPress == Buttons.None)
            {
                animator.SetTrigger("GotoNextState");
            }
            else
            {
                if (XInput.GetButtonDown(m_ButtonToPress, 0))
                {
                    animator.SetTrigger("GotoNextState");
                }
            }
        }
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
	    if(m_DestroyBubbleOnEnd)
        {
            m_speechUIElement.Destroy();
            StateMachineBlackboard.RemoveObject(animator, m_speechBubbleBlackboardId);
        }
	}
}
