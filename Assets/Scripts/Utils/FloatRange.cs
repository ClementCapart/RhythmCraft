using UnityEngine;

[System.Serializable]
public class FloatRange
{
	[SerializeField]
	private float m_min		= 0.0f;

	[SerializeField]
	private float m_max		= 0.0f;
	
	public float Min { get { return m_min;}}
	public float Max { get { return m_max;}}
	public float Random { get { return UnityEngine.Random.Range(m_min,m_max);}}

	public bool IsValid
	{
		get
		{
			return m_min <= m_max;
		}
	}

	public FloatRange()
	{
		m_min = 0.0f;
		m_max = 0.0f;
	}

	public FloatRange(float min,float max)
	{
		Set(min,max);
	}

	public float RandomLOD(Transform owner,float extra = 0.0f)
	{
		Vector3 line = owner.position - Camera.main.transform.position;
		float dist	 = line.magnitude;
		float ratio	 = Mathf.Clamp01((dist - 75.0f) / 50.0f);
		float lmin = m_min + ((m_max - m_min) * ratio);

		return UnityEngine.Random.Range(lmin,m_max) + (extra * ratio);
	}

	public void Set(float min,float max)
	{
		m_min = Mathf.Min(min,max);
		m_max = Mathf.Max(min,max);
	}

	public void SetRaw(float min, float max)
	{
		m_min	= min;
		m_max	= max;
	}
}
