using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformMoveEaser : Easer {

	public RectTransform rectTransform;

	protected override void Delta() {

		float ease = Easing.EaseOut(Mathf.Clamp01(timer / duration), EasingType.Quadratic);

		Vector3 newVec = rectTransform.anchoredPosition;
		newVec.x = Mathf.Lerp(fromVec.x, toVec.x, ease);
		newVec.y = Mathf.Lerp(fromVec.y, toVec.y, ease);
		rectTransform.anchoredPosition = newVec;
	}
}
