using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum CraftState
{
    Invalid,
    Success,
    Failure,
    NearSuccess,
}

public class CraftPatternPlayer : MonoBehaviour 
{
    public enum PlayerState
    {
        Stopped,
        Playing,
        Paused,
    }

    public enum SettingToTest
    {
        First,
        Second,
        Third,
        Fourth,
    }

    public SettingToTest m_SettingsToTest;

    public PlayerState m_State = PlayerState.Stopped;
    public float m_PlayerSpeed = 1.0f;
    public float m_NoteTimeOnScreen = 1.0f;
    public bool m_UseCurrentNextFeedback = false;

    public delegate void CraftSequenceStarted(ItemData item);
    public static CraftSequenceStarted s_craftSequenceStarted;

    public delegate void CraftSequenceEnded(ItemData item, CraftState state);
    public static CraftSequenceEnded s_craftSequenceEnded;

    void Start()
    {
    }

    void Reset()
    {
        m_State = PlayerState.Stopped;
    }

    void OnValidate()
    {
        SwitchSettings();
    }

    void SwitchSettings()
    {
        switch(m_SettingsToTest)
        {
            case SettingToTest.First:
                m_PlayerSpeed = 2.5f;
                m_NoteTimeOnScreen = 1.0f;
                m_UseCurrentNextFeedback = false;
                break;

            case SettingToTest.Second:
                m_PlayerSpeed = 2.5f;
                m_NoteTimeOnScreen = 2.0f;
                m_UseCurrentNextFeedback = true;
                break;

            case SettingToTest.Third:
                m_PlayerSpeed = 2.5f;
                m_NoteTimeOnScreen = 0.8f;
                m_UseCurrentNextFeedback = true;
                break;

            case SettingToTest.Fourth:
                m_PlayerSpeed = 2.0f;
                m_NoteTimeOnScreen = 1.5f;
                m_UseCurrentNextFeedback = true;
                break;
        }
    }

    public void StartPattern(ItemData item)
    {
        Reset();
        if(item != null)
        {
            StartCoroutine(PlayPattern(item));            
        }
    }

	public CraftToken CreateNewToken(CraftPattern.PatternNote note)
	{
		return new CraftToken(note.m_Direction, m_NoteTimeOnScreen);
	}

	IEnumerator PlayPattern(ItemData item)
	{        
        CraftPattern.PatternNote[] notes = item.m_CraftPattern.GetPattern();

        if(notes.Length == 0)
            yield break;

        List<CraftToken> currentTokens = new List<CraftToken>();
        int currentNote = 0;
        int successTokenCount = 0;
        float currentDelay = notes[currentNote].m_Delay;

        m_State = PlayerState.Playing;
        HUDSectionSelection.LockSelection();

        if(s_craftSequenceStarted != null)
        {
            s_craftSequenceStarted(item);
        }

        while(m_State == PlayerState.Playing)
        {    
            if(currentNote < notes.Length)
            {
                currentDelay -= Time.deltaTime * m_PlayerSpeed;

                if(currentDelay <= 0.0f)
                {
                    currentTokens.Add(CreateNewToken(notes[currentNote]));
                    
                    if(m_UseCurrentNextFeedback)
                    {
                        if(currentTokens.Count == 1)
                        {
                            currentTokens[0].SetAsCurrent();
                        }
                        else if(currentTokens.Count == 2)
                        {
                            currentTokens[1].SetAsNext();
                        }
                    }                    

                    currentNote++;
                    if(currentNote < notes.Length)
                    {
                        currentDelay += notes[currentNote].m_Delay;
                    }
                }
            }        
            else if(currentTokens.Count == 0)
            {
                m_State = PlayerState.Stopped;
            }

            for (int i = currentTokens.Count - 1; i >= 0; i--)
		    {
			    TokenState state = currentTokens[i].SubUpdate();
                if (state != TokenState.Running)
                {
                    if(state == TokenState.Success)
                        successTokenCount++;
                    currentTokens.RemoveAt(i);

                    if (m_UseCurrentNextFeedback)
                    {
                        if (currentTokens.Count >= 1)
                        {
                            currentTokens[0].SetAsCurrent();
                        }
                        if (currentTokens.Count >= 2)
                        {
                            currentTokens[1].SetAsNext();
                        }
                    }
                }
		    }

            yield return 0;          
        }
		
        CraftState endState = CheckEndCraftState(successTokenCount, notes.Length);

        ContextualPopupManager.CreateCraftResultPopup(item, endState);
        
        if(s_craftSequenceEnded != null)
        {
            s_craftSequenceEnded(item, endState);
        }        
        HUDSectionSelection.UnlockSelection();

        Reset();        
	}

    CraftState CheckEndCraftState(int successTokenCount, int totalTokenCount)
    {
        float successRate = (float)successTokenCount / (float)totalTokenCount; 
        
        Debug.Log("EndPattern - Success Rate: " + successRate * 100.0f + "%");        

        if(successRate >= 1.0f)
        {
            return CraftState.Success;
        }
        else if(successRate >= 0.8f)
        {
            return CraftState.NearSuccess;
        }

        return CraftState.Failure;
    }
}
