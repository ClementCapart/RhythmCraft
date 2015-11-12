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
	Direction	m_direction	= Direction.None;
	TokenType	m_type		= TokenType.None;

    float		m_timeLeft	= float.MaxValue;

	public CraftToken(Direction direction, TokenType type, float timeLeft)
	{
		m_direction = direction;
		m_type = type;
		m_timeLeft = timeLeft;
	}

	public void SubUpdate()
	{
		m_timeLeft -= Time.deltaTime;
		if(m_timeLeft <= 0.0f)
		{
			CheckValidation();
		}
	}

	private void CheckValidation()
	{
		//If(Controller opposite Direction of direction
		{
			Validate();
		}
		//else
		{
			Destroy();
		}
	}

	private void Validate()
	{

	}

	private void Destroy()
	{

	}
}
