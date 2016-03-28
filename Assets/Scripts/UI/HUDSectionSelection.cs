using UnityEngine;
using System.Collections;

public class HUDSectionSelection : MonoBehaviour 
{
    public HUDSection[] m_HUDSections = null;
    private HUDSection m_CurrentSelection = null;

    void Awake()
    {
        for(int i = 0; i < m_HUDSections.Length; i ++)
        {
            m_HUDSections[i].Initialize();
        }
    }

    void Update()
    {
        if (m_CurrentSelection != null)
        {
            if(m_CurrentSelection.m_State == HUDSectionState.Maximized)
            {
                if(!m_CurrentSelection.WantsFocus())
                {
                    m_CurrentSelection.Minimize();
                }
                else
                {
                    m_CurrentSelection.SubUpdate();
                }
            }
            else if(m_CurrentSelection.m_State == HUDSectionState.Minimized)
            {
                m_CurrentSelection = null;
            }
        }
        else
        {
            for(int i = 0; i < m_HUDSections.Length; i++)
            {
                if(m_HUDSections[i].WantsFocus())
                {
                    m_CurrentSelection = m_HUDSections[i];
                    m_CurrentSelection.Maximize();
                }
            }
        }
    }
}
