using UnityEngine;
using System.Collections;

public class DisplayTextSpeedsBlackboard : MonoBehaviour 
{
    public Animator m_Animator = null;

    public float m_SlowDisplaySpeed = 10.0f;
    public float m_MediumDisplaySpeed = 30.0f;
    public float m_FastDisplaySpeed = 60.0f;

    void Start()
    {
        if(m_Animator != null)
        {
            StateMachineBlackboard.AddFloat(m_Animator, DisplayTextBehaviourState._DisplaySpeedSlowBlackboardId, m_SlowDisplaySpeed);
            StateMachineBlackboard.AddFloat(m_Animator, DisplayTextBehaviourState._DisplaySpeedMediumBlackboardId, m_MediumDisplaySpeed);
            StateMachineBlackboard.AddFloat(m_Animator, DisplayTextBehaviourState._DisplaySpeedFastBlackboardId, m_FastDisplaySpeed);
        }
    }

    void OnValidate()
    {
        if(m_Animator != null)
        {
            StateMachineBlackboard.AddFloat(m_Animator, DisplayTextBehaviourState._DisplaySpeedSlowBlackboardId, m_SlowDisplaySpeed);
            StateMachineBlackboard.AddFloat(m_Animator, DisplayTextBehaviourState._DisplaySpeedMediumBlackboardId, m_MediumDisplaySpeed);
            StateMachineBlackboard.AddFloat(m_Animator, DisplayTextBehaviourState._DisplaySpeedFastBlackboardId, m_FastDisplaySpeed);
        }
    }

    void Update()
    {
        OnValidate();
    }
}
