using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void OnEndCallback();

public class SplineInterpolator : MonoBehaviour
{
    public enum eEndPointsMode  { AUTO, AUTOCLOSED, EXPLICIT }
    public enum eWrapMode       { ONCE, LOOP }
    
    eEndPointsMode mEndPointsMode = eEndPointsMode.AUTO;

	[System.Serializable]
	public class Info
	{
		public Vector3		m_Position;
		public Quaternion	m_Rotation;
	}

    public float m_splineTangentHalfSampleSize = 2.5f;

    [System.Serializable]
	internal class SplineNode
	{
		internal Vector3    m_point;
		internal Quaternion m_rotation;
		internal float      m_distance;
		internal Vector2    EaseIO;

		internal SplineNode(Vector3 p, Quaternion q, float t, Vector2 io) { m_point = p; m_rotation = q; m_distance = t; EaseIO = io; }
		internal SplineNode(SplineNode o) { m_point = o.m_point; m_rotation = o.m_rotation; m_distance = o.m_distance; EaseIO = o.EaseIO; }
    }

	private List<SplineNode>    m_Nodes = new List<SplineNode>();
	private string              m_State = "";
	private bool                m_Rotations;
	private int					m_LastPoint = 0;
	private OnEndCallback       m_OnEndCallback;
    
	void Awake()
	{
		Reset();
	}

    public List<Vector3> Points()
    {
        List<Vector3> points = new List<Vector3>();

        foreach (SplineNode node in m_Nodes)
        {
            points.Add(node.m_point);
        }

        return points;
    }

    public Vector3 ClosestPoint(Vector3 position, int iterations)
    {
        return GetHermiteAtDistance(GetClosestDistance(position, iterations));
    }

    public float GetClosestDistance(Vector3 position, int iterations)
    {
		return GetClosestDistanceUpToNode(position, iterations, m_Nodes.Count - 1);
    }

	public float GetClosestDistanceUpToDistance(Vector3 position, int iterations, float distance)
	{
		return Mathf.Clamp(GetClosestDistanceUpToNode(position, iterations, MaxNodeAtDistance(distance)), 0.0f, distance);
	}

	private float GetClosestDistanceUpToNode(Vector3 position, int iterations, int node)
	{
		int point1 = 0;
		int point2 = 0;
		float closestDistance = Mathf.Infinity;
		//Vector3 closestPointApprox = new Vector3();

		for (int i = 0; i < node && i < (m_Nodes.Count - 1); ++i)
		{
			Vector3 thisPoint = MathUtils.ClosestPointOnLine(m_Nodes[i].m_point, m_Nodes[i + 1].m_point, position);
			float thisDistance = (position - thisPoint).sqrMagnitude;

			if (thisDistance < closestDistance)
			{
				point1 = i;
				point2 = i + 1;

				closestDistance = thisDistance;
				//closestPointApprox = thisPoint;
			}
		}

		//Binary search
		//float leftSplineLength   = 0;
		//float rightSplineLength  = GetTotalAmount();
		float leftSplineLength = m_Nodes[point1].m_distance;
		float rightSplineLength = m_Nodes[point2].m_distance;

		//float approxDistance       = leftSplineLength + (m_Nodes[point1].m_point - closestPointApprox).magnitude;
		float centreSplineLength = leftSplineLength + ((rightSplineLength - leftSplineLength) / 2);

		float left = (position - GetHermiteAtDistance(leftSplineLength)).magnitude;
		float right = (position - GetHermiteAtDistance(rightSplineLength)).magnitude;
		float centre = (position - GetHermiteAtDistance(centreSplineLength)).magnitude;

		for (int i = 0; i < iterations; ++i)
		{
			if (right > left)
			{
				right = centre;
				rightSplineLength = centreSplineLength;
			}
			else
			{
				left = centre;
				leftSplineLength = centreSplineLength;
			}

			centreSplineLength = leftSplineLength + ((rightSplineLength - leftSplineLength) / 2);
			centre = (position - GetHermiteAtDistance(centreSplineLength)).magnitude;
		}

		return centreSplineLength;
	}

	private int MaxNodeAtDistance(float distance)
	{
		if (m_Nodes.Count == 0) return 0;

		int lastPoint = m_Nodes.Count - 1;

		while (m_Nodes[lastPoint].m_distance > distance)
		{
			lastPoint--;
		}

		return Mathf.Clamp(lastPoint + 1, 0, m_Nodes.Count - 1);
	}

	public void StartInterpolation(OnEndCallback endCallback, bool bRotations, eWrapMode mode)
	{
		if (m_State != "Reset")
			throw new System.Exception("First reset, add points and then call here");

		m_State = mode == eWrapMode.ONCE ? "Once" : "Loop";
		m_Rotations = bRotations;
		m_OnEndCallback = endCallback;

		SetInput();
	}

	public void Reset()
	{
		m_Nodes.Clear();
		m_State         = "Reset";
		mCurrentIdx     = 1;
		mCurrentTime    = 0;
		m_Rotations     = false;
		mEndPointsMode  = eEndPointsMode.AUTO;
	}

