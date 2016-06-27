using UnityEngine;
using System.Collections;

public enum TextDisplaySpeed
{
    Slow,
    Medium,
    Fast,
    Instant
}

public class DisplayTextBehaviourState : StateMachineBehaviour 
{
    [TextArea(3,10)]
    public string  m_TextToDisplay = "";
    
    public TextDisplaySpeed m_DisplaySpeed = TextDisplaySpeed.Medium;
    public string m_speechBubbleBlackboardId = "SpeechBubble";

    public const string _DisplaySpeedSlowBlackboardId = "TextSlowSpeed";
    public const string _DisplaySpeedMediumBlackboardId = "TextMediumSpeed";
    public const string _DisplaySpeedFastBlackboardId = "TextFastSpeed";   
 
    private float m_displaySpeedPicked = 0.0f;

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

        switch(m_DisplaySpeed)
        {
            case TextDisplaySpeed.Slow:
                if(!StateMachineBlackboard.GetFloat(animator, _DisplaySpeedSlowBlackboardId, out m_displaySpeedPicked))
                {
                    m_displaySpeedPicked = 0.0f;
                    Debug.LogWarning("StateMachine: Displaying Text with no Display Speed stored in Blackboard, using Instant instead");
                }
                break;

            case TextDisplaySpeed.Medium:
                if(!StateMachineBlackboard.GetFloat(animator, _DisplaySpeedMediumBlackboardId, out m_displaySpeedPicked))
                {
                    m_displaySpeedPicked = 0.0f;
                    Debug.LogWarning("StateMachine: Displaying Text with no Display Speed stored in Blackboard, using Instant instead");
                }
                break;

            case TextDisplaySpeed.Fast:
                if(!StateMachineBlackboard.GetFloat(animator, _DisplaySpeedFastBlackboardId, out m_displaySpeedPicked))
                {
                    m_displaySpeedPicked = 0.0f;
                    Debug.LogWarning("StateMachine: Displaying Text with no Display Speed stored in Blackboard, using Instant instead");
                }
                break;

            case TextDisplaySpeed.Instant:
                m_displaySpeedPicked = 0.0f;
                break;
        }

        m_speechUIElement.DisplayText(m_TextToDisplay, m_displaySpeedPicked);
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
