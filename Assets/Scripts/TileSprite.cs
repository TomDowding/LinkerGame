using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSprite : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private float shrinkDuration = 0.25f;

	[SerializeField]
	private float shrinkScale = 0.8f;

	private float shrinkTimer = 0.0f;

	[SerializeField]
	private float disappearDuration = 0.25f;

	private float disappearTimer = 0.0f;


	public void Reset(int tileType) {
		transform.localScale = Vector3.one;
		ShowNotInChain(tileType);
	}

	#region Hover
	public void ShowHover() {

		shrinkTimer = 0.0f;
		StartCoroutine(ShowHoverCoroutine());
	}

	private IEnumerator ShowHoverCoroutine() {

		while(shrinkTimer < shrinkDuration) {
			shrinkTimer = Mathf.Min(shrinkTimer += Time.deltaTime, shrinkDuration);

			DeltaShrink();

			yield return new WaitForEndOfFrame();
		}
	}

	private void DeltaShrink() {
		float ease = Easing.EaseInOut(Mathf.Clamp01(shrinkTimer / shrinkDuration), EasingType.Quadratic);
		Vector3 scale = transform.localScale;
		scale.x = Mathf.Lerp(1, shrinkScale, ease);
		scale.y = Mathf.Lerp(1, shrinkScale, ease);
		transform.localScale = scale;
	}
	#endregion


	#region No hover
	public void ShowNoHover() {

		shrinkTimer = 0.0f;
		StartCoroutine(ShowNoHoverCoroutine());
	}

	private IEnumerator ShowNoHoverCoroutine() {

		while(shrinkTimer < shrinkDuration) {
			shrinkTimer = Mathf.Min(shrinkTimer += Time.deltaTime, shrinkDuration);

			DeltaGrow();

			yield return new WaitForEndOfFrame();
		}
	}

	private void DeltaGrow() {
		float ease = Easing.EaseInOut(Mathf.Clamp01(shrinkTimer / shrinkDuration), EasingType.Quadratic);
		Vector3 scale = transform.localScale;
		scale.x = Mathf.Lerp(shrinkScale, 1, ease);
		scale.y = Mathf.Lerp(shrinkScale, 1, ease);
		transform.localScale = scale;
	}
	#endregion


	#region In chain
	public void ShowInChain(int tileType) {
		spriteRenderer.sprite = SpriteManager.Instance.OutlineSpriteForTileType(tileType);
	}

	public void ShowNotInChain(int tileType) {
		spriteRenderer.sprite = SpriteManager.Instance.NormalSpriteForTileType(tileType);
	}
	#endregion


	#region Disappearing
	public void ShowDisappear() {

		disappearTimer = 0.0f;
		StartCoroutine(DisappearCoroutine());
	}

	private IEnumerator DisappearCoroutine() {

		while(disappearTimer < disappearDuration) {
			disappearTimer = Mathf.Min(disappearTimer += Time.deltaTime, disappearDuration);

			DeltaDisappear();

			yield return new WaitForEndOfFrame();
		}
	}

	private void DeltaDisappear() {
		float ease = Easing.EaseInOut(Mathf.Clamp01(disappearTimer / disappearDuration), EasingType.Quadratic);
		Vector3 scale = transform.localScale;
		scale.x = Mathf.Lerp(1, 0, ease);
		scale.y = Mathf.Lerp(1, 0, ease);
		transform.localScale = scale;
	}
	#endregion
}
