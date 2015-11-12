using System;
using System.Collections;
using UnityEngine;


public class Controller : MonoBehaviour
{
	private Controller m_instance = null;

	private Direction m_currentDirection = Direction.None;
	public Direction ControlDirection
	{
		get { return m_currentDirection; }
	}

	void Awake()
	{
		m_instance = this;
	}

	void Update()
	{
		if (XInput.GetButton(Buttons.LeftStickUp, 0))
		{
			m_currentDirection = Direction.Up;
		}
		else if (XInput.GetButton(Buttons.LeftStickRight, 0))
		{
			m_currentDirection = Direction.Right;
		}
		else if (XInput.GetButton(Buttons.LeftStickDown, 0))
		{
			m_currentDirection = Direction.Down;
		}
		else if (XInput.GetButton(Buttons.LeftStickLeft, 0))
		{
			m_currentDirection = Direction.Left;
		}
	}
}
