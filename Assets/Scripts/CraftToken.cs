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

public enum TokenState
{
    Invalid,
    Running,
    Success,
    Fail,
}

public class CraftToken
{
	const float TIME_TO_DISPLAY = 3.0f;

	Direction	m_direction	= Direction.None;
	//TokenType	m_type		= TokenType.None;

    float		m_timeLeft	= float.MaxValue;
	float		m_speed = 0.0f;

	public  TokenState m_State = TokenState.Invalid;
    private  int m_tokenIndex = -1;
    private bool m_isDirty = false;
	private GameObject m_TokenObject = null;
	private RectTransform m_TokenRectTransform = null;


	public delegate void OnTokenCheckDelegate(bool state);
	private static OnTokenCheckDelegate s_onTokenCheck = null;
	public static OnTokenCheckDelegate OnTokenCheck
	{
		get { return s_onTokenCheck; }
		set { s_onTokenCheck = value; }
	}

	public CraftToken(Direction direction, float timeLeft)
	{
        m_State = TokenState.Running;
		m_direction = direction;
		//m_type = type;
		m_timeLeft = timeLeft;
	}

	public TokenState SubUpdate()
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

        if(m_isDirty)
        {
            if(m_tokenIndex == 0)
            {
                InternalSetAsCurrent();
            }
            else if(m_tokenIndex == 1)
            {
                InternalSetAsNext();
            }
        }

        return m_State;
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
        m_State = passed ? TokenState.Success : TokenState.Fail;
		Destroy();
	}

	private void Destroy()
	{
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

    public void SetAsCurrent()
    {
        m_tokenIndex = 0;
        m_isDirty = true;
    }

    public void SetAsNext()
    {
        m_tokenIndex = 1;
        m_isDirty = true;
    }

    private void InternalSetAsCurrent()
    {        
        if (m_TokenObject)
        {
            UnityEngine.UI.Image image = m_TokenObject.GetComponent<UnityEngine.UI.Image>();
            if(image)
            {
                image.color = CraftTokenFolder.CurrentTokenColor;
            }
        }
    }

    private void InternalSetAsNext()
    {
        if (m_TokenObject)
        {
            UnityEngine.UI.Image image = m_TokenObject.GetComponent<UnityEngine.UI.Image>();
            if(image)
            {
                image.color = CraftTokenFolder.NextTokenColor; 
            }
        }   
    }
}
