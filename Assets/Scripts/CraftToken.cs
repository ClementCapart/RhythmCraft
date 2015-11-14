using UnityEngine;
using System.Collections;

public enum Direction
{
	None,
	Up, 
	Right,
	Down,
	Left
}

public enum TokenType
{
	None,
	Progress,
	Quality,
	Durability
}

public class CraftToken
{
	const float TIME_TO_DISPLAY = 3.0f;

	Direction	m_direction	= Direction.None;
	TokenType	m_type		= TokenType.None;

    float		m_timeLeft	= float.MaxValue;
	float		m_speed = 0.0f;

	public bool m_toBeDestroyed = false;
	private GameObject m_TokenObject = null;
	private RectTransform m_TokenRectTransform = null;


	public delegate void OnTokenCheckDelegate(bool state);
	private static OnTokenCheckDelegate s_onTokenCheck = null;
	public static OnTokenCheckDelegate OnTokenCheck
	{
		get { return s_onTokenCheck; }
		set { s_onTokenCheck = value; }
	}

	public CraftToken(Direction direction, TokenType type, float timeLeft)
	{
		m_direction = direction;
		m_type = type;
		m_timeLeft = timeLeft;
	}

	public void SubUpdate()
	{
		m_timeLeft -= Time.deltaTime;
		
		if (m_timeLeft <= TIME_TO_DISPLAY)
		{
			if(m_TokenObject == null) CreateAndSetupToken();

			m_TokenRectTransform.anchoredPosition += new Vector2(Time.deltaTime * m_speed, 0.0f);
		}
		
		if(m_timeLeft <= 0.0f)
		{
			CheckValidation();
		}
	}

	private void CheckValidation()
	{
		if(Controller.Instance.ControlDirection == m_direction)
		{
			Validate(true);
		}
		else
		{
			Validate(false);
		}
	}

	private void Validate(bool passed)
	{
		OnTokenCheck(passed);
		Destroy();
	}

	private void Destroy()
	{
		m_toBeDestroyed = true;
		GameObject.Destroy(m_TokenObject);
	}

	private void CreateAndSetupToken()
	{
		m_TokenObject = GameObject.Instantiate(CraftTokenFolder.GetTokenPrefab(), Vector3.zero, Quaternion.identity) as GameObject;
		m_TokenObject.transform.SetParent(CraftTokenFolder.GetLane(m_direction), false);

		m_TokenRectTransform = m_TokenObject.GetComponent<RectTransform>();	
		RectTransform laneRect = m_TokenRectTransform.parent.transform.GetComponent<RectTransform>();

		m_speed = laneRect.rect.width / m_timeLeft;
	}
}
