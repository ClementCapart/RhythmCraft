using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;

using System;
using System.Reflection;
using System.Collections.Generic;

[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		Transform transform = (Transform)target;

		EditorGUIUtility.LookLikeControls();
		EditorGUI.indentLevel = 0;

		GUILayout.BeginHorizontal();
			Vector3 position	= EditorGUILayout.Vector3Field("Position", transform.localPosition);
			if(GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(22.0f)))
			{
				position	= Vector3.zero;
			}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
			Vector3 eulerAngles	= EditorGUILayout.Vector3Field("Rotation", transform.localEulerAngles);
			if(GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(22.0f)))
			{
				eulerAngles	= Vector3.zero;
			}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
			Vector3 scale		= EditorGUILayout.Vector3Field("Scale", transform.localScale);
			if(GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(22.0f)))
			{
				scale	= Vector3.one;
			}
		GUILayout.EndHorizontal();

		OtherOptionsGUI(ref position, ref eulerAngles, transform);

		if(GUI.changed)
		{
			Undo.RecordObject(transform, "Transform Change");

			transform.localPosition		= FixIfNaN(position);
			transform.localEulerAngles	= FixIfNaN(eulerAngles);
			transform.localScale		= FixIfNaN(scale);
		}
	}

	private void OtherOptionsGUI(ref Vector3 position, ref Vector3 eulerRotation, Transform transform)
	{
		if (GUILayout.Button("Drop to ground"))
		{
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(transform.position + transform.up, transform.up * -1.0f, out hit, 1000.0f, 1 << LayerMask.NameToLayer("Terrain")))
			{
				transform.position = hit.point;
				position = transform.localPosition;
			}
		}

		if (GUILayout.Button("Align Rotation Along Ground Normal"))
		{
			RotateAlongFaceNormal(ref position, ref eulerRotation, false);
		}		
	}	

	private void RotateAlongFaceNormal(ref Vector3 position, ref Vector3 eulerRotation, bool zUp = false)
	{
		Vector3 gravity = -Vector3.up;
		Vector3 faceNormal = Vector3.zero;
		RaycastHit hitInfo;
		if (Physics.Raycast(position, gravity, out hitInfo, 1000.0f, 1 << LayerMask.NameToLayer("Terrain")))
		{
			faceNormal = hitInfo.normal;
		}

		if (zUp)
		{
			eulerRotation = Quaternion.FromToRotation(Vector3.forward, faceNormal).eulerAngles;
		}
		else
		{
			eulerRotation = Quaternion.FromToRotation(Vector3.up, faceNormal).eulerAngles;
		}
	}

	private Vector3 FixIfNaN(Vector3 v)
	{
		if (float.IsNaN(v.x))
		{
			v.x = 0;
		}
		if (float.IsNaN(v.y))
		{
			v.y = 0;
		}
		if (float.IsNaN(v.z))
		{
			v.z = 0;
		}
		return v;
	}
}
