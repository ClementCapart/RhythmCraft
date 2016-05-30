using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    private bool m_locked = false;
    public bool IsLocked()
    {
        return m_locked;
    }

    public void Lock()
    {
        m_locked = true;
    }

    public void Unlock()
    {
        m_locked = false;
    }

    private static List<Controller> m_Controllers = new List<Controller>();

    public static void RegisterController(Controller controller)
    {
        if (!m_Controllers.Contains(controller))
        {
            m_Controllers.Add(controller);
        }
    }

    public static void UnregisterController(Controller controller)
    {
        if(m_Controllers.Contains(controller))
        {
            m_Controllers.Remove(controller);
        }
    }

    public static void LockAll()
    {
        for(int i = 0; i < m_Controllers.Count; i++)
        {
            m_Controllers[i].Lock();
        }
    }

    public static void UnlockAll()
    {
        for (int i = 0; i < m_Controllers.Count; i++)
        {
            m_Controllers[i].Unlock();
        }
    }
}
