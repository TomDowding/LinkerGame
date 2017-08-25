using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSprite : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private float shrinkDuration = 0.25f;

	[SerializeField]
	private float srhinkScale = 0.8f;

	private float shrinkTimer = 0.0f;


	public void Reset(int tileType) {
		transform.localScale = Vector3.one;
		ShowNotInLink(tileType);
	}

	#region Hover state
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
		scale.x = Mathf.Lerp(1, srhinkScale, ease);
		scale.y = Mathf.Lerp(1, srhinkScale, ease);
		transform.localScale = scale;
	}
	#endregion


	#region No hover state
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
		scale.x = Mathf.Lerp(srhinkScale, 1, ease);
		scale.y = Mathf.Lerp(srhinkScale, 1, ease);
		transform.localScale = scale;
	}
	#endregion


	#region Link state
	public void ShowInLink(int tileType) {
		spriteRenderer.sprite = SpriteManager.Instance.OutlineSpriteForTileType(tileType);
	}

	public void ShowNotInLink(int tileType) {

		spriteRenderer.sprite = SpriteManager.Instance.NormalSpriteForTileType(tileType);
	}
	#endregion
}
