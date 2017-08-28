using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteFadeEaser : Easer {

	[SerializeField]
	private SpriteRenderer spriteRenderer;


	protected override void Delta() {
		
		float ease = Easing.EaseIn(Mathf.Clamp01(timer / duration), EasingType.Quadratic);

		Color col = spriteRenderer.color;
		col.a = Mathf.Lerp(fromVec.w, toVec.w, ease);
		spriteRenderer.color = col;
	}
}
