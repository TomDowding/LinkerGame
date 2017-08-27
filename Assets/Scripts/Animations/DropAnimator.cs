using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAnimator : EaseAnimator {

	protected override void Delta() {
		float ease = Easing.EaseIn(Mathf.Clamp01(timer / duration), EasingType.Quadratic);
		Vector3 newVec = t.localPosition;
		newVec.y = Mathf.Lerp(fromVec.y, toVec.y, ease);
		t.localPosition = newVec;
	}
}
