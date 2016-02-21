using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{
	public static UIManager Instance
    {
        get { return m_instance; }
    }

    private static UIManager m_instance = null;

    public Canvas m_Canvas;
    public GameObject m_PopupFolder;

    void Start()
    {
        m_instance = this;
    }
}
