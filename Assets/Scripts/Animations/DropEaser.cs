using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEaser : Easer {

	protected override void Delta() {
	
		float ease = Easing.EaseIn(Mathf.Clamp01(timer / duration), EasingType.Quadratic);

		Vector3 newVec = t.localPosition;
		newVec.x = Mathf.Lerp(fromVec.x, toVec.x, ease);
		newVec.y = Mathf.Lerp(fromVec.y, toVec.y, ease);
		t.localPosition = newVec;
	}
}
