using UnityEngine;
using System.Collections;
using System;

public static class Easing
{ 
	// Adapted from source : http://www.robertpenner.com/easing/
	
	public static float Ease (double linearStep, float acceleration, EasingType type)
	{
		float easedStep = acceleration > 0 ? EaseIn (linearStep, type) : 
			acceleration < 0 ? EaseOut (linearStep, type) : 
				(float)linearStep;
		
		return EasingMathHelper.Lerp (linearStep, easedStep, Math.Abs (acceleration));
	}
	
	public static float EaseIn (double linearStep, EasingType type)
	{
		linearStep = Mathf.Clamp01 ((float)linearStep);
		switch (type) {
		case EasingType.Step:
			return linearStep < 0.5 ? 0 : 1;
		case EasingType.Linear:
			return (float)linearStep;
		case EasingType.Sine:
			return Sine.EaseIn (linearStep);
		case EasingType.Quadratic:
			return Power.EaseIn (linearStep, 2);
		case EasingType.Cubic:
			return Power.EaseIn (linearStep, 3);
		case EasingType.Quartic:
			return Power.EaseIn (linearStep, 4);
		case EasingType.Quintic:
			return Power.EaseIn (linearStep, 5);
		}
		throw new NotImplementedException ();
	}
	
	public static float EaseOut (double linearStep, EasingType type)
	{
		linearStep = Mathf.Clamp01 ((float)linearStep);
		switch (type) {
		case EasingType.Step:
			return linearStep < 0.5 ? 0 : 1;
		case EasingType.Linear:
			return (float)linearStep;
		case EasingType.Sine:
			return Sine.EaseOut (linearStep);
		case EasingType.Quadratic:
			return Power.EaseOut (linearStep, 2);
		case EasingType.Cubic:
			return Power.EaseOut (linearStep, 3);
		case EasingType.Quartic:
			return Power.EaseOut (linearStep, 4);
		case EasingType.Quintic:
			return Power.EaseOut (linearStep, 5);
		}
		throw new NotImplementedException ();
	}
	
	public static float EaseWithReturn (double linearStep, EasingType ease)
	{
		linearStep = (double)Mathf.Clamp01 ((float)linearStep);
		var amount = linearStep > 0.5 ? 1 + ((0.5 - linearStep) * 2) : (linearStep * 2);
		return EaseOut (amount, ease);
	}
	
	public static float EaseInOut (double linearStep, EasingType easeInType, EasingType easeOutType)
	{
		linearStep = Mathf.Clamp01 ((float)linearStep);
		return linearStep < 0.5 ? EaseInOut (linearStep, easeInType) : EaseInOut (linearStep, easeOutType);
	}
	
	public static float EaseInOut (double linearStep, EasingType type)
	{
		linearStep = Mathf.Clamp01 ((float)linearStep);
		switch (type) {
		case EasingType.Step:
			return linearStep < 0.5 ? 0 : 1;
		case EasingType.Linear:
			return (float)linearStep;
		case EasingType.Sine:
			return Sine.EaseInOut (linearStep);
		case EasingType.Quadratic:
			return Power.EaseInOut (linearStep, 2);
		case EasingType.Cubic:
			return Power.EaseInOut (linearStep, 3);
		case EasingType.Quartic:
			return Power.EaseInOut (linearStep, 4);
		case EasingType.Quintic:
			return Power.EaseInOut (linearStep, 5);
		}
		throw new NotImplementedException ();
	}
	
	static class Sine
	{
		public static float EaseIn (double s)
		{
			return (float)Math.Sin (s * EasingMathHelper.HalfPi - EasingMathHelper.HalfPi) + 1;
		}
		
		public static float EaseOut (double s)
		{
			return (float)Math.Sin (s * EasingMathHelper.HalfPi);
		}
		
		public static float EaseInOut (double s)
		{
			return (float)(Math.Sin (s * EasingMathHelper.Pi - EasingMathHelper.HalfPi) + 1) / 2;
		}
	}
	
	static class Power
	{
		public static float EaseIn (double s, int power)
		{
			return (float)Math.Pow (s, power);
		}
		
