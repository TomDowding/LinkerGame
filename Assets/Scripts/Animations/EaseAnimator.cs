using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaseAnimator : MonoBehaviour {

	protected float timer = 0.0f;
	protected float duration;
	protected Vector3 fromVec;
	protected Vector3 toVec;
	protected Transform t;

	public void StartAnimation(Transform t, Vector3 fromVec, Vector3 toVec, float duration) {
		this.t = t;
		this.fromVec = fromVec;
		this.toVec = toVec;
		this.duration = duration;

		this.timer = 0.0f;

		StartCoroutine(AnimationCoroutine());
	}

	protected IEnumerator AnimationCoroutine() {

		while(timer < duration) {
			timer = Mathf.Min(timer += Time.deltaTime, duration);

			Delta();

			yield return new WaitForEndOfFrame();
		}
	}

	protected virtual void Delta() {
		
	}
}
