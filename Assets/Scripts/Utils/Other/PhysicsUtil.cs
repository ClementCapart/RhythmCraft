using UnityEngine;
using System.Collections;

public class PhysicsUtil
{
    public static float CalcDragFromTerminalVel(float mass, float terminalVelocity)
    {
        //0.5 in drag equation cancels out 2
        return (mass * Physics.gravity.magnitude) / (terminalVelocity * terminalVelocity);
    }


    public static float CalculateImpactSpeedFromHeight(float height, float terminalVelocity)
    {
        return terminalVelocity * Mathf.Sqrt(1.0f - Mathf.Exp((-2.0f * Physics.gravity.magnitude * height) / (terminalVelocity * terminalVelocity)));
    }

    public static float CaclulateImpactEnergyFromHeight(float height, float terminalVelocity, float mass)
    {
        float impactVelocity = CalculateImpactSpeedFromHeight(height, terminalVelocity);
        return 0.5f * mass * impactVelocity * impactVelocity;
    }

	public static bool RaycastNearest(Vector3 rayPos,Vector3 rayDir,float distance,ref RaycastHit res, int layerMask)
	{
		RaycastHit[] hits = Physics.RaycastAll(rayPos, rayDir, distance, layerMask);
		//order is not guaranteed; loop through and pick nearest non-self non-trigger
		float minDist	= distance;
		bool found		= false;

		int lim = hits.Length;
		for (int i = 0;i < lim;i++)
		{
			RaycastHit hit = hits[i];
			if (!hit.collider.isTrigger)
			{
				if (hit.distance < minDist)
				{
					found	= true;
					minDist = hit.distance;
					res		= hit;
				}
			}
		}

		return found;
	}

}

[System.Serializable]
public class CachedRigidBody 
{
	private Rigidbody			m_rigidBody	= null;
	private Transform			m_owner		= null;

	public Rigidbody Value { get { return m_rigidBody;}}

	public Rigidbody Reget(Transform owner) 
	{ 
		m_owner = null;
		return Get(owner);
	}

	public Rigidbody Get(Transform owner) 
	{ 
		if (m_owner == owner)
		{
			return m_rigidBody;
		}

		m_owner	= owner;

		if (owner)
		{
			m_rigidBody = owner.GetComponent<Rigidbody>();
			return m_rigidBody;
		}
		else
		{
			m_rigidBody = null;
		}

		return null;
	}
}

[System.Serializable]
public class CachedAudioSource 
{
	private AudioSource			m_source	= null;
	private Transform			m_owner		= null;

	public AudioSource Reget(Transform owner) 
	{ 
		m_owner = null;
		return Get(owner);
	}

	public AudioSource Get(Transform owner) 
	{ 
		if (m_owner == owner)
		{
			return m_source;
		}

		m_owner = owner;
		if (owner)
		{
			m_source = owner.GetComponent<AudioSource>();
			return m_source;
		}
		else
		{
			m_source = null;
		}

		return null;
	}

}

public class CachedMainCamera 
{
	private Camera				m_camera	= null;
	private Transform			m_transform	= null;

	public void Reset() 
	{ 
		m_camera	= null;
		m_transform = null;
	}

	public bool Exists { get { return Camera.main != null;}}

	public Camera camera 
	{ 
		get
		{
			if (m_camera == null)
			{
				m_camera = Camera.main;
			}
			return m_camera;
		}
	}

	public Transform transform 
	{ 
		get
		{
			if (m_transform == null)
			{
				m_transform = Camera.main.transform;
			}
			return m_transform;
		}
	}
}