		public static float EaseOut (double s, int power)
		{
			var sign = power % 2 == 0 ? -1 : 1;
			return (float)(sign * (Math.Pow (s - 1, power) + sign));
		}
		
		public static float EaseInOut (double s, int power)
		{
			if (s < 0.5)
				return EaseIn (s * 2, power) / 2;
			return (EaseOut ((s - 0.5) * 2, power) / 2) + 0.5f;
			
			//var sign = power % 2 == 0 ? -1 : 1;
			//return (float)(sign / 2.0 * (Math.Pow(s - 2, power) + sign * 2));
		}
	}
	
	/// <summary>
	/// Moves an object over a period of time
	/// </summary>
	/// <returns>
	/// A coroutine that moves the object
	/// </returns>
	/// <param name='objectToMove'>
	/// The Transform of the object to move.
	/// </param>
	/// <param name='position'>
	/// The destination position
	/// </param>
	/// <param name='time'>
	/// The number of seconds that the move should take
	/// </param>
	public static IEnumerator MoveObject (Transform objectToMove, Vector3 position, float time)
	{
		return MoveObject (objectToMove, position, time, EasingForm.InOut, EasingType.Quadratic);
	}
	
	public static IEnumerator MoveObject (Transform objectToMove, Vector3 position, float time, EasingType ease)
	{
		return MoveObject (objectToMove, position, time, EasingForm.InOut, ease);
	}
	
	/// <summary>
	/// Moves an object over a period of time
	/// </summary>
	/// <returns>
	/// A coroutine that moves the object
	/// </returns>
	/// <param name='objectToMove'>
	/// The Transform of the object to move.
	/// </param>
	/// <param name='position'>
	/// The destination position
	/// </param>
	/// <param name='time'>
	/// The number of seconds that the move should take
	/// </param>
	/// <param name='ease'>
	/// The easing function to use when moving the object
	/// </param>
	public static IEnumerator MoveObject (Transform objectToMove, Vector3 position, float time, EasingForm form, EasingType ease)
	{
		var t = 0f;
		var pos = objectToMove.localPosition;
		while (t < 1f) {
			
			if (form == EasingForm.InOut) {
				objectToMove.localPosition = Vector3.Lerp (pos, position, EaseInOut (t, ease));
			} else if (form == EasingForm.Out) {
				objectToMove.localPosition = Vector3.Lerp (pos, position, EaseOut (t, ease));
			} else {
				objectToMove.localPosition = Vector3.Lerp (pos, position, EaseIn (t, ease));
			}
			
			t += Time.deltaTime / time;
			yield return null;
		}
		objectToMove.localPosition = position;
	}


	
	public static IEnumerator ScaleGameObject (Transform targetObject, Vector3 scale, float time, EasingForm form, EasingType ease)
	{
		var t = 0f;
		var s = targetObject.localScale;
		
		while (t < 1f) {
			if (form == EasingForm.InOut) {
				targetObject.localScale = Vector3.Lerp (s, scale, EaseInOut (t, ease));
				
			} else if (form == EasingForm.Out) {
				targetObject.localScale = Vector3.Lerp (s, scale, EaseOut (t, ease));
			} else {
				targetObject.localScale = Vector3.Lerp (s, scale, EaseIn (t, ease));
			}
			
			t += Time.deltaTime / time;
			yield return null;
		}
		
		targetObject.localScale = scale;
	}
	
	public static IEnumerator ScaleObject (Transform targetObject, Vector3 scale, float time, EasingForm form, EasingType ease)
	{
		
		var t = 0f;
		var s = targetObject.localScale;
		
		while (t < 1f) {
			if (form == EasingForm.InOut) {
				targetObject.localScale = Vector3.Lerp (s, scale, EaseInOut (t, ease));
				
			} else if (form == EasingForm.Out) {
				targetObject.localScale = Vector3.Lerp (s, scale, EaseOut (t, ease));
			} else {
				targetObject.localScale = Vector3.Lerp (s, scale, EaseIn (t, ease));
			}
			
			t += Time.deltaTime / time;
			yield return null;
		}
		
		targetObject.localScale = scale;
	}
	
