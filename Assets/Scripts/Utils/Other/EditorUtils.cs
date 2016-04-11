using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Reflection;
using System.Collections.Generic;

#if UNITY_EDITOR
public class EditorUtils
{
	public class CompoundGUI
	{
		public class ArrayInspector<ArrayType> where ArrayType : new()
		{
			// Return true to delete this item
			public	delegate	bool OnItemGUI(ArrayType item);

			public	void OnGUI(ref ArrayType[] array, OnItemGUI itemGUI)
			{
				foreach (ArrayType item in array)
				{
					bool deleteItem = itemGUI(item);

					if(deleteItem)
					{
						ArrayUtility.Remove<ArrayType>(ref array, item);
						break;
					}
				}

				EditorUtils.CompoundGUI.PaddedHorizontalLine(2.0f);

				if(GUILayout.Button("Add"))
				{
					ArrayType	newObject	= new ArrayType();
					ArrayUtility.Add<ArrayType>(ref array, newObject);
				}
			}
		}

		public static void HorizontalLine(float thickness = 1.0f)
		{
			GUIStyle lineStyle = new GUIStyle();
			lineStyle.normal.background = EditorGUIUtility.whiteTexture;
			lineStyle.stretchWidth = true;
			lineStyle.margin = new RectOffset(0, 0, 3, 0);

			Rect position = GUILayoutUtility.GetRect(GUIContent.none, lineStyle, GUILayout.Height(thickness));

			if (Event.current.type == EventType.Repaint)
			{
				Color oldColor = GUI.color;
				GUI.color = new Color(0.5f, 0.5f, 0.5f);
				lineStyle.Draw(position, false, false, false, false);
				GUI.color = oldColor;
			}
		}

		public static void VerticalLine(float thickness = 1.0f)
		{
			GUIStyle lineStyle = new GUIStyle();
			lineStyle.normal.background = EditorGUIUtility.whiteTexture;
			lineStyle.stretchWidth = true;
			lineStyle.margin = new RectOffset(0, 0, 3, 0);

			Rect position = GUILayoutUtility.GetRect(GUIContent.none, lineStyle,
														new GUILayoutOption[] { GUILayout.Width(thickness),
																				GUILayout.ExpandHeight(true) });

			if (Event.current.type == EventType.Repaint)
			{
				Color oldColor = GUI.color;
				GUI.color = new Color(0.5f, 0.5f, 0.5f);
				lineStyle.Draw(position, false, false, false, false);
				GUI.color = oldColor;
			}
		}

		public static void PaddedVerticalLine(float pad = 5.0f, float thickness = 1.0f)
		{
			GUILayout.Space(pad);

			VerticalLine(thickness);

			GUILayout.Space(pad);
		}

		public static void PaddedHorizontalLine(float pad = 5.0f, float thickness = 1.0f)
		{
			GUILayout.Space(pad);

			HorizontalLine(thickness);

			GUILayout.Space(pad);
		}

		public static bool BeginFadeGroup(float foldout)
		{
			return EditorGUILayout.BeginFadeGroup(foldout);
		}

		public static void EndFadeGroup()
		{
			EditorGUILayout.EndFadeGroup();
		}

		public static void AddMouseIcon()
		{
			Rect mouseRect = GUILayoutUtility.GetLastRect();
			EditorGUIUtility.AddCursorRect(mouseRect, MouseCursor.ArrowPlus);
		}

		public static void RemoveMouseIcon()
		{
			Rect mouseRect = GUILayoutUtility.GetLastRect();
			EditorGUIUtility.AddCursorRect(mouseRect, MouseCursor.ArrowMinus);
		}

		public static void MinMaxSlider(FloatRange range, float min = 0.0f, float max = 1.0f)
		{
			GUILayout.BeginHorizontal();
			{
				Color oldColor = GUI.color;

				GUI.color = range.IsValid ? oldColor : Color.red;

				float minimum = range.Min;
				float maximum = range.Max;

				minimum = EditorGUILayout.FloatField(minimum, GUILayout.Width(50.0f));

				EditorGUILayout.MinMaxSlider(ref minimum, ref maximum, min, max);

				maximum = EditorGUILayout.FloatField(maximum, GUILayout.Width(50.0f));

				minimum = float.Parse(minimum.ToString("F"));
				maximum = float.Parse(maximum.ToString("F"));

				range.SetRaw(minimum, maximum);

				GUI.color = oldColor;
			}
			GUILayout.EndHorizontal();
		}

		public static void MinMaxSlider(IntRange range, int min = 0, int max = 5)
		{
			GUILayout.BeginHorizontal();
			{
				Color oldColor = GUI.color;

				GUI.color = range.IsValid ? oldColor : Color.red;

				float minimum = range.Min;
				float maximum = range.Max;

				minimum = (float)EditorGUILayout.IntField((int)minimum, GUILayout.Width(50.0f));

				EditorGUILayout.MinMaxSlider(ref minimum, ref maximum, min, max);

				maximum = (float)EditorGUILayout.IntField((int)maximum, GUILayout.Width(50.0f));

				minimum = float.Parse(minimum.ToString("F"));
				maximum = float.Parse(maximum.ToString("F"));

				range.SetRaw((int)minimum, (int)maximum);

				GUI.color = oldColor;
			}
			GUILayout.EndHorizontal();
		}
	}

