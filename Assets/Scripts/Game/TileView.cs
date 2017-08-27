using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour {

	[SerializeField]
	private Transform spriteTransform;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private DropAnimator dropAnimator;

	[SerializeField]
	private GrowShrink growShrinkAnimator;

	[SerializeField]
	private float shrinkDuration = 0.25f;

	[SerializeField]
	private float shrinkScale = 0.8f;

	[SerializeField]
	private float disappearDuration = 0.25f;

	[SerializeField]
	private float dropDuration = 0.4f;


	public void Reset(int tileType) {
		transform.localScale = Vector3.one;
		ShowNotInChain(tileType);
	}


	public void ShowHover() {

		growShrinkAnimator.StartAnimation(spriteTransform, Vector2.one, new Vector2(shrinkScale, shrinkScale), shrinkDuration);
	}
		
	public void ShowNoHover() {

		growShrinkAnimator.StartAnimation(spriteTransform, spriteTransform.localScale, Vector2.one, shrinkDuration);
	}
		
	public void ShowDrop(Vector2 toPosition) {

		dropAnimator.StartAnimation(transform, transform.localPosition, toPosition, dropDuration);
	}

	public void ShowInChain(int tileType) {
		spriteRenderer.sprite = SpriteManager.Instance.OutlineSpriteForTileType(tileType);
	}

	public void ShowNotInChain(int tileType) {
		spriteRenderer.sprite = SpriteManager.Instance.NormalSpriteForTileType(tileType);
	}

	public void ShowDisappear() {

		growShrinkAnimator.StartAnimation(spriteTransform, Vector2.one, Vector3.zero, disappearDuration);
	}


}
