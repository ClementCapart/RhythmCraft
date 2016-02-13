using UnityEngine;
using System.Collections;

public class IntRange
{
	[SerializeField]
	private int	m_min		= 0;

	[SerializeField]
	private int m_max		= 0;
	
	public int Min { get { return m_min;}}
	public int Max { get { return m_max;}}
	public int Random { get { return UnityEngine.Random.Range(m_min,m_max);}}

	public bool IsValid
	{
		get
		{
			return m_min <= m_max;
		}
	}

	public IntRange()
	{
		m_min = 0;
		m_max = 0;
	}

	public IntRange(int min,int max)
	{
		Set(min,max);
	}

	public void Set(int min, int max)
	{
		m_min = Mathf.Min(min,max);
		m_max = Mathf.Max(min,max);
	}

	public void SetRaw(int min, int max)
	{
		m_min	= min;
		m_max	= max;
	}
}