	public void AddPoint(Vector3 pos, Quaternion quat, float amount, Vector2 easeInOut)
	{
		if (m_State != "Reset")
			throw new System.Exception("Cannot add points after start");

        if (m_Nodes.Count > 1)
        {
            m_Nodes.Add(new SplineNode(pos, quat, m_Nodes[m_Nodes.Count - 1].m_distance + amount, easeInOut));
        }
        else
        {
            m_Nodes.Add(new SplineNode(pos, quat, amount, easeInOut));
        }
	}

	void SetInput()
	{
		if (m_Nodes.Count < 2)
			throw new System.Exception("Invalid number of points");

		if (m_Rotations)
		{
			for (int c = 1; c < m_Nodes.Count; c++)
			{
				SplineNode node = m_Nodes[c];
				SplineNode prevNode = m_Nodes[c - 1];

				// Always interpolate using the shortest path -> Selective negation
				if (Quaternion.Dot(node.m_rotation, prevNode.m_rotation) < 0)
				{
					node.m_rotation.x = -node.m_rotation.x;
					node.m_rotation.y = -node.m_rotation.y;
					node.m_rotation.z = -node.m_rotation.z;
					node.m_rotation.w = -node.m_rotation.w;
				}
			}
		}

		if (mEndPointsMode == eEndPointsMode.AUTO)
		{
			m_Nodes.Insert(0, m_Nodes[0]);
			m_Nodes.Add(m_Nodes[m_Nodes.Count - 1]);
		}
		else if (mEndPointsMode == eEndPointsMode.EXPLICIT && (m_Nodes.Count < 4))
			throw new System.Exception("Invalid number of points");
	}

	void SetExplicitMode()
	{
		if (m_State != "Reset")
			throw new System.Exception("Cannot change mode after start");

		mEndPointsMode = eEndPointsMode.EXPLICIT;
	}

    public void SetAutoCloseMode(float joiningPointTime)
    {
        if (m_State != "Reset")
            throw new System.Exception("Cannot change mode after start");

        mEndPointsMode = eEndPointsMode.AUTOCLOSED;

        m_Nodes.Add(new SplineNode(m_Nodes[0] as SplineNode));
        m_Nodes[m_Nodes.Count - 1].m_distance = joiningPointTime;

        Vector3 vInitDir = (m_Nodes[1].m_point - m_Nodes[0].m_point).normalized;
        Vector3 vEndDir = (m_Nodes[m_Nodes.Count - 2].m_point - m_Nodes[m_Nodes.Count - 1].m_point).normalized;
        float firstLength = (m_Nodes[1].m_point - m_Nodes[0].m_point).magnitude;
        float lastLength = (m_Nodes[m_Nodes.Count - 2].m_point - m_Nodes[m_Nodes.Count - 1].m_point).magnitude;

        SplineNode firstNode = new SplineNode(m_Nodes[0] as SplineNode);
        firstNode.m_point = m_Nodes[0].m_point + vEndDir * firstLength;

        SplineNode lastNode = new SplineNode(m_Nodes[m_Nodes.Count - 1] as SplineNode);
        lastNode.m_point = m_Nodes[0].m_point + vInitDir * lastLength;

        m_Nodes.Insert(0, firstNode);
        m_Nodes.Add(lastNode);
    }

	float mCurrentTime;
	int mCurrentIdx = 1;

    //Quick fix to stop the old-time based version from changing the positions of the spline
    void Start()
    {
        m_State = "Stopped";
    }

	void Update()
	{
        m_State = "Stopped";

        if (m_State == "Reset" || m_State == "Stopped" || m_Nodes.Count < 4)
			return;

		mCurrentTime += Time.deltaTime;

		// We advance to next point in the path
		if (mCurrentTime >= m_Nodes[mCurrentIdx + 1].m_distance)
		{
			if (mCurrentIdx < m_Nodes.Count - 3)
			{
				mCurrentIdx++;
			}
			else
			{
				if (m_State != "Loop")
				{
					m_State = "Stopped";

					// We stop right in the end point
					transform.position = m_Nodes[m_Nodes.Count - 2].m_point;

					if (m_Rotations)
						transform.rotation = m_Nodes[m_Nodes.Count - 2].m_rotation;

					// We call back to inform that we are ended
					if (m_OnEndCallback != null)
						m_OnEndCallback();
				}
				else
				{
					mCurrentIdx = 1;
					mCurrentTime = 0;
				}
			}
		}

		if (m_State != "Stopped")
		{
			// Calculates the t param between 0 and 1
			float param = (mCurrentTime - m_Nodes[mCurrentIdx].m_distance) / (m_Nodes[mCurrentIdx + 1].m_distance - m_Nodes[mCurrentIdx].m_distance);

			// Smooth the param
			param = MathUtils.Ease(param, m_Nodes[mCurrentIdx].EaseIO.x, m_Nodes[mCurrentIdx].EaseIO.y);

			transform.position = GetHermiteInternal(mCurrentIdx, param);

			if (m_Rotations)
			{
				transform.rotation = GetSquad(mCurrentIdx, param);
			}
		}
	}

