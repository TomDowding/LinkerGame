using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimator : MonoBehaviour {

	private float moveTimer = 0.0f;

	private Vector2 fromPos;
	private Vector2 toPos;
	private float duration;


	public void Move(Vector2 fromPos, Vector2 toPos, float duration) {
		this.fromPos = fromPos;
		this.toPos = toPos;
		this.moveTimer = 0.0f;
		this.duration = duration;

		StartCoroutine(MoveCoroutine());
	}

	private IEnumerator MoveCoroutine() {

		while(moveTimer < duration) {
			moveTimer = Mathf.Min(moveTimer += Time.deltaTime, duration);

			DeltaMove();

			yield return new WaitForEndOfFrame();
		}
	}

	private void DeltaMove() {
		float ease = Easing.EaseInOut(Mathf.Clamp01(moveTimer / duration), EasingType.Quadratic);
		Vector3 scale = transform.localPosition;
		scale.x = Mathf.Lerp(fromPos.x, toPos.x, ease);
		scale.y = Mathf.Lerp(fromPos.y, toPos.y, ease);
		transform.localPosition = scale;
	}
}
