using UnityEngine;
using System.Collections;

public class HUDSectionSelection : Singleton<HUDSectionSelection>
{
    public HUDSection[] m_HUDSections = null;
    private HUDSection m_CurrentSelection = null;
    private bool m_Locked = false;

    public static bool HasSelection()
    {
        if(Instance && Instance.m_CurrentSelection != null)
            return true;

        return false;
    }

    public static bool IsCurrentSelection<T>() where T : HUDSection
    {
        if(HasSelection() && Instance.m_CurrentSelection is T)
        {
            return true;
        }

        return false;
    }

    public static HUDSectionState GetCurrentSelectionState()
    {
        if(HasSelection())
        {
            return Instance.m_CurrentSelection.m_State;
        }
        else
        {
            return HUDSectionState.NotInitialized;
        }
    }

    public static HUDSectionState GetHUDSectionState<T>() where T : HUDSection
    {
        if(Instance != null)
        {
            for(int i = 0; i < Instance.m_HUDSections.Length; i++)
            {
                if(Instance.m_HUDSections[i] is T)
                {
                    return Instance.m_HUDSections[i].m_State;
                }
            }
        }    
    
        return HUDSectionState.NotInitialized;
    }

    public static void LockSelection()
    {
        if(Instance)
            Instance.m_Locked = true;
    }

    public static void UnlockSelection()
    {
        if(Instance)
            Instance.m_Locked = false;
    }

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
                if(!m_CurrentSelection.WantsFocus() || m_Locked)
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
        else if(!m_Locked)
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