	Quaternion GetSquad(int idxFirstPoint, float t)
	{
		Quaternion Q0 = m_Nodes[idxFirstPoint - 1].m_rotation;
		Quaternion Q1 = m_Nodes[idxFirstPoint].m_rotation;
		Quaternion Q2 = m_Nodes[idxFirstPoint + 1].m_rotation;
		Quaternion Q3 = m_Nodes[idxFirstPoint + 2].m_rotation;

		Quaternion T1 = MathUtils.GetSquadIntermediate(Q0, Q1, Q2);
		Quaternion T2 = MathUtils.GetSquadIntermediate(Q1, Q2, Q3);

		return MathUtils.GetQuatSquad(t, Q1, Q2, T1, T2);
	}

	public Vector3 GetHermiteInternal(int idxFirstPoint, float t)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		Vector3 P0 = m_Nodes[idxFirstPoint - 1].m_point;
		Vector3 P1 = m_Nodes[idxFirstPoint].m_point;
		Vector3 P2 = m_Nodes[idxFirstPoint + 1].m_point;
		Vector3 P3 = m_Nodes[idxFirstPoint + 2].m_point;

		float tension = 0.5f;	// 0.5 equivale a catmull-rom

		Vector3 T1 = tension * (P2 - P0);
		Vector3 T2 = tension * (P3 - P1);

		float Blend1 = 2 * t3 - 3 * t2 + 1;
		float Blend2 = -2 * t3 + 3 * t2;
		float Blend3 = t3 - 2 * t2 + t;
		float Blend4 = t3 - t2;

		return Blend1 * P1 + Blend2 * P2 + Blend3 * T1 + Blend4 * T2;
	}

	public Vector3 GetHermiteAtDistance(float distance)
	{
        if (m_Nodes.Count < 2) return Vector3.zero;
        
        if (distance >= m_Nodes[m_Nodes.Count - 2].m_distance)
		{
			return m_Nodes[m_Nodes.Count - 2].m_point;
		}

		while (m_Nodes[m_LastPoint].m_distance > distance)
		{
			m_LastPoint--;
		}

		while (m_Nodes[m_LastPoint+1].m_distance <= distance)
		{
			m_LastPoint++;
		}

		int lo = m_LastPoint;
		int hi = m_LastPoint + 1;

		float ratio = (distance - m_Nodes[lo].m_distance) / (m_Nodes[hi].m_distance - m_Nodes[lo].m_distance);
		ratio = MathUtils.Ease(ratio, m_Nodes[lo].EaseIO.x, m_Nodes[lo].EaseIO.y);

		return GetHermiteInternal(lo, ratio);
	}

    public Quaternion GetSquadAtDistance(float distance)
    {
        int c;
        for (c = 1; c < m_Nodes.Count - 2; c++)
        {
            if (m_Nodes[c].m_distance > distance)
                break;
        }

        int idx = c - 1;

		if (idx < 0 || idx >= m_Nodes.Count - 1)
		{
			Debug.Log("Tried to access: " + idx + " in list of size: " + m_Nodes.Count);
			return new Quaternion();
		}

        float param = (distance - m_Nodes[idx].m_distance) / (m_Nodes[idx + 1].m_distance - m_Nodes[idx].m_distance);
        return GetSquad(idx, param);
    }

	public void GetDirtectedQuatAndPosAtDistance(ref Info result,float distance, float sampleSize)
	{
		float distance1 = distance;
        float distance2 = distance + sampleSize;

        if (distance < 0)
        {
            distance1 = 0;
        }

        if ((distance + sampleSize) > GetTotalAmount())
        {
            distance2 = GetTotalAmount();
        }

        Vector3 position1 = GetHermiteAtDistance(distance1);
        Vector3 position2 = GetHermiteAtDistance(distance2);
        
		result.m_Position = position1;

		Vector3 lookDirection = (position2 - position1).normalized;
		if (lookDirection != Vector3.zero) result.m_Rotation = Quaternion.LookRotation(lookDirection, GetSquadAtDistance(distance).Up());
	}

    public Quaternion GetDirectedQuatAtDistance(float distance, float halfSampleSize)
    {
        float distance1 = distance - halfSampleSize;
        float distance2 = distance + halfSampleSize;

        if (distance - halfSampleSize < 0)
        {
            distance1 = 0;
        }

        if (distance + halfSampleSize > GetTotalAmount())
        {
            distance2 = GetTotalAmount();
        }

        Vector3 position1 = GetHermiteAtDistance(distance1);
        Vector3 position2 = GetHermiteAtDistance(distance2);
        return Quaternion.LookRotation((position2 - position1).normalized, GetSquadAtDistance(distance).Up());
    }

    public float GetTotalAmount()
    {
        if (m_Nodes.Count > 0)
        {
            return m_Nodes[m_Nodes.Count - 1].m_distance;
        }

        return 0.0f;
    }
}