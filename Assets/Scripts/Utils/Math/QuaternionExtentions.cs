using UnityEngine;
using System.Collections;

public static class QuaternionExtentions
{
	public	static	Vector3	Forward(this Quaternion rotation)
	{
		return rotation * Vector3.forward;
	}

	public static Vector3 Right(this Quaternion rotation)
	{
		return rotation * Vector3.right;
	}

	public static Vector3 Up(this Quaternion rotation)
	{
		return rotation * Vector3.up;
	}
}
