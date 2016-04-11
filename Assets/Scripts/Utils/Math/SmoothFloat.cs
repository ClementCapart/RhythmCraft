using UnityEngine;
using System;

//////////////////////////////////////////////////////////////////////////
// 
// Simple class to smoothly interpolate between float values over time
// Motion is critically damped and supports any size of time step. 
//
//////////////////////////////////////////////////////////////////////////

[System.Serializable]
public class SmoothFloat
{
	private float					m_Curr;				//< current value of this float
	private float					m_Target;			//< target value of this float
	private float					m_Vel;				//< change last frame
	private float					m_SlideRate;		//< how smoothly should we move towards the target direction
	private bool					m_IsDone;

	public bool IsDone { get { return m_IsDone;}}

	public SmoothFloat(float trackingRate, float initalValue = 0.0f)
	{
		m_SlideRate = trackingRate;
		SetNow(initalValue);
	}

    public static implicit operator float(SmoothFloat val)
    {
        return val.Value;
    }

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public float Value
	{
		get
		{
			return m_Curr;
		}
		set
		{
			float dist = value - m_Curr;
			if ((dist * dist) > 0.00001f)
			{
				m_Target    = value;
				m_IsDone    = false;
			}
			else
			{
				m_Curr      = value;
				m_Target    = value;
				m_IsDone    = true;
				m_Vel       = 0.0f;
			}
		}
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public float NextValue
	{
		get
		{
			float timestep = Time.fixedDeltaTime;
			if (timestep > 0.0f)
			{
				float newVel = m_Vel;
				return SmoothFloatCD(m_Curr,m_Target,ref newVel,m_SlideRate,timestep);
			}
			else
			{
				return m_Curr;
			}
		}
	}
	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public SmoothFloat ()
	{
		m_SlideRate = 2.0f;				
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
        if (!m_IsDone)
        {
            if (timestep > 0.0f)
            {
                m_Curr = SmoothFloatCD(m_Curr, m_Target, ref m_Vel, m_SlideRate, timestep);

                float GapLine = Math.Abs(m_Target - m_Curr);
                if (GapLine < 0.0001f)
                {
                    m_Curr = m_Target;
                    m_Vel = 0.0f;
                    m_IsDone = true;
                }
            }

        }
    }

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public void SetNow(float val)
	{
		m_Curr      = val;
		m_Target    = val;
		m_IsDone    = true;
		m_Vel       = 0.0f;
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	private float SmoothFloatCD(float from,float to,ref float vel,float smoothTime,float timestep)
	{
		float omega		= smoothTime * 2.0f;
		float x			= omega * timestep;
		float texp		= 1.0f / (1.0f + x + (x * x * 0.48f) + (x * x * x * 0.235f));
		float change	= from - to;
		float temp		= (vel + omega * change) * timestep;
		vel				= (vel - omega * temp) * texp;

		return	to + (change+temp) * texp;
	}
}

