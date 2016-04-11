using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class MathUtils
{
	public static float SqrtRetainSign(float x)
	{
		return Mathf.Sqrt(Mathf.Abs(x)) * SignExcludingZero(x);
	}

	public static bool SolveQuadraticMax(float a,float b,float c,ref float res)
    {
        //< solve quadratic and return highest result
 		float dSqr = b*b - 4.0f*a*c;
		if ((a * a) > 0.000001f)
		{
			if (dSqr > 0.0f)
			{
				float d = Mathf.Sqrt(dSqr);
				float inv2a = 1.0f / (2.0f * a);

				float t1 = (-b+d) * inv2a;
				float t2 = (-b-d) * inv2a;

				res = Mathf.Max(t1,t2);
				return true;
			}
			else if (Math.Abs(dSqr) < 0.00001f)
			{
				res = -b / (2.0f * a);
				return true;
			}
		}
		else if ((b * b) > 0.000001f)
		{
			res = -c / b;
			return true;
		}
		else if (Math.Abs(c) < 0.00001f)
		{
			res = 0.0f;
			return true;
		}

        return false;
    }

	public static bool SolveQuadraticMaxAboveZero(float a,float b,float c,ref float res)
    {
        //< solve quadratic and return highest result
 		float dSqr = b*b - 4.0f*a*c;
		if ((a * a) > 0.000001f)
		{
			if (dSqr > 0.0f)
			{
				float d = Mathf.Sqrt(dSqr);
				float inv2a = 1.0f / (2.0f * a);

				float t1 = (-b+d) * inv2a;
				float t2 = (-b-d) * inv2a;

				res = Mathf.Max(t1,t2);
				return res >= 0;
			}
			else if (Math.Abs(dSqr) < 0.00001f)
			{
				res = -b / (2.0f * a);
				return res >= 0;
			}
		}
		else if ((b * b) > 0.000001f)
		{
			res = -c / b;
			return res >= 0;
		}
		else if (Math.Abs(c) < 0.00001f)
		{
			res = 0.0f;
			return true;
		}

        return false;
    }

    public static bool SolveQuadraticMin(float a, float b, float c, ref float res)
    {
        //< solve quadratic and return highest result
 		float dSqr = b*b - 4.0f*a*c;
		if ((a * a) > 0.000001f)
		{
			if (dSqr > 0.0f)
			{
				float d = Mathf.Sqrt(dSqr);
				float inv2a = 1.0f / (2.0f * a);

				float t1 = (-b+d) * inv2a;
				float t2 = (-b-d) * inv2a;

				res = Mathf.Min(t1,t2);
				return true;
			}
			else if (Math.Abs(dSqr) < 0.00001f)
			{
				res = -b / (2.0f * a);
				return true;
			}
		}
		else if ((b * b) > 0.000001f)
		{
			res = -c / b;
			return true;
		}
		else if (Math.Abs(c) < 0.00001f)
		{
			res = 0.0f;
			return true;
		}

        return false;
    }

    public static bool SolveQuadratic(float a, float b, float c, ref float resLow, ref float resHigh)
    {
        //< solve quadratic and return highest result
 		float dSqr = b*b - 4.0f*a*c;
		if ((a * a) > 0.000001f)
		{
			if (dSqr > 0.0f)
			{
				float d = Mathf.Sqrt(dSqr);
				float inv2a = 1.0f / (2.0f * a);

				float t1 = (-b+d) * inv2a;
				float t2 = (-b-d) * inv2a;

				resHigh = Mathf.Max(t1,t2);
				resLow  = Mathf.Min(t1,t2);
				return true;
			}
			else if (Math.Abs(dSqr) < 0.00001f)
			{
				resHigh = resLow = -b / (2.0f * a);
				return true;
			}
		}
		else if ((b * b) > 0.000001f)
		{
			resHigh = resLow = -c / b;
			return true;
		}
		else if (Math.Abs(c) < 0.00001f)
		{
			resHigh = resLow = 0.0f;
			return true;
		}

        return false;
    }

	public static int SolveCubic (float a,float b,float c,float d,ref float res1,ref float res2,ref float res3)
	{
		//< calculates roots for Cubics in the form ax� + bx� + cx + d = 0 .. returns number of solutions
		if (a == 0)
		{
			//< it's just a quadratic .. solve that instead
			if (SolveQuadratic(b,c,d,ref res1,ref res2))
			{
				return 2;
			}
			return 0;
		} 

		float disc, q, r, dum1, s, t, term1, r13;

		b	/= a;
		c	/= a;
		d	/= a;
		q	= (3.0f * c - (b * b)) / 9.0f;
		r	= -(27.0f * d) + b * (9.0f * c - 2.0f * (b * b));
		r	/= 54.0f;

		disc	= q*q*q + r*r;
		term1	= (b / 3.0f);

		res1	= 0.0f;

		if (disc > 0) 
		{ 
			// one root real, two complex
			s		= r + Mathf.Sqrt(disc);
			s		= ((s < 0) ? -Mathf.Pow(-s, (1.0f / 3.0f)) : Mathf.Pow(s, (1.0f / 3.0f)));
			t		= r - Mathf.Sqrt(disc);
			t		= ((t < 0) ? -Mathf.Pow(-t, (1.0f / 3.0f)) : Mathf.Pow(t, (1.0f / 3.0f)));

			res1	= -term1 + s + t;
			return 1;
		} 
		
		//< The remaining options are all real
		if (disc == 0)
		{ 
			//< All roots real, at least two are equal.
			r13		= ((r < 0) ? -Mathf.Pow(-r,(1.0f / 3.0f)) : Mathf.Pow(r,(1.0f / 3.0f)));

			res1	= -term1 + 2.0f * r13;
			res2	= -(r13 + term1);
			res3	= res2;
			return 2;
		} 

		//< All roots are real and different
		q		= -q;
		dum1	= q*q*q;
		dum1	= Mathf.Acos(r / Mathf.Sqrt(dum1));
		r13		= 2.0f * Mathf.Sqrt(q);
		res1	= -term1 + r13 * Mathf.Cos(dum1 / 3.0f);
		res2	= -term1 + r13 * Mathf.Cos((dum1 + 2.0f * Mathf.PI) / 3.0f);
		res3	= -term1 + r13 * Mathf.Cos((dum1 + 4.0f * Mathf.PI) / 3.0f);
		return 3;
	}

	public static bool SolveCubicMax(float a,float b,float c,float d,ref float result)
	{
		float res1 = 0;
		float res2 = 0;
		float res3 = 0;

		int vals = SolveCubic(a,b,c,d,ref res1,ref res2,ref res3);
		
		if (vals == 3)
		{
			result = Mathf.Max(res1,Mathf.Max(res2,res3));
			return true;
		}
		else if (vals == 2)
		{
			result = Mathf.Max(res1,res2);
			return true;
		}
		else if (vals == 1)
		{
			result = res1;
			return true;
		}
		
		return false;
	}

	public static bool SolveCubicMin(float a,float b,float c,float d,ref float result)
	{
		float res1 = 0;
		float res2 = 0;
		float res3 = 0;

		int vals = SolveCubic(a,b,c,d,ref res1,ref res2,ref res3);
		
		if (vals == 3)
		{
			result = Mathf.Min(res1,Mathf.Min(res2,res3));
			return true;
		}
		else if (vals == 2)
		{
			result = Mathf.Min(res1,res2);
			return true;
		}
		else if (vals == 1)
		{
			result = res1;
			return true;
		}
		
		return false;
	}

	public static bool SolveCubicMinAboveZero(float a,float b,float c,float d,ref float result)
	{
		float res1 = 0;
		float res2 = 0;
		float res3 = 0;

		int vals = SolveCubic(a,b,c,d,ref res1,ref res2,ref res3);
		
		if (vals == 3)
		{
			float maxVal = Mathf.Max(res1,Mathf.Max(res2,res3));
			if (maxVal < 0.0f) return false; //< no result above zero;

			if (res1 < 0.0f) res1 = maxVal;
			if (res2 < 0.0f) res2 = maxVal;
			if (res3 < 0.0f) res3 = maxVal;

			result = Mathf.Min(res1,Mathf.Min(res2,res3));
			return true;
		}
		else if (vals == 2)
		{
			float maxVal = Mathf.Max(res1,res2);
			if (maxVal < 0.0f) return false; //< no result above zero;

			if (res1 < 0.0f) res1 = maxVal;
			if (res2 < 0.0f) res2 = maxVal;

			result = Mathf.Min(res1,res2);
			return true;
		}
		else if (vals == 1)
		{
			if (res1 < 0.0f) return false; //< no result above zero;

			result = res1;
			return true;
		}
		
		return false;
	}

	public static void ForceMinimumLength(ref Vector3 vect,float minLength)
	{
		if (vect.sqrMagnitude < (minLength * minLength))
		{
			vect = vect.normalized * minLength;
		}
	}

	public static void LimitLength(ref Vector3 vect,float maxLength)
	{
		if (vect.sqrMagnitude > (maxLength * maxLength))
		{
			vect = vect.normalized * maxLength;
		}
	}

	public static void LimitLength(ref Vector2 vect,float maxLength)
	{
		if (vect.sqrMagnitude > (maxLength * maxLength))
		{
			vect = vect.normalized * maxLength;
		}
	}

	public static Vector3 RetainSignAndSquare(Vector3 vect)
	{
		float magSq = vect.sqrMagnitude;
		if (magSq > 0.0f)
		{
			return vect.normalized * magSq;
		}

		return Vector3.zero;
	}

	public static Vector3 RetainSignAndSquareRoot(Vector3 vect)
	{
		float magSq = vect.magnitude;
		if (magSq > 0.0f)
		{
			return vect.normalized * Mathf.Sqrt(magSq);
		}

		return Vector3.zero;
	}

	public static Vector3 SafeNormalize(Vector3 vec,Vector3 fallback)
	{
		if (vec.sqrMagnitude > 0.0001f)
		{
			return vec.normalized;
		}
		
		return fallback;
	}

	public static Vector3 SafeNormalize(Vector3 vec)
	{
		if (vec.sqrMagnitude > 0.0001f)
		{
			return vec.normalized;
		}
		
		return Vector3.zero;
	}

	public static Quaternion SafeLookRotation(Vector3 fwd)
	{
		return SafeLookRotation(fwd, Vector3.up);
	}

	public static Quaternion SafeLookRotation(Vector3 fwd, Vector3 up)
	{
		Quaternion rotation = Quaternion.identity;

		if ((up.sqrMagnitude > 0.0001f) && (fwd.sqrMagnitude > 0.0001f))
		{
			fwd = fwd.normalized;
			up = up.normalized;

			if (Vector3.Dot(up, fwd) < 0.9999f)
			{
				rotation = Quaternion.LookRotation(fwd, up);
			}
		}

		return rotation;
	}

	public static Quaternion SafeLookRotation(Vector3 fwd, Vector3 up, Quaternion fallback)
	{
		Quaternion rotation = fallback;

		if ((up.sqrMagnitude > 0.0001f) && (fwd.sqrMagnitude > 0.0001f))
		{
			fwd = fwd.normalized;
			up = up.normalized;

			if (Vector3.Dot(up, fwd) < 0.9999f)
			{
				rotation = Quaternion.LookRotation(fwd, up);
			}
		}

		return rotation;
	}

	
	//------------------------------------------------------------------------ 
	//------------------------------------------------------------------------
	public static float AbsDifferenceOverSum(float a,float b)
	{
		return Mathf.Abs(a-b) / (a + b);
	}
	
	//------------------------------------------------------------------------ 
	// returns the range between the biggest and smallest angles within this triangle with respect to a given normal .. IN RADIANS!!
	//------------------------------------------------------------------------
	public static float AngleRangeRads(Vector3 v0,Vector3 v1,Vector3 v2)
	{
		float minVal = Mathf.PI;
		float maxVal = -Mathf.PI;

		Vector3 line01	= (v1 - v0).normalized;
		Vector3 line12	= (v2 - v1).normalized;
		Vector3 line20	= (v0 - v2).normalized;

		float ang012 = Vector3.Angle(line12,line01) * Mathf.Deg2Rad;
		float ang120 = Vector3.Angle(line20,line12) * Mathf.Deg2Rad;
		float ang201 = Vector3.Angle(line01,line20) * Mathf.Deg2Rad;

		minVal = Mathf.Min(minVal,ang012);
		minVal = Mathf.Min(minVal,ang120);
		minVal = Mathf.Min(minVal,ang201);

		maxVal = Mathf.Max(maxVal,ang012);
		maxVal = Mathf.Max(maxVal,ang120);
		maxVal = Mathf.Max(maxVal,ang201);

		return maxVal - minVal;
	}

	//------------------------------------------------------------------------ 
	// returns the range between the biggest and smallest sines of angles within this triangle
	//------------------------------------------------------------------------
	public static float FlatnessScore(Vector3 v0,Vector3 v1,Vector3 v2)
	{
		float minVal = Mathf.PI;
		float maxVal = -Mathf.PI;

		Vector3 line01	= (v1 - v0).normalized;
		Vector3 line12	= (v2 - v1).normalized;
		Vector3 line20	= (v0 - v2).normalized;

		float ang012 = Vector3.Angle(line12,line01) * Mathf.Deg2Rad;
		float ang120 = Vector3.Angle(line20,line12) * Mathf.Deg2Rad;
		float ang201 = Vector3.Angle(line01,line20) * Mathf.Deg2Rad;

		minVal = Mathf.Min(minVal,ang012);
		minVal = Mathf.Min(minVal,ang120);
		minVal = Mathf.Min(minVal,ang201);

		maxVal = Mathf.Max(maxVal,ang012);
		maxVal = Mathf.Max(maxVal,ang120);
		maxVal = Mathf.Max(maxVal,ang201);

		return (maxVal - minVal) / Mathf.PI;
	}

	public static bool IsConvexQuad(Vector3 v0,Vector3 v1,Vector3 v2,Vector3 v3,Vector3 normal)
	{
		Vector3 line0to1	= v1 - v0;
		Vector3 line1to2	= v2 - v1;
		Vector3 line2to3	= v3 - v2;
		Vector3 line3to0	= v0 - v3;

		Vector3 perp		= -Vector3.Cross(line1to2,line0to1);
		if (Vector3.Dot(perp,normal) < 0.0f) return false;

		perp		= -Vector3.Cross(line2to3,line1to2);
		if (Vector3.Dot(perp,normal) < 0.0f) return false;

		perp		= -Vector3.Cross(line3to0,line2to3);
		if (Vector3.Dot(perp,normal) < 0.0f) return false;

		perp		= -Vector3.Cross(line0to1,line3to0);
		if (Vector3.Dot(perp,normal) < 0.0f) return false;

		return true;
	}

	public static float ShortestEdgeLength(Vector3 v0,Vector3 v1,Vector3 v2)
	{
		float mag01Sq	= (v1 - v0).sqrMagnitude;
		float mag12Sq	= (v2 - v1).sqrMagnitude;
		float mag20Sq	= (v0 - v2).sqrMagnitude;
		
		float minMag	= (mag01Sq < mag12Sq) ? mag01Sq : mag12Sq;
		minMag			= (minMag < mag20Sq) ? minMag : mag20Sq;

		return Mathf.Sqrt(minMag);
	}

	public static float TriangleArea(Vector3 v0,Vector3 v1,Vector3 v2)
	{
		Vector3 edge10	= v1 - v0;
		Vector3 edge12	= v1 - v2;
		Vector3 perp	= Vector3.Cross(edge10.normalized,edge12.normalized);
		float res		= (edge10.magnitude * edge12.magnitude * 0.5f) * perp.magnitude;
		return res;
	}

	public static Vector3 TriangleNormal(Vector3 v0,Vector3 v1,Vector3 v2)
	{
		Vector3 edge10	= (v1 - v0).normalized;
		Vector3 edge12	= (v1 - v2).normalized;
		Vector3 res		= Vector3.Cross(edge12,edge10).normalized;
		return res;
	}

	public static bool TriangleIntersection(Vector3 rayStart,Vector3 rayEnd,Vector3 v0,Vector3 v1,Vector3 v2,ref Vector3 result)
	{
		Vector3		fullRay = rayEnd - rayStart;
		Vector3		rayDir	= (fullRay).normalized;

		// Vectors from p1 to p2/p3 (edges)
         Vector3 e1, e2;  
 
         Vector3 p, q, t;
         float det, invDet, u, v;
 
 
         //Find vectors for two edges sharing vertex/point p1
         e1 = v1 - v0;
         e2 = v2 - v0;
 
         // calculating determinant 
         p = Vector3.Cross(rayDir, e2);
 
         //Calculate determinat
         det = Vector3.Dot(e1, p);
 
         //if determinant is near zero, ray lies in plane of triangle otherwise not
         if (det > -Mathf.Epsilon && det < Mathf.Epsilon) 
		 { 
			 return false; 
		 }

         invDet = 1.0f / det;
 
         //calculate distance from p1 to ray origin
         t = rayStart - v0;
 
         //Calculate u parameter
         u = Vector3.Dot(t, p) * invDet;
 
         //Check for ray hit
         if (u < 0 || u > 1) { return false; }
 
         //Prepare to test v parameter
         q = Vector3.Cross(t, e1);
 
         //Calculate v parameter
         v = Vector3.Dot(rayDir, q) * invDet;
 
         //Check for ray hit
         if (v < 0 || u + v > 1) 
		 { 
			 return false; 
		 }
 
         if ((Vector3.Dot(e2, q) * invDet) > Mathf.Epsilon)
         { 
             //ray does intersect

			 float w = 1.0f - (u + v);

			 result = (v0 * u) + (v1 * v) + (v2 * w);

			 Vector3 resLine = result - rayStart;

			 if (resLine.sqrMagnitude <= fullRay.sqrMagnitude)
			 {
				 //< intersection exists within ray
				 return true;
			 }
         }
 
         // No hit at all
         return false;	
	}

	public static float GetAdjustmentByPlaneAlongAxis(Vector3 offset,Vector3 axis,Vector3 planeNormal)
	{
		float diff = Vector3.Dot(axis,planeNormal);
		
		Vector3 flatOffset = MathUtils.ProjectOntoPlane(offset,planeNormal);
		float dist		   = flatOffset.magnitude;
		float extra		   = dist / diff;

		return extra;
	}

	public static Vector3 AdjustByPlaneAlongAxis(Vector3 offset,Vector3 axis,Vector3 planeNormal)
	{
		float diff = Vector3.Dot(axis,planeNormal);
		
		Vector3 flatOffset = MathUtils.ProjectOntoPlane(offset,planeNormal);
		float dist		   = flatOffset.magnitude;
		float extra		   = dist / diff;

		return offset + (axis * extra);
	}

	public static Vector3 ProjectOntoPlaneIfBelow(Vector3 line,Vector3 planeNormal)
	{
		float planeDot = Vector3.Dot(line,planeNormal);
		if (planeDot < 0)
		{
			return line - (planeNormal * Vector3.Dot(line,planeNormal));
		}
		return line;
	}

	public static Vector3 ProjectOntoPlaneIfAbove(Vector3 line,Vector3 planeNormal)
	{
		float planeDot = Vector3.Dot(line,planeNormal);
		if (planeDot > 0)
		{
			return line - (planeNormal * Vector3.Dot(line,planeNormal));
		}
		return line;
	}

	public static Vector3 ProjectOntoPlane(Vector3 line,Vector3 planeNormal)
	{
		return line - (planeNormal * Vector3.Dot(line,planeNormal));
	}

    public static Vector3 ProjectOntoDirection(Vector3 input, Vector3 directionNormal)
    {
        return directionNormal * Vector3.Dot(input, directionNormal);
    }

	public static Vector3 NearpointOnLine(Vector3 p0,Vector3 p1,Vector3 test)
	{
		Vector3		line = p1;

		test -= p0;
		line -= p0;

		float lenSq		= line.sqrMagnitude;
		float t			= 0.0f;

		if (lenSq > 0.0f)
		{
			float offset	= Vector3.Dot(test,line);
			t				= Mathf.Clamp01(offset / lenSq);
		}

		Vector3 res = (line * t) + p0;

		return res;
	}

	public static float NearpointOnLine(Vector3 p0,Vector3 p1,Vector3 test,out Vector3 res)
	{
		Vector3		line = p1;

		test -= p0;
		line -= p0;

		float lenSq		= line.sqrMagnitude;
		float t			= 0.0f;

		if (lenSq > 0.0f)
		{
			float offset	= Vector3.Dot(test,line);
			t				= Mathf.Clamp01(offset / lenSq);
		}

		res = (line * t) + p0;

		return t;
	}

	public static Vector3 Barycentric(Vector3 test,Vector3 p0, Vector3 p1, Vector3 p2)
	{
		Vector3 v0 = p1 - p0;
		Vector3 v1 = p2 - p0;
		Vector3 v2 = test - p0;

		float d00 = Vector3.Dot(v0, v0);
		float d01 = Vector3.Dot(v0, v1);
		float d11 = Vector3.Dot(v1, v1);
		float d20 = Vector3.Dot(v2, v0);
		float d21 = Vector3.Dot(v2, v1);
		float denom = d00 * d11 - d01 * d01;

		float v = (d11 * d20 - d01 * d21) / denom;
		float w = (d00 * d21 - d01 * d20) / denom;
		float u = 1.0f - v - w;

		return new Vector3(u,v,w);
	}

	public static float Frac(float value)
	{
		return value - Mathf.Floor(value);
	}

    public static float SolveMotionForDistance(float vel, float acc, float time)
    {
        //< solve s = ut + (at^2) / 2

        float timeSqr = time * time;

        return (vel * time) + ((acc * timeSqr) * 0.5f);
    }

    public static Vector3 SolveMotionForDistance(Vector3 vel, Vector3 acc, float time)
    {
        //< solve s = ut + (at^2) / 2

        float timeSqr = time * time;

        return (vel * time) + ((acc * timeSqr) * 0.5f);
    }

    public static Vector3 SolveMotionForDistance(Vector3 vel, Vector3 acc, Vector3 jolt, float time)
    {
        //< solve s = ut + (at^2) / 2 + (jt^3) / 6

        float timeSqr = time * time;
        float timeCube = timeSqr * time;

        return (vel * time) + ((acc * timeSqr) * 0.5f) + ((jolt * timeCube) / 6.0f);
    }

    public static float SolveMotionForAcceleration(float vel, float distance, float time)
    {
        //< solve a = 2 * ((s - ut) / t^2)

        float timeSqr = time * time;

        return 2.0f * ((distance - (vel * time)) / timeSqr);
    }

    public static Vector3 SolveMotionForAcceleration(Vector3 vel, Vector3 distance, float time)
    {
        //< solve a = 2 * ((s - ut) / t^2)

        float timeSqr = time * time;

        return 2.0f * ((distance - (vel * time)) / timeSqr);
    }

    public static float SolveMotionForVelocity(float acceleration, float distance)
    {
        //< solve v^2 = 2as;
        return Mathf.Sqrt(2.0f * Mathf.Abs(acceleration) * distance);
    }

    public static Vector3 SolveMotionForVelocity(Vector3 acceleration, Vector3 distance)
    {
        //< solve v^2 = 2as;
        return SqrtVector(2.0f * MultiplyVectors(AbsVector(acceleration), distance));
    }

    public static float SolveMotionForVelocity(float currVelocity, float acceleration, float distance)
    {
        //< solve v^2 = u^2 + 2as;
        return Mathf.Sqrt((currVelocity * currVelocity) + (2.0f * Mathf.Abs(acceleration) * distance));
    }

    public static Vector3 SolveMotionForVelocity(Vector3 currVelocity, Vector3 acceleration, Vector3 distance)
    {
        //< solve v^2 = 2as;
        return SqrtVector(MultiplyVectors(currVelocity,currVelocity) + 2.0f * MultiplyVectors(AbsVector(acceleration), distance));
    }

    public static Vector3 MultiplyVectors(Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z);
    }

    public static Vector3 AbsVector(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public static Vector3 SqrtVector(Vector3 vector)
    {
        return new Vector3(Mathf.Sqrt(vector.x), Mathf.Sqrt(vector.y), Mathf.Sqrt(vector.z));
    }

	public static Vector3 FixIfNaNOrInfinite(Vector3 v)
	{
		if (float.IsNaN(v.x) || float.IsInfinity(v.x))
		{
			v.x = 0;
		}
		if (float.IsNaN(v.y) || float.IsInfinity(v.y))
		{
			v.y = 0;
		}
		if (float.IsNaN(v.z) || float.IsInfinity(v.z))
		{
			v.z = 0;
		}
		return v;
	}

	public static Vector3 TorqueToTurnTo(Transform subject, Transform target, Vector3 relAngVel, float extrapolation)
	{
		return TorqueToTurnTo(subject, target.rotation, relAngVel, extrapolation);
	}

	public static Vector3 TorqueToTurnTo(Transform subject, Quaternion target, Vector3 relAngVel, float extrapolation)
	{
		Quaternion inertiaTensotRot = Quaternion.identity;
		Vector3    inertiaTensor	= Vector3.one;

		if (subject.GetComponent<Rigidbody>())
		{
			inertiaTensotRot = subject.GetComponent<Rigidbody>().inertiaTensorRotation;
			inertiaTensor	 = subject.GetComponent<Rigidbody>().inertiaTensor;
		}

		Quaternion q	= subject.transform.rotation * inertiaTensotRot;

		Vector3 targetFwd	= target * Vector3.forward;
		Vector3 targetUp	= target * Vector3.up;
					
		//rotation extrapolation
		Vector3 v1		= subject.forward		+ Vector3.Cross(relAngVel, subject.forward)	* extrapolation;
		Vector3 v2		= targetFwd				- Vector3.Cross(relAngVel, targetFwd)	* extrapolation;
					
		Vector3 x		= Vector3.Cross(v1.normalized, v2.normalized);	//axis (normalized later)
		float theta		= Mathf.Asin(x.magnitude);						//angle
		Vector3 w		= x.normalized * theta / Time.fixedDeltaTime;		//ang.vel needed to get there in fixedDeltaTime
		Vector3 T		= q * Vector3.Scale(inertiaTensor, Quaternion.Inverse(q) * w);	//torque
					
		v1				= subject.up + Vector3.Cross(relAngVel, subject.up) * extrapolation;
		v2				= targetUp - Vector3.Cross(relAngVel, targetUp) * extrapolation;
		
		Vector3 y		= Vector3.Cross(v1.normalized, v2.normalized);
		theta			= Mathf.Asin(y.magnitude);
		w				= y.normalized * theta / Time.fixedDeltaTime;

		T				+= q * Vector3.Scale(inertiaTensor, Quaternion.Inverse(q) * w);

		T = FixIfNaNOrInfinite(T);
		return T;
	}

    /// <summary>
    /// Calculate the period of a given pendulum in seconds.
    /// </summary>
    /// <param name="length">Lengh of the string</param>
    /// <param name="oscillationAngle">Angle of oscillation, to help correct period at higher angles.</param>
    /// <param name="gravity">Value of gravity.</param>
    /// <returns>Pendulum period in seconds</returns>
    public static float CalculatePendulumPerdiod(float length, float oscillationAngle, float gravity)
    {
        // http://en.wikipedia.org/wiki/Pendulum

        float angle2 = oscillationAngle * oscillationAngle;
        float angle4 = angle2 * angle2;

        float c1 = 1.0f / 16.0f;
        float c2 = 11.0f / 3072.0f;

        return (Mathf.PI * 2.0f) * Mathf.Sqrt(length / gravity) * (1 + (c1 * angle2) + (c2 * angle4));
    }

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public static float CalcMinAngleDif(float startAng,float endAng)
	{
		const float TWO_PI	= 6.283185307f;
		const float PI		= 3.14159276f;

		if (Mathf.Abs(endAng - startAng) > TWO_PI)
		{
			startAng = startAng % TWO_PI;
			endAng	 = endAng % TWO_PI;

			if (startAng > PI)
			{
				startAng -= TWO_PI;
			}

			if (endAng > PI)
			{
				endAng -= TWO_PI;
			}

			if (startAng < -PI)
			{
				startAng += TWO_PI;
			}

			if (endAng < -PI)
			{
				endAng += TWO_PI;
			}
		}

		float up,down;

		if (endAng > startAng)
		{
			up		= endAng - startAng;
			down	= (endAng - TWO_PI) - startAng;
		}
		else
		{
			up		= (endAng + TWO_PI) - startAng;
			down	= endAng - startAng;
		}

		if (up < -down)
		{
			return up;
		}

		return down;
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public static void CalcAnglesFromDir(Vector3 source,ref float angY,ref float angX)
	{
		Vector3 dirXZ = new Vector3( source.x, 0.0f, source.z );
		Vector3 dirFull = source;

		dirXZ	= dirXZ.normalized;
		dirFull	= dirFull.normalized;

		angY = Mathf.Acos(dirXZ.z);
		angX = Mathf.Asin(dirFull.y);

		if (dirXZ.x < 0.0f)
		{
			angY = -angY;
		}
	}

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public static void CalcDirFromAngles(ref Vector3 dest,float angY,float angX)
	{
		dest.y = Mathf.Sin(angX);

		float	xzLen = Mathf.Sqrt(1.0f - (dest.y * dest.y));

		dest.x = Mathf.Sin(angY);
		dest.z = Mathf.Cos(angY);

		dest.x *= xzLen;
		dest.z *= xzLen;

		dest	= dest.normalized;
	}	

	//------------------------------------------------------------------------
	//------------------------------------------------------------------------ 
	public static Vector3 CalcDirFromAngles(float angY,float angX)
	{
		Vector3 dest = Vector3.zero;

		dest.y = Mathf.Sin(angX);

		float	xzLen = Mathf.Sqrt(1.0f - (dest.y * dest.y));

		dest.x = Mathf.Sin(angY);
		dest.z = Mathf.Cos(angY);

		dest.x *= xzLen;
		dest.z *= xzLen;

		dest	= dest.normalized;
		return dest;
	}	

	public static Vector3 ClosestPointOnLine(Vector3 point1, Vector3 point2, Vector3 testPoint)
	{
		Vector3 point1ToTestPoint	= testPoint - point1;
		Vector3 normPoint1ToPoint2	= (point2 - point1).normalized;
		float	lineLength			= Vector3.Distance(point1, point2);

		return point1 + Mathf.Clamp(Vector3.Dot(point1ToTestPoint, normPoint1ToPoint2), 0.0f, lineLength) * normPoint1ToPoint2;
	}

	public static float GetFitness(Vector3 testPos, Vector3 sourceLocation, Vector3 sourceDirection, float limitAngle, float cosLimitAngle, float limitRange)
	{
		Vector3 line = testPos - sourceLocation;
		float distSq = line.sqrMagnitude;
		float limitRangeSq = limitRange * limitRange;
		if (distSq < limitRangeSq)
		{
			float angDot = Vector3.Dot(line.normalized, sourceDirection);

			if (angDot >= cosLimitAngle)
			{
				float rangeFitness = 1 - (distSq / limitRangeSq);
				float ang = Mathf.Acos(angDot);
				float angleRatio = Mathf.Min(1.0f, ang / limitAngle);
				float angleFitness = Mathf.Sqrt(Mathf.Max(0.0f, 1.0f - (angleRatio * angleRatio)));
				return angleFitness * rangeFitness;
			}
		}

		return 0.0f;
	}

    public static float GetFitness(Vector3 testPos, Transform source,float limitAngle,float cosLimitAngle,float limitRange)
    {
		return GetFitness(testPos, source.position, source.forward, limitAngle, cosLimitAngle, limitRange);
    }

	public static float MaxByMagnitude(float x,float y)
	{
		if (Mathf.Abs(x) > Mathf.Abs(y))
		{
			return x;
		}
		return y;
	}

	public static float SignExcludingZero(float val)
	{
		if (val > 0.0f)
		{
			return 1.0f;
		}
		else if (val < -0.0f)
		{
			return -1.0f;
		}

		return 0.0f;
	}

	public static Vector3 RandomVectorWithinAngleInDirection(Vector3 vector, float angleDegs)
	{
        uint timeout = 100;
        float cosTest = Mathf.Cos(angleDegs * Mathf.Deg2Rad);

        while (timeout > 0)
        {
            timeout--;
            Vector3 randomVector = UnityEngine.Random.insideUnitSphere;
            Vector3 crossVector = Vector3.Cross(vector, randomVector).normalized;

            float s = UnityEngine.Random.value;
            float r = UnityEngine.Random.value;

            float h = Mathf.Cos(angleDegs * Mathf.Deg2Rad);

            float phi = 2 * Mathf.PI * s;
            float z = h + (1 - h) * r;
            float sinT = Mathf.Sqrt(1 - z * z);
            float x = Mathf.Cos(phi) * sinT;
            float y = Mathf.Sin(phi) * sinT;

            Vector3 res = (randomVector * x) + (crossVector * y) + (vector * z);

            if (Vector3.Dot(res.normalized, vector.normalized) > cosTest)
            {
                return res;
            }
        }
        return Vector3.zero;
	}

	public static bool LineIntersect2D(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,ref Vector2 res) 
	{
		// Store the values for fast access and easy
		// equations-to-code conversion
		float x1 = p1.x, x2 = p2.x, x3 = p3.x, x4 = p4.x;
		float y1 = p1.y, y2 = p2.y, y3 = p3.y, y4 = p4.y;
 
		float d = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
		// If d is zero, there is no intersection
		if (d == 0) return false;
 
		// Get the x and y
		float pre	= (x1*y2 - y1*x2);
		float post	= (x3*y4 - y3*x4);
		float x		= ( pre * (x3 - x4) - (x1 - x2) * post ) / d;
		float y		= ( pre * (y3 - y4) - (y1 - y2) * post ) / d;
 
		// Check if the x and y coordinates are within both lines
		if ( x < Mathf.Min(x1, x2) || x > Mathf.Max(x1, x2) || x < Mathf.Min(x3, x4) || x > Mathf.Max(x3, x4) ) return false;
		if ( y < Mathf.Min(y1, y2) || y > Mathf.Max(y1, y2) || y < Mathf.Min(y3, y4) || y > Mathf.Max(y3, y4) ) return false;
 
		// Return the point of intersection
		res.x = x;
		res.y = y;
		return true;
	}

    //------------------------------------------------------------------------
    //------------------For Hermite Spline Controller------------------------- 
    //------------------------------------------------------------------------ 
    public static float GetQuatLength(Quaternion q)
    {
        return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
    }

    public static Quaternion GetQuatConjugate(Quaternion q)
    {
        return new Quaternion(-q.x, -q.y, -q.z, q.w);
    }

    /// <summary>
    /// Logarithm of a unit quaternion. The result is not necessary a unit quaternion.
    /// </summary>
    public static Quaternion GetQuatLog(Quaternion q)
    {
        Quaternion res = q;
        res.w = 0;

        if (Mathf.Abs(q.w) < 1.0f)
        {
            float theta = Mathf.Acos(q.w);
            float sin_theta = Mathf.Sin(theta);

            if (Mathf.Abs(sin_theta) > 0.0001)
            {
                float coef = theta / sin_theta;
                res.x = q.x * coef;
                res.y = q.y * coef;
                res.z = q.z * coef;
            }
        }

        return res;
    }

    public static Quaternion GetQuatExp(Quaternion q)
    {
        Quaternion res = q;

        float fAngle = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z);
        float fSin = Mathf.Sin(fAngle);

        res.w = Mathf.Cos(fAngle);

        if (Mathf.Abs(fSin) > 0.0001)
        {
            float coef = fSin / fAngle;
            res.x = coef * q.x;
            res.y = coef * q.y;
            res.z = coef * q.z;
        }

        return res;
    }

	public static float ExponentialEase(float t,float power)
	{
		if (t < 0.5f)
		{
			return Mathf.Pow(2.0f * t,power) * 0.5f;
		}

		return 1.0f - Mathf.Pow(2.0f * (1.0f - t),power) * 0.5f;
	}

    /// <summary>
    /// SQUAD Spherical Quadrangle interpolation [Shoe87]
    /// </summary>
    public static Quaternion GetQuatSquad(float t, Quaternion q0, Quaternion q1, Quaternion a0, Quaternion a1)
    {
        float slerpT = 2.0f * t * (1.0f - t);

        Quaternion slerpP = Slerp(q0, q1, t);
        Quaternion slerpQ = Slerp(a0, a1, t);

        return Slerp(slerpP, slerpQ, slerpT);
    }

    public static Quaternion GetSquadIntermediate(Quaternion q0, Quaternion q1, Quaternion q2)
    {
        Quaternion q1Inv = GetQuatConjugate(q1);
        Quaternion p0 = GetQuatLog(q1Inv * q0);
        Quaternion p2 = GetQuatLog(q1Inv * q2);
        Quaternion sum = new Quaternion(-0.25f * (p0.x + p2.x), -0.25f * (p0.y + p2.y), -0.25f * (p0.z + p2.z), -0.25f * (p0.w + p2.w));

        return q1 * GetQuatExp(sum);
    }

    /// <summary>
    /// Smooths the input parameter t.
    /// If less than k1 ir greater than k2, it uses a sin.
    /// Between k1 and k2 it uses linear interp.
    /// </summary>
    public static float Ease(float t, float k1, float k2)
    {
        float f; float s;

        f = k1 * 2 / Mathf.PI + k2 - k1 + (1.0f - k2) * 2 / Mathf.PI;

        if (t < k1)
        {
            s = k1 * (2 / Mathf.PI) * (Mathf.Sin((t / k1) * Mathf.PI / 2 - Mathf.PI / 2) + 1);
        }
        else
            if (t < k2)
            {
                s = (2 * k1 / Mathf.PI + t - k1);
            }
            else
            {
                s = 2 * k1 / Mathf.PI + k2 - k1 + ((1 - k2) * (2 / Mathf.PI)) * Mathf.Sin(((t - k2) / (1.0f - k2)) * Mathf.PI / 2);
            }

        return (s / f);
    }

    /// <summary>
    /// We need this because Quaternion.Slerp always uses the shortest arc.
    /// </summary>
    public static Quaternion Slerp(Quaternion p, Quaternion q, float t)
    {
        Quaternion ret;

        float fCos = Quaternion.Dot(p, q);

        if ((1.0f + fCos) > 0.00001)
        {
            float fCoeff0, fCoeff1;

            if ((1.0f - fCos) > 0.00001)
            {
                float omega = Mathf.Acos(fCos);
                float invSin = 1.0f / Mathf.Sin(omega);
                fCoeff0 = Mathf.Sin((1.0f - t) * omega) * invSin;
                fCoeff1 = Mathf.Sin(t * omega) * invSin;
            }
            else
            {
                fCoeff0 = 1.0f - t;
                fCoeff1 = t;
            }

            ret.x = fCoeff0 * p.x + fCoeff1 * q.x;
            ret.y = fCoeff0 * p.y + fCoeff1 * q.y;
            ret.z = fCoeff0 * p.z + fCoeff1 * q.z;
            ret.w = fCoeff0 * p.w + fCoeff1 * q.w;
        }
        else
        {
            float fCoeff0 = Mathf.Sin((1.0f - t) * Mathf.PI * 0.5f);
            float fCoeff1 = Mathf.Sin(t * Mathf.PI * 0.5f);

            ret.x = fCoeff0 * p.x - fCoeff1 * p.y;
            ret.y = fCoeff0 * p.y + fCoeff1 * p.x;
            ret.z = fCoeff0 * p.z - fCoeff1 * p.w;
            ret.w = p.z;
        }

        return ret;
    }

    public static void BoundingSphereOfPoints(List<Vector3> points, out Vector3 boundingSphereCentre, out float boundingSphereRadius)
    {
        //Calculate bounding sphere
        boundingSphereCentre = new Vector3();
        boundingSphereRadius = 0.0f;
        foreach (Vector3 point in points)
        {
            Vector3 sphereCentre = point;

            if (boundingSphereCentre == Vector3.zero)
            {
                boundingSphereCentre = sphereCentre;
            }
            else
            {
                float distBetweenCentres    = (boundingSphereCentre - sphereCentre).magnitude;
                Vector3 dirBetweenCentres   = (sphereCentre - boundingSphereCentre).normalized;
                float newRadius             = (distBetweenCentres + boundingSphereRadius) / 2;
                float radiusDifference      = Mathf.Max(0.0f, newRadius - boundingSphereRadius);

                if (newRadius > boundingSphereRadius)
                {
                    boundingSphereRadius = newRadius;
                    boundingSphereCentre = boundingSphereCentre + dirBetweenCentres * radiusDifference;
                }
            }
        }
    }

    public static void BoundingSphereOfPoints(Vector3[] points, out Vector3 boundingSphereCentre, out float boundingSphereRadius)
    {
        //Calculate bounding sphere
        boundingSphereCentre = new Vector3();
        boundingSphereRadius = 0.0f;
        foreach (Vector3 point in points)
        {
            Vector3 sphereCentre = point;

            if (boundingSphereCentre == Vector3.zero)
            {
                boundingSphereCentre = sphereCentre;
            }
            else
            {
                float distBetweenCentres    = (boundingSphereCentre - sphereCentre).magnitude;
                Vector3 dirBetweenCentres   = (sphereCentre - boundingSphereCentre).normalized;
                float newRadius             = (distBetweenCentres + boundingSphereRadius) / 2;
                float radiusDifference      = Mathf.Max(0.0f, newRadius - boundingSphereRadius);

                if (newRadius > boundingSphereRadius)
                {
                    boundingSphereRadius = newRadius;
                    boundingSphereCentre = boundingSphereCentre + dirBetweenCentres * radiusDifference;
                }
            }
        }
    }

    public static float MaxAngle(Vector3 pointFrom, List<Vector3> points)
    {
        float largestAngle = 0;

        //Two directions which are the furthest apart from each other
        Vector3 dir1 = new Vector3();
        Vector3 dir2 = new Vector3();

		int lim = points.Count;
		for (int i = 0; i < lim; ++i)
		{
			Vector3 dir = (pointFrom - points[i]).normalized;

			if (dir1 == Vector3.zero)
			{
				dir1 = dir;
			}
			else if (dir2 == Vector3.zero)
			{
				dir2 = dir;
				largestAngle = Vector3.Angle(dir1, dir2);
			}
			else
			{
				NewAngle(dir, ref largestAngle, ref dir1, ref dir2);
			}
		}

        return largestAngle;
    }

    private static void NewAngle(Vector3 newDir, ref float largestAngle, ref Vector3 dir1, ref Vector3 dir2)
    {
        float newAngle1 = Vector3.Angle(dir1, newDir);
        float newAngle2 = Vector3.Angle(dir2, newDir);

        //If the new direction has a greater angle between either of the current directions, replace the direction, resulting in the largest new angle
        if (newAngle1 > largestAngle || newAngle2 > largestAngle)
        {
            if (newAngle1 > newAngle2)
            {
                dir2 = newDir;
                largestAngle = newAngle1;
            }
            else
            {
                dir1 = newDir;
                largestAngle = newAngle2;
            }
        }
    }

	public static string GetMd5Hash(string input)
	{
		MD5 md5Hash = MD5.Create();
		// Convert the input string to a byte array and compute the hash. 
		byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

		// Create a new Stringbuilder to collect the bytes 
		// and create a string.
		StringBuilder sBuilder = new StringBuilder();

		// Loop through each byte of the hashed data  
		// and format each one as a hexadecimal string.
		for (int i = 0; i < data.Length; i++)
		{
			sBuilder.Append(data[i].ToString("x2"));
		}

		// Return the hexadecimal string. 
		return sBuilder.ToString();
	}

	public static float RoundUp(float value, float roundUp)
	{
		if (value > 0)
		{
			return Mathf.Ceil(value / roundUp) * roundUp;
		}
		else if (value < 0)
		{
			return Mathf.Floor(value / roundUp) * roundUp;
		}
		else
		{
			return roundUp;
		}
	}

	public static float RoundDown(float value, float roundDown)
	{
		if (value < 0)
		{
			return Mathf.Ceil(value / roundDown) * roundDown;
		}
		else if (value > 0)
		{
			return Mathf.Floor(value / roundDown) * roundDown;
		}
		else
		{
			return roundDown;
		}
	}

	public static float MeanAngle(float[] angles)
	{
		float y_part = 0.0f;
		float x_part = 0.0f;

		int numAngles = angles.Length;
		for (int i = 0; i < numAngles; i++)
		{
			x_part += Mathf.Cos(angles[i]);
			y_part += Mathf.Sin(angles[i]);
		}

		return Mathf.Atan2(y_part / numAngles, x_part / numAngles);
	}

	public static float WeightedAverageAngle(float[] angles, int[] weights)
	{
		float y_part = 0.0f;
		float x_part = 0.0f;

		int totalWeight = 0;

		int numAngles = angles.Length;
		for (int i = 0; i < numAngles; i++)
		{
			if (i > weights.Length)
			{
				//Assume all other weights would have been 0, so return current result
				break;
			}
			else
			{
				totalWeight += weights[i];
				x_part += Mathf.Cos(angles[i]) * weights[i];
				y_part += Mathf.Sin(angles[i]) * weights[i];
			}
		}

		return Mathf.Atan2(y_part / totalWeight, x_part / totalWeight);
	}

	public static Vector3 ExpectMatch(Vector3 value,Vector3 test,GameObject context)
	{
		if (Mathf.Abs(value.magnitude - test.magnitude) > 0.001f)
		{
			Debug.LogWarning("Vector LengthTest Failed : Magnitude",context);
		}

		if (Vector3.Dot(value,test) < 0.999f)
		{
			Debug.LogWarning("Vector LengthTest Failed : Direction",context);
		}

		return value;
	}

	public static float ExpectMatch(float value,float test,GameObject context)
	{
		if (Mathf.Abs(value - test) > 0.001f)
		{
			Debug.LogWarning("Float Match Failed : Magnitude",context);
		}

		return value;
	}

	public static Vector3 WeightedAverageVector(Vector3[] vectors, int[] weights)
	{
		Vector3 vector = Vector3.zero;
		int totalWeight = 0;

		int numVectors = vectors.Length;
		for (int i = 0; i < numVectors; i++)
		{
			if (i > weights.Length)
			{
				//Assume all other weights would have been 0, so return current result
				break;
			}
			else
			{
				totalWeight += weights[i];
				vector += vectors[i] * weights[i];
			}
		}

		return vector / totalWeight;
	}

	public static int iLog2(int v)
	{
		int r; 
		int shift;

		r =     ((v > 0xFFFF) ? 1 : 0) << 4; 
		v >>= r;
		shift = ((v > 0xFF  ) ? 1 : 0) << 3; 
		v >>= shift; 
		r |= shift;
		shift = ((v > 0xF   ) ? 1 : 0) << 2; 
		v >>= shift; 
		r |= shift;
		shift = ((v > 0x3   ) ? 1 : 0) << 1; 
		v >>= shift; 
		r |= shift;

		r |= (v >> 1);

		return r;
	}

	public static int NextPowerOfTwo(int v)
	{
		if (v < 0) return 1;
		if (v == 0) return 1;
		if (v == (v & (~v+1))) return v;

		v--;
		v |= v >> 1;
		v |= v >> 2;
		v |= v >> 4;
		v |= v >> 8;
		v |= v >> 16;
		v++;

		return v;
	}

	public static bool IsPowerOfTwo(int v) 
	{ 
		if (v < 0) return false;
		return v == (v & (~v+1));
	}

	public static bool IsPowerOfTwo(uint v) 
	{ 
		return v == (v & (~v+1));
	}

	public static uint BinaryToGray(uint v)
	{
		return (v >> 1) ^ v;
	}

	public static uint GrayToBinary(uint v)
	{
		v ^= (v >> 16);
		v ^= (v >> 8);
		v ^= (v >> 4);
		v ^= (v >> 2);
		v ^= (v >> 1);
		return v;
	}

	public static int BinaryToGray(int v)
	{
		return (v >> 1) ^ v;
	}

	public static int GrayToBinary(int v)
	{
		v ^= (v >> 16);
		v ^= (v >> 8);
		v ^= (v >> 4);
		v ^= (v >> 2);
		v ^= (v >> 1);
		return v;
	}

	public static int IntPow(int x, int pow)
	{
	    int ret = 1;
	    while ( pow != 0 )
		{
		    if ( (pow & 1) == 1 )
			{
		        ret *= x;
			}
		    x *= x;
	        pow >>= 1;
		}
		return ret;
	}

	public static Vector3 GetPositionOnBezier(float t, Vector3 p0, Vector3 anchor0, Vector3 anchor1, Vector3 p1)
	{
		float u = 1- t;
		float tt = t*t;
		float uu = u*u;
		float uuu = uu* u;
		float ttt = tt * t;

		Vector3 p = uuu * p0;
		p+= 3* uu * t * anchor0;
		p += 3* u * tt * anchor1;
		p += ttt * p1;

		return p;
	}

	public static Quaternion GetLookRotatationOnBezier(float t, Vector3 p0, Vector3 anchor0, Vector3 anchor1, Vector3 p1, float precision, Vector3 up)
	{
		Vector3 firstPosition = GetPositionOnBezier(t, p0, anchor0, anchor1, p1);
		Vector3 secondPosition = GetPositionOnBezier(t + precision, p0, anchor0, anchor1, p1);

		return Quaternion.LookRotation((secondPosition - firstPosition).normalized, up);
	}
}