	public	class HandlesX
	{
		private string selectedHandle;

		bool SelectionHandle(ref Vector3 position, Quaternion rotation, string name, float size = 0.5f)
		{
			string currentGUIName = GUI.GetNameOfFocusedControl();

			if (currentGUIName == name)
			{
				selectedHandle = currentGUIName;
			}

			if (selectedHandle == name)
			{
				position = PositionHandle(position, rotation, name);
				return true;
			}
			else
			{
				NormalHandle(position, name, size);
				return false;
			}
		}

		private Vector3 PositionHandle(Vector3 position, Quaternion rotation, string name)
		{
			GUI.SetNextControlName(name);
			Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.75f);
			Vector3 newPos = Handles.Slider(position, rotation * Vector3.right);

			GUI.SetNextControlName(name);
			Handles.color = new Color(0.0f, 1.0f, 0.0f, 0.75f);
			newPos += Handles.Slider(position, rotation * Vector3.up) - position;

			GUI.SetNextControlName(name);
			Handles.color = new Color(0.0f, 0.0f, 1.0f, 0.75f);
			newPos += Handles.Slider(position, rotation * Vector3.forward) - position;

			float sizeMultiplier = 0.0f;

			if (Camera.current.orthographic)
			{
				sizeMultiplier = Camera.current.orthographicSize * 2.5f;
			}
			else
			{
				Plane screen = new Plane(Camera.current.transform.forward, Camera.current.transform.position);
				screen.Raycast(new Ray(position, Camera.current.transform.forward), out sizeMultiplier);
			}

			GUI.SetNextControlName(name);
			Handles.color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
			newPos += Handles.FreeMoveHandle(position, rotation, 0.02f * sizeMultiplier, Vector3.zero, Handles.RectangleCap) - position;

			return newPos;
		}

		private void NormalHandle(Vector3 position, string name, float size)
		{
			GUI.SetNextControlName(name);
			Handles.FreeMoveHandle(position, Quaternion.identity, size, Vector3.zero, Handles.SphereCap);
		}
	}
	
	public class JobSystem
	{
		public abstract class JobBase
		{
			public abstract void Execute();
		}

		public class Job<param1Type, param2Type> : JobBase
		{
			public delegate void JobFunction(param1Type param1, param2Type param2);

			private JobFunction m_function;
			private param1Type m_param1;
			private param2Type m_param2;

			public Job(JobFunction function, param1Type param1, param2Type param2)
			{
				m_function = function;
				m_param1 = param1;
				m_param2 = param2;
			}

			public override void Execute()
			{
				m_function(m_param1, m_param2);
			}
		}

		private List<JobBase> m_jobs = new List<JobBase>();

		public void AddJob(JobBase job)
		{
			m_jobs.Add(job);
		}

		public bool RunJobs()
		{
			int jobCount = m_jobs.Count;

			foreach (JobBase job in m_jobs)
			{
				job.Execute();
			}

			m_jobs.Clear();

			return jobCount != 0;
		}

		//// JOBS ////

		public void DeleteAtIndexList<type>(List<type> list, int index)
		{
			list.RemoveAt(index);
		}

		public void DeleteObjectList<type>(List<type> list, type objectToDelete)
		{
			list.Remove(objectToDelete);
		}
	}

	public static void DrawPoint(Vector3 position)
	{
		DrawPoint(position, Color.white);
	}

	public static void DrawPoint(Vector3 position, Color color, float duration = 0.0f)
	{
		Debug.DrawLine(position + Vector3.left,		position + Vector3.right,	color, duration);
		Debug.DrawLine(position + Vector3.up,		position + Vector3.down,	color, duration);
		Debug.DrawLine(position + Vector3.up,		position + Vector3.down,	color, duration);
		Debug.DrawLine(position + Vector3.forward,	position + Vector3.back,	color, duration);
	}

	public static void CreateAsset<T>() where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T>();

		string path = AssetDatabase.GetAssetPath(Selection.activeObject);
		if (path == "")
		{
			path = "Assets";
		}
		else if (Path.GetExtension(path) != "")
		{
			path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
		}

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

		AssetDatabase.CreateAsset(asset, assetPathAndName);

		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}

	public static string ConvertFolderStringToAssetFolderRelativeString(string path)
	{
		string[] strings = path.Split('/');

		int iter = 0;
		while (iter < strings.Length && strings[iter] != "Assets")
		{
			iter++;
		}

		if (iter < strings.Length)
		{
			string assetPath = string.Join("/", strings, iter, strings.Length - iter);
			return assetPath;
		}
		else
		{
			return null;
		}
	}

	public static void ScriptField(Object target)
	{
		EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target as MonoBehaviour), typeof(MonoScript), false);
	}
}
#endif
