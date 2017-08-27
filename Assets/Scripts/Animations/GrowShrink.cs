using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShrink : EaseAnimator {

	protected override void Delta() {
		float ease = Easing.EaseInOut(Mathf.Clamp01(timer / duration), EasingType.Quadratic);
		Vector3 newVec = t.localScale;
		newVec.x = Mathf.Lerp(fromVec.x, toVec.x, ease);
		newVec.y = Mathf.Lerp(fromVec.y, toVec.y, ease);
		t.localScale = newVec;
	}
}
