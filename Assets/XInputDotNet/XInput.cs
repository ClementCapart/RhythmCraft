using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public enum Buttons
{
    None,
    A,
    B,
    X,
    Y,
    Start,
    Back,
    RightBumper,
    LeftBumper,
    RightTrigger,
    LeftTrigger,
    Up,
    Right,
    Down,
    Left,
    LeftStick,
    RightStick,
    LeftStickUp,
    LeftStickRight,
    LeftStickDown,
    LeftStickLeft,
    RightStickUp,
    RightStickRight,
    RightStickDown,
    RightStickLeft,
}

public enum Axis
{
    None,
    LeftStickHorizontal,
    LeftStickVertical,
    RightStickHorizontal,
    RightStickVertical,
    LeftTrigger,
    RightTrigger,
}

public class XInput : MonoBehaviour
{
    private static XInput m_instance;

    private List<GamePadState> m_gamepadsState;
    private List<GamePadState> m_previousGamepadsState;

    public float m_SticksThresholdAsButtons = 0.5f;
    public float m_TriggersThresholdAsButtons = 0.3f;

    void OnEnable()
    {
        if(m_instance == null) m_instance = this;

        m_gamepadsState = new List<GamePadState>();
        m_previousGamepadsState = new List<GamePadState>();
    }

    void Update()
    {
        GetFourGamepads();
    }

    void GetFourGamepads()
    {
        m_previousGamepadsState.Clear();
        m_previousGamepadsState.AddRange(m_gamepadsState);

        m_gamepadsState.Clear();

        m_gamepadsState.Add(GamePad.GetState(PlayerIndex.One));
        m_gamepadsState.Add(GamePad.GetState(PlayerIndex.Two));
        m_gamepadsState.Add(GamePad.GetState(PlayerIndex.Three));
        m_gamepadsState.Add(GamePad.GetState(PlayerIndex.Four));

        if (m_previousGamepadsState.Count == 0)
            m_previousGamepadsState.AddRange(m_gamepadsState);
    }
    
    public static bool GetButtonDown(Buttons button, int playerIndex)
    {
        if (m_instance)
        {
            if (m_instance.m_gamepadsState != null && m_instance.m_gamepadsState.Count > playerIndex)
            {
                if (m_instance.m_gamepadsState[playerIndex].IsConnected)
                {
                    switch (button)
                    {
                        case Buttons.A:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.A == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].Buttons.A == ButtonState.Released;

                        case Buttons.B:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.B == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].Buttons.B == ButtonState.Released;

                        case Buttons.X:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.X == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].Buttons.X == ButtonState.Released;

                        case Buttons.Y:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.Y == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].Buttons.Y == ButtonState.Released;

                        case Buttons.Start:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.Start == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].Buttons.Start == ButtonState.Released;

