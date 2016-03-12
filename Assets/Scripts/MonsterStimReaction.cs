using UnityEngine;
using System.Collections;

public class MonsterStimReaction : MonoBehaviour
{
    public Animator m_MonsterAnimator = null;

    void Start()
    {
        StimMagic.SubscribeToStim(StimMagicReaction);
        StimEntertainment.SubscribeToStim(StimEntertainmentReaction);
    }

    void StimMagicReaction(StimMagic stim)
    {
        Debug.Log("Magic Stim! (" + gameObject.name + ")");
        if(m_MonsterAnimator != null)
        {
            m_MonsterAnimator.SetTrigger("MagicTrigger");
        }
    }

    void StimEntertainmentReaction(StimEntertainment stim)
    {
        Debug.Log("Entertainment Stim! (" + gameObject.name + ")");
        if(m_MonsterAnimator != null)
        {
            m_MonsterAnimator.SetTrigger("MagicTrigger");
        }
    }
}
