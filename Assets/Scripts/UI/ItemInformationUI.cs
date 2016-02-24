﻿using UnityEngine;
using System.Collections;

public class ItemInformationUI : MonoBehaviour 
{
    public UnityEngine.UI.Text m_Text = null;

	void Start()
    {
        if(m_Text != null)
        {
            m_Text.text = "";
        }

        CraftPatternPlayer.s_craftSequenceStarted += OnStartSequence;
        CraftPatternPlayer.s_craftSequenceEnded += OnEndSequence;
    }

    void OnDestroy()
    {
        CraftPatternPlayer.s_craftSequenceStarted -= OnStartSequence;
        CraftPatternPlayer.s_craftSequenceEnded -= OnEndSequence;
    }

    void OnStartSequence(ItemData item)
    {
        if(m_Text != null)
        {
            m_Text.text = item.m_Name;
        }
    }

    void OnEndSequence(ItemData item, CraftState state)
    {
        if(m_Text != null)
        {
            m_Text.text = "";
        }
    }
}