	public static IEnumerator RotateObject (Transform objectToMove, Quaternion q, float time, EasingForm form, EasingType ease)
	{
		
		var t = 0f;
		//float angleDone = 0f;
		Quaternion qOrg = objectToMove.rotation;
		
		while (t < 1f) {
			//float tAngle = 0.0f;
			//float tEaseLast = 0.0f;
			float tEaseNew = 0.0f;
			
			if (form == EasingForm.InOut) {
				tEaseNew = EaseInOut (t, ease);
				//float tEase = tEaseNew - tEaseLast;
				//tEaseLast = tEaseNew;
				//tAngle = angle * tEase;				
			} else if (form == EasingForm.Out) {
				tEaseNew = EaseOut (t, ease);
				//float tEase = tEaseNew - tEaseLast;
				//tEaseLast = tEaseNew;
				//tAngle = angle * tEase;	
			} else {
				tEaseNew = EaseIn (t, ease);
				//float tEase = tEaseNew - tEaseLast;
				//tEaseLast = tEaseNew;
				//tAngle = angle * tEase;	
			}
			
			//objectToMove.Rotate(axis, tAngle);
			
			objectToMove.rotation = Quaternion.Slerp (qOrg, q, tEaseNew);
			
			//angleDone += tAngle;
			
			t += Time.deltaTime / time;
			yield return null;
			
			//MPDebug.Log("Rotated: "+tEaseNew);
		}
		
		//MPDebug.Log("Rotated: "+angleDone);
		
		objectToMove.rotation = q;
		//objectToMove.Rotate(axis, angle - angleDone);
	}
	
	public static float PennerEaseIn (double linearStep, PennerEasingType type)
	{
		
		linearStep = Mathf.Clamp01 ((float)linearStep);
		switch (type) {
		case PennerEasingType.Linear:
			return (float)linearStep;
		case PennerEasingType.Quadratic:
			return (float)Penner.PennerInterpolator.QuadEaseIn (0.0, 1.0, linearStep);
		case PennerEasingType.Cubic:
			return (float)Penner.PennerInterpolator.CubicEaseIn (0.0, 1.0, linearStep);
		case PennerEasingType.Quartic:
			return (float)Penner.PennerInterpolator.QuarticEaseIn (0.0, 1.0, linearStep);
		case PennerEasingType.Quintic:
			return (float)Penner.PennerInterpolator.QuinticEaseIn (0.0, 1.0, linearStep);
		case PennerEasingType.Sine:
			return (float)Penner.PennerInterpolator.SineEaseIn (0.0, 1.0, linearStep);
		case PennerEasingType.Exponential:
			return (float)Penner.PennerInterpolator.ExpoEaseIn (0.0, 1.0, linearStep);
		case PennerEasingType.Circular:
			return (float)Penner.PennerInterpolator.CircularEaseIn (0.0, 1.0, linearStep);
		case PennerEasingType.Elastic:
			return (float)Penner.PennerInterpolator.ElasticEaseIn (0.0, 1.0, linearStep);
		case PennerEasingType.Bounce:
			return (float)Penner.PennerInterpolator.BounceEaseIn (0.0, 1.0, linearStep);
		case PennerEasingType.Back:
			return (float)Penner.PennerInterpolator.BackEaseIn (0.0, 1.0, linearStep);
		}
		throw new NotImplementedException ();
	}
	
