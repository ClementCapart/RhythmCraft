using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CraftPatternPlayer : MonoBehaviour 
{
	List<CraftToken> m_currentTokens = new List<CraftToken>();
	
	public Animator m_Animator = null;

	void Start()
	{
		m_Animator.Play("StartPattern");
	}

	public void AddNewToken(AnimationEvent eventInfo)
	{
		Direction tokenDirection = (Direction)Enum.Parse(typeof(Direction), eventInfo.stringParameter);
		TokenType tokenType = (TokenType)eventInfo.intParameter;
		float delay = eventInfo.floatParameter;				

		m_currentTokens.Add(new CraftToken(tokenDirection, tokenType, delay));
	}

	void Update()
	{
		for (int i = m_currentTokens.Count - 1; i >= 0; i--)
		{
			m_currentTokens[i].SubUpdate();
			if(m_currentTokens[i].m_toBeDestroyed)
				m_currentTokens.RemoveAt(i);
		}
	}
}
