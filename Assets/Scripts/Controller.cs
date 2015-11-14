using System;
using System.Collections;
using UnityEngine;


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
		if (XInput.GetButton(Buttons.LeftStickUp, 0))
		{
			ChangeDirection(Direction.Up);
		}
		else if (XInput.GetButton(Buttons.LeftStickRight, 0))
		{
			ChangeDirection(Direction.Right);
		}
		else if (XInput.GetButton(Buttons.LeftStickDown, 0))
		{
			ChangeDirection(Direction.Down);
		}
		else if (XInput.GetButton(Buttons.LeftStickLeft, 0))
		{
			ChangeDirection(Direction.Left);
		}
		else
		{
			ChangeDirection(Direction.None);
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
