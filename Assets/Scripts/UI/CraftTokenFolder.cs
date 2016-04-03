using UnityEngine;
using System.Collections;

public class CraftTokenFolder : MonoBehaviour 
{
	private static CraftTokenFolder m_instance = null;
	
	public GameObject m_TokenPrefab = null;

	public Transform m_UpLane = null;
	public Transform m_RightLane = null;
	public Transform m_DownLane = null;
	public Transform m_LeftLane = null;

    public Color m_CurrentTokenColor = Color.blue;
    public static Color CurrentTokenColor { get { return m_instance.m_CurrentTokenColor; } }
    public Color m_NextTokenColor = Color.green;  
    public static Color NextTokenColor { get {return m_instance.m_NextTokenColor; } }

	public static GameObject GetTokenPrefab()
	{
		if(m_instance != null)
			return m_instance.m_TokenPrefab;
		else
			return null;
	}

	public static Transform GetFolder()
	{
		if(m_instance != null)
			return m_instance.transform;
		else
			return null;			
	}

	public static Transform GetLane(Direction direction)
	{
		if(m_instance == null) return null;

		switch (direction)
		{
			case Direction.Up:
				return m_instance.m_UpLane;

			case Direction.Right:
				return m_instance.m_RightLane;

			case Direction.Down:
				return m_instance.m_DownLane;

			case Direction.Left:
				return m_instance.m_LeftLane;
		}

		return null;
	}

	void Awake()
	{
		m_instance = this;
	}
}