	public static float PennerEaseOut (double linearStep, PennerEasingType type)
	{
		linearStep = Mathf.Clamp01 ((float)linearStep);
		switch (type) {
		case PennerEasingType.Linear:
			return (float)linearStep;
		case PennerEasingType.Quadratic:
			return (float)Penner.PennerInterpolator.QuadEaseOut (0.0, 1.0, linearStep);
		case PennerEasingType.Cubic:
			return (float)Penner.PennerInterpolator.CubicEaseOut (0.0, 1.0, linearStep);
		case PennerEasingType.Quartic:
			return (float)Penner.PennerInterpolator.QuarticEaseOut (0.0, 1.0, linearStep);
		case PennerEasingType.Quintic:
			return (float)Penner.PennerInterpolator.QuinticEaseOut (0.0, 1.0, linearStep);
		case PennerEasingType.Sine:
			return (float)Penner.PennerInterpolator.SineEaseOut (0.0, 1.0, linearStep);
		case PennerEasingType.Exponential:
			return (float)Penner.PennerInterpolator.ExpoEaseOut (0.0, 1.0, linearStep);
		case PennerEasingType.Circular:
			return (float)Penner.PennerInterpolator.CircularEaseOut (0.0, 1.0, linearStep);
		case PennerEasingType.Elastic:
			return (float)Penner.PennerInterpolator.ElasticEaseOut (0.0, 1.0, linearStep);
		case PennerEasingType.Bounce:
			return (float)Penner.PennerInterpolator.BounceEaseOut (0.0, 1.0, linearStep);
		case PennerEasingType.Back:
			return (float)Penner.PennerInterpolator.BackEaseOut (0.0, 1.0, linearStep);
		}
		throw new NotImplementedException ();
	}
	
	public static IEnumerator PennerMoveObject (Transform objectToMove, Vector3 position, float time, PennerEasingType ease, EasingForm form)
	{
		var t = 0f;
		var pos = objectToMove.localPosition;
		while (t < 1f) {
			float easeT;
			
			if (form == EasingForm.In) {
				easeT = PennerEaseIn (t, ease);
			} else {
				easeT = PennerEaseOut (t, ease);
			}
			
			objectToMove.localPosition = Vector3.Lerp (pos, position, easeT);
			t += Time.deltaTime / time;
			yield return null;
		}
		objectToMove.localPosition = position;
	}
	
	public static IEnumerator PennerScaleObject (Transform objectToScale, Vector3 scale, float time, PennerEasingType ease, EasingForm form)
	{
		var t = 0f;
		var newScale = objectToScale.localScale;
		
		while (t < 1f) {
			float easeT;
			
			if (form == EasingForm.In) {
				easeT = PennerEaseIn (t, ease);
			} else {
				easeT = PennerEaseOut (t, ease);
			}
			
			objectToScale.localScale = Vector3.Lerp (newScale, scale, easeT);
			t += Time.deltaTime / time;
			yield return null;
		}
		objectToScale.localScale = scale;
	}
	
	public static IEnumerator FadeObjectRecursive (Transform objectToFade, float alphaFrom, float alphaTo, float time, EasingType ease)
	{
		var t = 0f;
		SetAlphaOnAllChildren (objectToFade, alphaFrom);
		
		while (t < 1f) {
			SetAlphaOnAllChildren (objectToFade, Mathf.Lerp (alphaFrom, alphaTo, Easing.EaseInOut (t, ease))); 
			
			t += Time.deltaTime / time;
			yield return null;
		}
		
		SetAlphaOnAllChildren (objectToFade, alphaTo);
	}
	
	public static void SetAlphaOnAllChildren (Transform objectToFade, float alpha)
	{
		bool notTk2d = true;
		
		if (notTk2d) {
			MeshRenderer thisMesh = objectToFade.GetComponent<MeshRenderer> ();
			if (thisMesh != null) {
				foreach (Material mat in thisMesh.materials) {
					if (mat.HasProperty ("_Color")) {
						mat.color = new Color (mat.color.r, mat.color.g, mat.color.b, alpha);
					}
				}
			}
		}
		
		foreach (Transform child in objectToFade.transform) {
			
			bool notTk2dChild = true;
			
			if (notTk2dChild) {
				MeshRenderer mesh = child.GetComponent<MeshRenderer> ();
				if (mesh != null) {	
					foreach (Material mat in mesh.materials) {
						if (mat.HasProperty ("_Color")) {
							mat.color = new Color (mat.color.r, mat.color.g, mat.color.b, alpha);
						}
					}
				}
			}
			
			SetAlphaOnAllChildren (child, alpha);
		}
	}
	
