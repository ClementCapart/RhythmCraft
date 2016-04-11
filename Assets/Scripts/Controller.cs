using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class Controller : MonoBehaviour
{
	private static Controller m_instance = null;
	public static Controller Instance
	{
		get { return m_instance; }
	}

	private Direction m_currentDirection = Direction.None;
	public Direction ControlDirection
	{
		get { return m_currentDirection; }
	}

	public delegate void OnChangeDirectionDelegate(Direction direction);
	private static OnChangeDirectionDelegate s_onChangeDirection = null;
	public static OnChangeDirectionDelegate OnChangeDirection
	{
		get { return s_onChangeDirection; }
		set {  s_onChangeDirection = value; }
	}

	void Awake()
	{
		m_instance = this;
	}

	void Update()
	{
        if(HUDSectionSelection.HasSelection())
        {
            TryChangeDirection(Direction.None);
        }
        else if (XInput.GetButton(Buttons.LeftStickUp, 0) || Input.GetKey(KeyCode.UpArrow))
		{
			TryChangeDirection(Direction.Up);
		}
		else if (XInput.GetButton(Buttons.LeftStickRight, 0) || Input.GetKey(KeyCode.RightArrow))
		{
			TryChangeDirection(Direction.Right);
		}
		else if (XInput.GetButton(Buttons.LeftStickDown, 0) || Input.GetKey(KeyCode.DownArrow))
		{
			TryChangeDirection(Direction.Down);
		}
		else if (XInput.GetButton(Buttons.LeftStickLeft, 0) || Input.GetKey(KeyCode.LeftArrow))
		{
			TryChangeDirection(Direction.Left);
		}
		else
		{
			TryChangeDirection(Direction.None);
		}	
	}

    void TryChangeDirection(Direction newDirection)
	{
		if (m_currentDirection != newDirection)
		{
			ChangeDirection(newDirection);
		}	
	}

	void ChangeDirection(Direction newDirection)
	{     
		if (m_currentDirection != newDirection)
		{
			m_currentDirection = newDirection;
			OnChangeDirection(newDirection);
		}	
	}
}
