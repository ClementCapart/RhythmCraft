using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance;
	
    public static T Instance
    {
        get
        {
            if(m_instance)
                return m_instance;

            m_instance = (T)FindObjectOfType(typeof(T));

            if(m_instance)
                return m_instance;

            m_instance = CreateSingleton();

            return m_instance;
        }
    }

    private static T CreateSingleton()
    {
        GameObject singleton = new GameObject();
        singleton.name = typeof(T).ToString();
        
        Debug.LogWarning("Singleton " + singleton.name + " has been created. But it shouldn't, please add it to the scene.");

        return singleton.AddComponent<T>();
    }

}
