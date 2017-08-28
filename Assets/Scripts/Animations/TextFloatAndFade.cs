using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFloatAndFade : MonoBehaviour {

	[SerializeField]
	private float duration = 0.25f;

	[SerializeField]
	private float floatDistance = 50;

	[SerializeField]
	private Text text;

	private float timer = 0.0f;

	private float initialY;


	void Start() {

		timer = 0.0f;

		initialY = transform.localPosition.y;

		StartCoroutine(FloatAndFadeCoroutine());
	}

	private IEnumerator FloatAndFadeCoroutine() {

		while(timer < duration) {
			timer = Mathf.Min(timer += Time.deltaTime, duration);

			DeltaFloatAndFade();

			yield return new WaitForEndOfFrame();
		}

		Destroy(gameObject);
	}

	private void DeltaFloatAndFade() {
		float ease = Easing.EaseIn(Mathf.Clamp01(timer / duration), EasingType.Quadratic);

		Vector3 position = transform.localPosition;
		position.y = initialY + Mathf.Lerp(0, floatDistance, ease);
		transform.localPosition = position;

		float alpha = Mathf.Lerp(1, 0, ease);

		text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
	}
}
