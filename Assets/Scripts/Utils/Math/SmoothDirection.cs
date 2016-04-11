using UnityEngine;
using System;
using System.Collections;

//////////////////////////////////////////////////////////////////////////
// 
// Simple class to smoothly interpolate between unit vectors over time
//
// Despite being prone to gimbal lock, vectors are described as angle/azimuth rotations.
// As this is used for cameras, such motions look more natural.
// Motion is critically damped and supports any size of time step. 
//
//////////////////////////////////////////////////////////////////////////

[System.Serializable]
public class SmoothDirection
{
	private Vector3					m_Curr;				//< current direction as a vector
	private Vector3					m_CurrAngles;		//< current angles around the x and y axis		
	private Vector3					m_DestAngles;		//< destination angles around the x and y axis
	private Vector3					m_Vel;				//< current angular velocities
	
	private float					m_SlideRate;		//< how smoothly should we move towards the target position

    private bool                    m_isClose;
    private bool                    m_IsDone;

    public bool                     IsClose { get { return m_isClose; } }
    public bool                     IsDone { get { return m_IsDone; } }

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
			MathUtils.CalcAnglesFromDir(value,ref m_DestAngles.y,ref m_DestAngles.x);

			Vector3		gapLine;
			gapLine.x		= MathUtils.CalcMinAngleDif(m_CurrAngles.x,m_DestAngles.x);
			gapLine.y		= MathUtils.CalcMinAngleDif(m_CurrAngles.y,m_DestAngles.y);
			gapLine.z		= 0.0f;
			m_DestAngles	= m_CurrAngles + gapLine;

            float GapLine = gapLine.magnitude;
            if (GapLine < 0.0001f)
            {
                m_isClose = true;
                if (GapLine < 0.000001f)
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
	public SmoothDirection()
	{
		m_SlideRate = 2.0f;	
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public SmoothDirection(float slideRate)
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
			Vector3		gapLine;
			gapLine.x		= MathUtils.CalcMinAngleDif(m_CurrAngles.x,m_DestAngles.x);
			gapLine.y		= MathUtils.CalcMinAngleDif(m_CurrAngles.y,m_DestAngles.y);
			gapLine.z		= 0.0f;
			m_DestAngles	= m_CurrAngles + gapLine;

			m_CurrAngles	= SmoothAnglesCD(m_CurrAngles,m_DestAngles,ref m_Vel,m_SlideRate,timestep);

			MathUtils.CalcDirFromAngles(ref m_Curr,m_CurrAngles.y,m_CurrAngles.x);

            float GapLine = gapLine.magnitude;
            if (GapLine < 0.01f)
            {
                m_isClose = true;
                if (GapLine < 0.0001f)
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
		MathUtils.CalcAnglesFromDir(m_Curr,ref m_DestAngles.y,ref m_DestAngles.x);

		m_CurrAngles	= m_DestAngles;
		m_Vel			= new Vector3(0,0,0);
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public void SetNowFromAngles(float yAxis,float xAxis)
	{
		m_DestAngles.y = yAxis;
		m_DestAngles.x = xAxis;
		m_DestAngles.z = 0.0f;

		MathUtils.CalcDirFromAngles(ref m_Curr,m_DestAngles.y,m_DestAngles.x);

		m_CurrAngles	= m_DestAngles;
		m_Vel			= new Vector3(0,0,0);
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public void AddFromAngles(float yAxis,float xAxis)
	{
		float targetY = m_DestAngles.y + yAxis;
		float targetX = m_DestAngles.x + xAxis;
		Vector3 newDir = new Vector3();
	
		MathUtils.CalcDirFromAngles(ref newDir,targetY,targetX);

		Value = newDir;
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	private Vector3 SmoothAnglesCD(Vector3 from,Vector3 to,ref Vector3 vel,float smoothTime,float timestep)
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
