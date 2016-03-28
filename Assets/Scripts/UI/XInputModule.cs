using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class XInputModule : BaseInputModule 
{
    private float m_NextAction;

	private InputMode m_CurrentInputMode = InputMode.Buttons;

	public enum InputMode
	{
		Buttons
	}

    public Axis m_HorizontalAxis;
    public Axis m_VerticalAxis;

    public Buttons m_ConfirmButton;
    public Buttons m_CancelButton;

    public float m_InputActionsPerSecond = 10;

    public override bool ShouldActivateModule()
	{
		if (!base.ShouldActivateModule ())
			return false;

		var shouldActivate = XInput.GetButtonDown (m_ConfirmButton, 0);
		shouldActivate |= XInput.GetButtonDown (m_CancelButton, 0);
		shouldActivate |= !Mathf.Approximately (XInput.GetAxis(m_HorizontalAxis, 0), 0.0f);
		shouldActivate |= !Mathf.Approximately (XInput.GetAxis(m_VerticalAxis, 0), 0.0f);
		return shouldActivate;
	}

    public override void DeactivateModule()
	{
		base.DeactivateModule ();
        eventSystem.SetSelectedGameObject(null);
	}

    public override void Process()
    {
        bool usedEvent = SendUpdateEventToSelectedObject();

		if (eventSystem.sendNavigationEvents)
		{
			if (!usedEvent)
				usedEvent |= SendMoveEventToSelectedObject ();

			if (!usedEvent)
				SendSubmitEventToSelectedObject ();
		}
    }

    private bool SendSubmitEventToSelectedObject()
	{
		if (eventSystem.currentSelectedGameObject == null)
			return false;

		var data = GetBaseEventData ();
		if (XInput.GetButtonDown (m_ConfirmButton, 0))
			ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

		if (XInput.GetButtonDown (m_CancelButton, 0))
			ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
		return data.used;
	}

    private bool AllowMoveEventProcessing(float time)
	{
		bool allow = (time > m_NextAction);
		return allow;
	}

    private Vector2 GetRawMoveVector()
	{
		Vector2 move = Vector2.zero;
		move.x = XInput.GetAxis(m_HorizontalAxis, 0);
		move.y = XInput.GetAxis(m_VerticalAxis, 0);

		if (move.x < 0)
			move.x = -1f;
		if (move.x > 0)
			move.x = 1f;

		if (move.y < 0)
			move.y = -1f;
		if (move.y > 0)
			move.y = 1f;

		return move;
	}

    private bool SendMoveEventToSelectedObject()
	{
		float time = Time.unscaledTime;

		if (!AllowMoveEventProcessing (time))
			return false;

		Vector2 movement = GetRawMoveVector ();
		//Debug.Log(m_ProcessingEvent.rawType + " axis:" + m_AllowAxisEvents + " value:" + "(" + x + "," + y + ")");
		var axisEventData = GetAxisEventData (movement.x, movement.y, 0.6f);
		if (!Mathf.Approximately (axisEventData.moveVector.x, 0f)
			|| !Mathf.Approximately (axisEventData.moveVector.y, 0f))
		{
			if (m_CurrentInputMode != InputMode.Buttons)
			{
				// so if we are chaning to keyboard
				m_CurrentInputMode = InputMode.Buttons;

				// if we are doing a 'fresh selection'
				// return as we don't want to do a move.
				if (ResetSelection ())
				{
					m_NextAction = time + 1f / m_InputActionsPerSecond;
					return true;
				}
			}
			ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
		}
		m_NextAction = time + 1f / m_InputActionsPerSecond;
		return axisEventData.used;
	}

    private bool ResetSelection()
	{
		var baseEventData = GetBaseEventData ();
		// clear all selection
		// & figure out what the mouse is over
		eventSystem.SetSelectedGameObject (null, baseEventData);

		return true;
	}

    private bool SendUpdateEventToSelectedObject()
	{
		if (eventSystem.currentSelectedGameObject == null)
			return false;

		var data = GetBaseEventData ();
		ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
		return data.used;
	}


}
