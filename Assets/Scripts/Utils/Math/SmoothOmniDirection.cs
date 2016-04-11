using UnityEngine;
using System;
using System.Collections;

//////////////////////////////////////////////////////////////////////////
// 
// Simple class to smoothly slerp between unit vectors over time.
// 
// Motion is NOT critically damped but supports any size of time step. 
//
//////////////////////////////////////////////////////////////////////////

public class SmoothOmniDirection
{
	private Vector3					m_Curr;				//< current direction as a vector
	private Vector3					m_Dest;		        //< destination angles around the x and y axis
	private Vector3					m_Vel;				//< current angular velocities
	
	private float					m_SlideRate;		//< how smoothly should we move towards the target position

    private bool                    m_isClose;
    private bool                    m_IsDone;

    public bool                     IsClose { get { return m_isClose; } }
    public bool                     IsDone { get { return m_IsDone; } }
    public bool                     IsWithin(float difference) { return (m_Dest - m_Curr).magnitude <= difference; }

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public Vector3 Value
	{
		get
		{
			return m_Curr;
		}
		set
		{
            m_Dest = value;

            //Note, not spherical, but should suffice for checking if close
            float difference = (m_Dest - m_Curr).magnitude;
            if (difference < 0.01f)
            {
                m_isClose = true;
                if (difference < 0.0001f)
                {
                    m_IsDone = true;
                }
            }
            else
            {
                m_IsDone = false;
                m_isClose = false;
            }
		}
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public SmoothOmniDirection()
	{
		m_SlideRate = 2.0f;	
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
    public SmoothOmniDirection(float slideRate)
	{
		m_SlideRate = slideRate ;	
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public void SetTrackingRate(float slideRate)
	{
		m_SlideRate = slideRate;
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public void Update()
	{
		Update(Time.fixedDeltaTime);
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public void Update(float timestep)
	{
        if (!m_IsDone && (timestep > 0.0f))
		{
            m_Curr = SmoothDirectionCD(m_Curr, m_Dest, ref m_Vel, m_SlideRate, timestep);

            //Note, not spherical, but should suffice for checking if close
            float difference = (m_Dest - m_Curr).magnitude;
            if (difference < 0.01f)
            {
                m_isClose = true;
                if (difference < 0.0001f)
                {
                    m_IsDone = true;
                }
            }
            else
            {
                m_IsDone = false;
                m_isClose = false;
            }
		}
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public void SetNow(Vector3 val)
	{
		m_Curr			= val;
		m_Curr.Normalize();

		m_Vel			= new Vector3(0,0,0);
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	private Vector3 SmoothDirectionCD(Vector3 from,Vector3 to,ref Vector3 vel,float smoothTime,float timestep)
	{
        return Vector3.Slerp(from, to, smoothTime*timestep);
	}
}
