using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class SmoothPosition
{
	private Vector3				    m_Curr;				//< current position
	private Vector3				    m_Target;			//< target position to move towards
	private Vector3					m_Vel;				//< motion last frame (used to smooth acceleration)
	
	private float					m_SlideRate;		//< how smoothly should we move towards the target position
	private bool					m_IsDone;			//< is the interpolation done.

	public bool IsDone { get { return m_IsDone;}}

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
			Vector3 dist = value - m_Curr;
			if (dist.magnitude > 0.000001)
			{
				m_Target    = value;
				m_IsDone    = false;
			}
			else
			{
				m_Curr	    = value;
				m_Target    = value;
				m_IsDone    = true;
				m_Vel       = new Vector3(0,0,0);
			}
		}
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public SmoothPosition (float slideRate)
	{
		m_SlideRate = slideRate;				
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public SmoothPosition ()
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
				m_Curr = SmoothPositionCD(m_Curr,m_Target,ref m_Vel,m_SlideRate,timestep);

				Vector3 GapLine			= (m_Target - m_Curr);

				if (GapLine.magnitude < 0.001f)
				{
					m_Curr		    = m_Target;
					m_Vel		    = new Vector3(0,0,0);
					m_IsDone	    = true;
				}
			}
			
		}
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public void SetNow(Vector3 val)
	{
		m_Curr      = val;
		m_Target    = val;
		m_IsDone    = true;
		m_Vel       = new Vector3(0,0,0);
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	Vector3 SmoothPositionCD(Vector3 from,Vector3 to,ref Vector3 vel,float smoothTime,float timestep)
	{
		float omega		= smoothTime * 2.0f;
		float x			= omega * timestep;
		float texp		= 1.0f / (1.0f + x + (x * x * 0.48f) + (x * x * x * 0.235f));
		Vector3 change	= from - to;
		Vector3 temp	= (vel + omega * change) * timestep;
		vel				= (vel - omega * temp) * texp;

		return	to + (change+temp) * texp;
	}

}
