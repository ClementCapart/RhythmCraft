using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public sealed class StringSet : IEnumerable<string>
{
	[SerializeField]
	private string[]			m_Set	= null;				//< list of all filter sets this filter works in
	public  string				m_Summary = "";

	public string this[int index] 
	{
		get 
		{ 			
			if (m_Set != null)
			{
				if (index < m_Set.Length)
				{
					return m_Set[index];
				}
			}

			return "";
		}
	}
	
	public int Length
	{
		get 
		{
			if (m_Set != null)
			{
				return m_Set.Length;
			}
			return 0;
		}
	}

	public bool IsEmpty { get { return Length == 0;}}

	private void GeterateSummary()
	{
		string res = "[empty]";

		if (m_Set != null)
		{
			bool gotFirst = false;

			foreach (string str in m_Set)
			{
				if (gotFirst)
				{
					res = res + "," + str;
				}
				else
				{
					gotFirst = true;
					res = str;
				}
			}
		}
		m_Summary = res;
	}

	public override string ToString()
	{
		return m_Summary;
	}

	public StringSet()
	{

	}

	public StringSet(string[] set)
	{
		Set(set);
	}

	public StringSet(string commaSeparated)
	{
		Set(commaSeparated);
	}

	public void Set(string commaSeparated)
	{
		string noStartSpace = commaSeparated.TrimStart(new char[] {' '});
		string noEndSpace	= noStartSpace.TrimEnd(new char[] {' '});

		string[] contents = noEndSpace.Split(',');
		
		Set(contents);
	}

	public void Set(string[] values)
	{
		Array.Sort(values,StringComparer.InvariantCulture);
		List<string> temp = new List<string>();
		int n = 0;
		string prev = "";
		char[] singleSpaceArray = new char[] {' '};
		
		foreach (string str in values)
		{
			if (str != "")
			{
				string noStartSpace = str.TrimStart(singleSpaceArray);
				string final		= noStartSpace.TrimStart(singleSpaceArray);
				
				if (final.Length > 0)
				{
					if (String.Compare(str,prev) != 0)
					{
						temp.Add(str);
						prev = str;
					}
				}
			}
		}

		if (temp.Count > 0)
		{
			m_Set = new string[temp.Count];

			foreach (string str in temp)
			{
				m_Set[n] = str;
				n++;
			}
		}
		else
		{
			m_Set = null;
		}
		GeterateSummary();
		m_Summary = ToString();
	}

	public void Add(string value)
	{
		if (m_Set == null)
		{
			Set(new string[] { value});
		}
		else
		{
			string[] newVals = new string[Length + 1];
			Array.Copy(m_Set,newVals,Length);
			newVals[Length] = value;
			Set(newVals);
		}
	}

	public bool Contains(string test)
	{
		if (m_Set != null)
		{
			foreach (string str in m_Set)
			{
				if (String.Compare(str,test) == 0)
				{
					return true;
				}
			}
		}

		return false;
	}

	public bool Contains(StringSet testSet) //< test for complete containment of test set
	{
		//< naive implementation .. needs doing properly if it becomes a bottleneck
		//< assumes both filtersets are sorted 
		
		int localLim	= Length;
		int testLim		= testSet.Length;

		int locali		= 0;
		int testi		= 0;

		int hits = 0;

		while(locali < localLim && testi < testLim)
		{
			int compareResult = String.Compare(testSet[testi],m_Set[locali],false);

			if (compareResult < 0)
			{
				testi++;
			}
			else if (compareResult > 0)
			{
				locali++;
			}
			else 
			{
				hits++;
				testi++;
				locali++;
			}
		}						

		return hits == testSet.Length;
	}

	public bool Intersects(StringSet testSet)
	{
		if (testSet == null) return false;
		//< naive implementation .. needs doing properly if it becomes a bottleneck
		//< assumes both filtersets are sorted
		
		int localLim	= Length;
		int testLim		= testSet.Length;

		int locali		= 0;
		int testi		= 0;

		while(locali < localLim && testi < testLim)
		{
			int compareResult = String.Compare(testSet[testi],m_Set[locali],false);

			if (compareResult < 0)
			{
				testi++;
			}
			else if (compareResult > 0)
			{
				locali++;
			}
			else 
			{
				return true;
			}
		}						
		return false;
	}

	public StringSet GetIntersection(StringSet testSet)
	{
		//< naive implementation .. needs doing properly if it becomes a bottleneck
		//< assumes both filtersets are sorted
		
		int localLim	= Length;
		int testLim		= testSet.Length;

		int locali		= 0;
		int testi		= 0;

		List<string>	overlap = new List<string>();

		while(locali < localLim && testi < testLim)
		{
			int compareResult = String.Compare(testSet[testi],m_Set[locali],false);

			if (compareResult < 0)
			{
				testi++;
			}
			else if (compareResult > 0)
			{
				locali++;
			}
			else 
			{
				overlap.Add(m_Set[locali]);
			}
		}
		
		if (overlap.Count > 0)
		{
			string[] strOverlap = new string[overlap.Count];
			int n = 0;
			foreach (string str in overlap)
			{
				strOverlap[n] = str;
				n++;
			}
			return new StringSet(strOverlap);
		}

		return new StringSet();
	}

	public StringSet GetSubtraction(StringSet testSet)
	{
		//< naive implementation .. needs doing properly if it becomes a bottleneck
		//< assumes both filtersets are sorted
		
		int localLim	= Length;
		int testLim		= testSet.Length;

		int locali		= 0;
		int testi		= 0;

		List<string>	subtraction = new List<string>();

		while(locali < localLim && testi < testLim)
		{
			int compareResult = String.Compare(testSet[testi],m_Set[locali],false);

			if (compareResult < 0)
			{
				subtraction.Add(m_Set[testi]);
				testi++;
			}
			else if (compareResult > 0)
			{
				subtraction.Add(m_Set[locali]);
				locali++;
			}
		}
		
		if (subtraction.Count > 0)
		{
			string[] strSubtraction = new string[subtraction.Count];
			int n = 0;
			foreach (string str in subtraction)
			{
				strSubtraction[n] = str;
				n++;
			}
			return new StringSet(strSubtraction);
		}

		return new StringSet();
	}

	public IEnumerator<string> GetEnumerator()
	{
		foreach (string text in m_Set)
		{
			yield return text;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