	public static IEnumerator ColourObjectRecursive (Transform objectToColour, Color colourFrom, Color colourTo, float time, EasingType ease)
	{
		var t = 0f;
		SetColourOnAllChildren (objectToColour, colourFrom);
		
		while (t < 1f) {
			
			float r =  Mathf.Lerp (colourFrom.r, colourTo.r, Easing.EaseInOut (t, ease));
			float g =  Mathf.Lerp (colourFrom.g, colourTo.g, Easing.EaseInOut (t, ease));
			float b =  Mathf.Lerp (colourFrom.b, colourTo.b, Easing.EaseInOut (t, ease));
			float a =  Mathf.Lerp (colourFrom.a, colourTo.a, Easing.EaseInOut (t, ease));
			
			Color colour = new Color(r, g, b, a);
			
			SetColourOnAllChildren (objectToColour, colour); 
			
			t += Time.deltaTime / time;
			yield return null;
		}
		
		SetColourOnAllChildren (objectToColour, colourTo);
	}
	
	public static void SetColourOnAllChildren (Transform objectToColour, Color colour)
	{
		bool notTk2d = true;
		
		if (notTk2d) {
			MeshRenderer thisMesh = objectToColour.GetComponent<MeshRenderer> ();
			if (thisMesh != null) {
				foreach (Material mat in thisMesh.materials) {
					if (mat.HasProperty ("_Color")) {
						mat.color = new Color (colour.r, colour.g, colour.b, colour.a);
					}
				}
			}
		}
		
		foreach (Transform child in objectToColour.transform) {
			
			bool notTk2dChild = true;
			
			if (notTk2dChild) {
				MeshRenderer mesh = child.GetComponent<MeshRenderer> ();
				if (mesh != null) {	
					foreach (Material mat in mesh.materials) {
						if (mat.HasProperty ("_Color")) {
							mat.color = new Color (colour.r, colour.g, colour.b, colour.a);
						}
					}
				}
			}
			
			SetColourOnAllChildren (child, colour);
		}
	}
	
	public static IEnumerator FadeObject (MonoBehaviour source, Transform objectToFade, float alpha, float time, EasingType ease)
	{
		bool notTk2d = true;
		
		if (notTk2d) {
			MeshRenderer thisMesh = objectToFade.GetComponent<MeshRenderer> ();
			if (thisMesh != null) {
				foreach (Material mat in thisMesh.materials) {
					if (mat.HasProperty ("_Color")) {
						source.StartCoroutine (FadeMaterial (mat, alpha, time, ease));
					}	
				}
			}
		}
		
		foreach (Transform child in objectToFade.transform) {
			source.StartCoroutine (FadeObject (source, child, alpha, time, ease));
		}
		
		yield return new WaitForSeconds(time);
	}
	
	public static IEnumerator FadeMaterial (Material materialToFade, float alphaTo, float time, EasingType ease)
	{
		var t = 0f;
		
		if (!materialToFade.HasProperty ("_Color")) {
			yield break;
		}
		
		float startingAlpha = materialToFade.color.a;
		
		while (t < 1f) {
			Color newColour = materialToFade.color;
			newColour.a = Mathf.Lerp (startingAlpha, alphaTo, Easing.EaseInOut (t, ease));
			materialToFade.color = newColour;
			
			t += Time.deltaTime / time;
			yield return null;
		}
		
		Color finalColour = materialToFade.color;
		finalColour.a = alphaTo;
		materialToFade.color = finalColour;
	}
}

public enum EasingForm
{
	In,
	Out, 
	InOut,
}

public enum EasingType
{
	Step,
	Linear,
	Sine,
	Quadratic,
	Cubic,
	Quartic,
	Quintic
}

public enum PennerEasingType
{
	Linear,
	Quadratic,
	Cubic,
	Quartic,
	Quintic,
	Sine,
	Exponential,
	Circular,
	Elastic,
	Bounce,
	Back
}

public static class EasingMathHelper
{
	public const float Pi = (float)Math.PI;
	public const float HalfPi = (float)(Math.PI / 2);
	
	public static float Lerp (double from, double to, double step)
	{
		return (float)((to - from) * step + from);
	}
}