                        case Buttons.Back:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.Back == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].Buttons.Back == ButtonState.Released;

                        case Buttons.RightBumper:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.RightShoulder == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].Buttons.RightShoulder == ButtonState.Released;

                        case Buttons.LeftBumper:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.LeftShoulder == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].Buttons.LeftShoulder == ButtonState.Released;

                        case Buttons.RightTrigger:
                            return m_instance.m_gamepadsState[playerIndex].Triggers.Right < m_instance.m_TriggersThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].Triggers.Right > m_instance.m_TriggersThresholdAsButtons;

                        case Buttons.LeftTrigger:
                            return m_instance.m_gamepadsState[playerIndex].Triggers.Left < m_instance.m_TriggersThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].Triggers.Left > m_instance.m_TriggersThresholdAsButtons;

                        case Buttons.Up:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Up == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].DPad.Up == ButtonState.Released;

                        case Buttons.Right:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Right == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].DPad.Right == ButtonState.Released;

                        case Buttons.Down:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Down == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].DPad.Down == ButtonState.Released;

                        case Buttons.Left:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Left == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].DPad.Left == ButtonState.Released;

                        case Buttons.LeftStick:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.LeftStick == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].Buttons.LeftStick == ButtonState.Released;

                        case Buttons.RightStick:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.RightStick == ButtonState.Pressed && m_instance.m_previousGamepadsState[playerIndex].Buttons.RightStick == ButtonState.Released;

                        case Buttons.LeftStickUp:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.Y > m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Left.Y < m_instance.m_SticksThresholdAsButtons;

                        case Buttons.LeftStickRight:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.X > m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Left.X < m_instance.m_SticksThresholdAsButtons;

                        case Buttons.LeftStickDown:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.Y < -m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Left.Y > -m_instance.m_SticksThresholdAsButtons;

                        case Buttons.LeftStickLeft:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.X < -m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Left.X > -m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickUp:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.Y > m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Right.Y < m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickRight:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.X > m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Right.X < m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickDown:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.Y < -m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Right.Y > -m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickLeft:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.X < -m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Right.X > -m_instance.m_SticksThresholdAsButtons;
                    }
                }
            }
        }
        return false;
    }

    public static bool GetButtonUp(Buttons button, int playerIndex)
    {
        if (m_instance)
        {
            if (m_instance.m_gamepadsState != null && m_instance.m_gamepadsState.Count > playerIndex)
            {
                if (m_instance.m_gamepadsState[playerIndex].IsConnected)
                {
                    switch (button)
                    {
                        case Buttons.A:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.A == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].Buttons.A == ButtonState.Pressed;

                        case Buttons.B:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.B == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].Buttons.B == ButtonState.Pressed;

                        case Buttons.X:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.X == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].Buttons.X == ButtonState.Pressed;

                        case Buttons.Y:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.Y == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].Buttons.Y == ButtonState.Pressed;

                        case Buttons.Start:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.Start == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].Buttons.Start == ButtonState.Pressed;

                        case Buttons.Back:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.Back == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].Buttons.Back == ButtonState.Pressed;

                        case Buttons.RightBumper:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.RightShoulder== ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].Buttons.RightShoulder == ButtonState.Pressed;

                        case Buttons.LeftBumper:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.LeftShoulder == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].Buttons.LeftShoulder == ButtonState.Pressed;

                        case Buttons.RightTrigger:
                            return m_instance.m_gamepadsState[playerIndex].Triggers.Right > m_instance.m_TriggersThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].Triggers.Right < m_instance.m_TriggersThresholdAsButtons;

                        case Buttons.LeftTrigger:
                            return m_instance.m_gamepadsState[playerIndex].Triggers.Left > m_instance.m_TriggersThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].Triggers.Left < m_instance.m_TriggersThresholdAsButtons;

                        case Buttons.Up:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Up == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].DPad.Up == ButtonState.Pressed;

                        case Buttons.Right:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Right == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].DPad.Right == ButtonState.Pressed;

                        case Buttons.Down:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Down == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].DPad.Down == ButtonState.Pressed;

                        case Buttons.Left:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Left == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].DPad.Left == ButtonState.Pressed;

                        case Buttons.LeftStick:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.LeftStick == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].Buttons.LeftStick == ButtonState.Pressed;

                        case Buttons.RightStick:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.RightStick == ButtonState.Released && m_instance.m_previousGamepadsState[playerIndex].Buttons.RightStick == ButtonState.Pressed;

                        case Buttons.LeftStickUp:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.Y < m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Left.Y > m_instance.m_SticksThresholdAsButtons;

                        case Buttons.LeftStickRight:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.X < m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Left.X > m_instance.m_SticksThresholdAsButtons;

                        case Buttons.LeftStickDown:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.Y > -m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Left.Y < -m_instance.m_SticksThresholdAsButtons;

                        case Buttons.LeftStickLeft:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.X > -m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Left.X < -m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickUp:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.Y < m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Right.Y > m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickRight:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.X < m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Right.X > m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickDown:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.Y > -m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Right.Y < -m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickLeft:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.X > -m_instance.m_SticksThresholdAsButtons && m_instance.m_previousGamepadsState[playerIndex].ThumbSticks.Right.X < -m_instance.m_SticksThresholdAsButtons;
                    }
                }
            }
        }
        return false;
    }

    public static bool GetButton(Buttons button, int playerIndex)
    {
        if(m_instance)
        {
            if(m_instance.m_gamepadsState != null && m_instance.m_gamepadsState.Count > playerIndex)
            {
                if(m_instance.m_gamepadsState[playerIndex].IsConnected)
                {
                    switch(button)
                    {
                        case Buttons.A:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.A == ButtonState.Pressed;

                        case Buttons.B:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.B == ButtonState.Pressed;

                        case Buttons.X:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.X == ButtonState.Pressed;

                        case Buttons.Y:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.Y == ButtonState.Pressed;

                        case Buttons.Start:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.Start == ButtonState.Pressed;

                        case Buttons.Back:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.Back == ButtonState.Pressed;

                        case Buttons.RightBumper:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.RightShoulder == ButtonState.Pressed;

                        case Buttons.LeftBumper:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.LeftShoulder == ButtonState.Pressed;

                        case Buttons.RightTrigger:
                            return m_instance.m_gamepadsState[playerIndex].Triggers.Right > m_instance.m_TriggersThresholdAsButtons;

                        case Buttons.LeftTrigger:
                            return m_instance.m_gamepadsState[playerIndex].Triggers.Left > m_instance.m_TriggersThresholdAsButtons;

                        case Buttons.Up:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Up == ButtonState.Pressed;

                        case Buttons.Right:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Right == ButtonState.Pressed;

                        case Buttons.Down:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Down == ButtonState.Pressed;

                        case Buttons.Left:
                            return m_instance.m_gamepadsState[playerIndex].DPad.Left == ButtonState.Pressed;

                        case Buttons.LeftStick:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.LeftStick == ButtonState.Pressed;

                        case Buttons.RightStick:
                            return m_instance.m_gamepadsState[playerIndex].Buttons.RightStick == ButtonState.Pressed;

                        case Buttons.LeftStickUp:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.Y > m_instance.m_SticksThresholdAsButtons;

                        case Buttons.LeftStickRight:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.X > m_instance.m_SticksThresholdAsButtons;

                        case Buttons.LeftStickDown:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.Y < -m_instance.m_SticksThresholdAsButtons;

                        case Buttons.LeftStickLeft:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.X < -m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickUp:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.Y > m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickRight:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.X > m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickDown:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.Y < -m_instance.m_SticksThresholdAsButtons;

                        case Buttons.RightStickLeft:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.X < -m_instance.m_SticksThresholdAsButtons;

                    }
                }
            }
        }
        return false;
    }

    public static float GetAxis(Axis axis, int playerIndex)
    {
        if (m_instance)
        {
            if (m_instance.m_gamepadsState != null && m_instance.m_gamepadsState.Count > playerIndex)
            {
                if (m_instance.m_gamepadsState[playerIndex].IsConnected)
                {
                    switch(axis)
                    {
                        case Axis.LeftStickHorizontal:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.X;

                        case Axis.LeftStickVertical:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Left.Y;

                        case Axis.RightStickHorizontal:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.X;

                        case Axis.RightStickVertical:
                            return m_instance.m_gamepadsState[playerIndex].ThumbSticks.Right.Y;

                        case Axis.LeftTrigger:
                            return m_instance.m_gamepadsState[playerIndex].Triggers.Left;

                        case Axis.RightTrigger:
                            return m_instance.m_gamepadsState[playerIndex].Triggers.Right;

                    }
                }
            }
        }

        return 0.0f;
    }
}

