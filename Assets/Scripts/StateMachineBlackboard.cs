using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StateMachineBlackboard
{
    private static StateMachineBlackboard m_instance;
    private static StateMachineBlackboard Instance
    {
        get
        {
            if(m_instance == null)
                m_instance = new StateMachineBlackboard();

            return m_instance;
        }
    }    

    public static Blackboard CreateNewBlackboard(Animator animator)
    {
        if(Instance != null)
        {
            Blackboard newBlackboard = new Blackboard();
            Instance.m_blackboards.Add(animator, newBlackboard);
            Instance.m_cachedBlackboard.Cache(animator, newBlackboard);
            return newBlackboard;
        }

        return null;
    }

    public static Blackboard GetBlackboard(Animator animator, bool andCache = true, bool createIfNeeded = true)
    {
        if(Instance != null)
        {
            if(Instance.m_cachedBlackboard.IsCaching(animator))
            {
                return Instance.m_cachedBlackboard.m_Blackboard;
            }

            Blackboard outBoard = null;
            if(Instance.m_blackboards.TryGetValue(animator, out outBoard))
            {
                if(andCache) Instance.m_cachedBlackboard.Cache(animator, outBoard);
                return outBoard;
            }
            else if(createIfNeeded)
            {
                return CreateNewBlackboard(animator);
            }
        }

        return null;
    }

    public static void AddInt(Animator animator, string key, int value)
    {
        if(Instance != null)
        {
            Blackboard board = GetBlackboard(animator);
            if(board != null)
            {
                board.AddInt(key, value);
            }
        }
    }

    public static void AddFloat(Animator animator, string key, float value)
    {
        if (Instance != null)
        {
            Blackboard board = GetBlackboard(animator);
            if (board != null)
            {
                board.AddFloat(key, value);
            }
        }
    }

    public static void AddString(Animator animator, string key, string value)
    {
        if (Instance != null)
        {
            Blackboard board = GetBlackboard(animator);
            if (board != null)
            {
                board.AddString(key, value);
            }
        }
    }

    public static void AddObject(Animator animator, string key, object value)
    {
        if (Instance != null)
        {
            Blackboard board = GetBlackboard(animator);
            if (board != null)
            {
                board.AddObject(key, value);
            }
        }
    }

    public static bool GetInt(Animator animator, string key, out int value)
    {
        if(Instance != null)
        {
            Blackboard blackboard = GetBlackboard(animator);
            if(blackboard != null)
            {
                if(blackboard.GetInt(key, out value))
                {
                    return true;
                }
            }
        }

        value = -1;
        return false;
    }

    public static bool GetFloat(Animator animator, string key, out float value)
    {
        if (Instance != null)
        {
            Blackboard blackboard = GetBlackboard(animator);
            if (blackboard != null)
            {
                if (blackboard.GetFloat(key, out value))
                {
                    return true;
                }
            }
        }

        value = -1.0f;
        return false;
    }

    public static bool GetString(Animator animator, string key, out string value)
    {
        if (Instance != null)
        {
            Blackboard blackboard = GetBlackboard(animator);
            if (blackboard != null)
            {
                if (blackboard.GetString(key, out value))
                {
                    return true;
                }
            }
        }

        value = "";
        return false;
    }

    public static bool Get<T>(Animator animator, string key, out T value)
    {
        if (Instance != null)
        {
            Blackboard blackboard = GetBlackboard(animator);
            if (blackboard != null)
            {
                if (blackboard.Get<T>(key, out value))
                {
                    return true;
                }
            }
        }

        value = default(T);
        return false;
    }

    public static void ClearBlackboard(Animator animator)
    {
        if(Instance != null)
        {
            Blackboard blackboard = null;
            if(Instance.m_blackboards.TryGetValue(animator, out blackboard))
            {
                blackboard.CleanAll();
                Instance.m_blackboards.Remove(animator);
            }
        }
    }

    private Dictionary<Animator, Blackboard> m_blackboards = new Dictionary<Animator,Blackboard>();
    
    private class CachedBlackboard
    {
        public Animator m_Animator;
        public Blackboard m_Blackboard;

        public bool IsCaching(Animator animator)
        {
            return (animator == m_Animator);
        }

        public void Cache(Animator animator, Blackboard blackboard)
        {
            m_Animator = animator;
            m_Blackboard = blackboard;
        }
    }

    private CachedBlackboard m_cachedBlackboard = new CachedBlackboard();
}

public class Blackboard
{
    private Dictionary<string, int> m_intBoard = new Dictionary<string, int>();
    private Dictionary<string, float> m_floatBoard = new Dictionary<string, float>();
    private Dictionary<string, string> m_stringBoard = new Dictionary<string, string>();
    private Dictionary<string, object> m_objectBoard = new Dictionary<string, object>();

    public void CleanAll()
    {
        m_intBoard.Clear();
        m_floatBoard.Clear();
        m_stringBoard.Clear();
        m_objectBoard.Clear();
    }

    public void AddInt(string key, int value)
    {
        if(m_intBoard.ContainsKey(key))
        {
            m_intBoard[key] = value;
        }
        else
        {
            m_intBoard.Add(key, value);
        }
    }

    public void AddFloat(string key, float value)
    {
        if (m_floatBoard.ContainsKey(key))
        {
            m_floatBoard[key] = value;
        }
        else
        {
            m_floatBoard.Add(key, value);
        }
    }

    public void AddString(string key, string value)
    {
        if (m_stringBoard.ContainsKey(key))
        {
            m_stringBoard[key] = value;
        }
        else
        {
            m_stringBoard.Add(key, value);
        }
    }

    public void AddObject(string key, object value)
    {
        if (m_objectBoard.ContainsKey(key))
        {
            m_objectBoard[key] = value;
        }
        else
        {
            m_objectBoard.Add(key, value);
        }
    }

    public bool GetInt(string key, out int value)
    {
        if(m_intBoard.TryGetValue(key, out value))
        {
            return true;
        }

        return false;
    }

    public bool GetFloat(string key, out float value)
    {
        if (m_floatBoard.TryGetValue(key, out value))
        {
            return true;
        }

        return false;
    }

    public bool GetString(string key, out string value)
    {
        if (m_stringBoard.TryGetValue(key, out value))
        {
            return true;
        }

        return false;
    }

    public bool Get<T>(string key, out T value)
    {
        object obj;
        if (m_objectBoard.TryGetValue(key, out obj))
        {
            if(obj.GetType().Equals(typeof(T)))
            {
                value = (T)obj;
                return true;
            }            
        }

        value = default(T);
        return false;
    }
}
