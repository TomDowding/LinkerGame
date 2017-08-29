using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour {

	[SerializeField]
	private Transform spriteTransform;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private DropEaser dropEaser;

	[SerializeField]
	private ScaleEaser scaleEaser;

	[SerializeField]
	private SpriteFadeEaser spriteFadeAnimator;

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

		scaleEaser.StartAnimation(spriteTransform, Vector2.one, new Vector2(shrinkScale, shrinkScale), shrinkDuration);
	}
		
	public void ShowNoHover() {

		scaleEaser.StartAnimation(spriteTransform, spriteTransform.localScale, Vector2.one, shrinkDuration);
	}
		
	public void ShowDrop(Vector2 toPosition) {

		dropEaser.StartAnimation(transform, transform.localPosition, toPosition, dropDuration);
		StartCoroutine(DropSFX());
	}

	public IEnumerator DropSFX() {
		yield return new WaitForSeconds(dropDuration);
		AudioController.Instance.DropTile();
	}

	public void ShowInChain(int tileType) {
		
		spriteRenderer.sprite = SpriteServer.Instance.OutlineSpriteForTileType(tileType);
	}

	public void ShowNotInChain(int tileType) {
		
		spriteRenderer.sprite = SpriteServer.Instance.NormalSpriteForTileType(tileType);
	}

	public void ShowDisappear() {

		scaleEaser.StartAnimation(spriteTransform, Vector2.one, Vector3.zero, disappearDuration);
	}

	public void ShowInvisible() {

		spriteRenderer.color = new Color(1, 1, 1, 0);
	}

	public void ShowVisible() {
		
		Vector4 fromColVec =  new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a);
		Vector4 toColVec =  new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
		spriteFadeAnimator.StartAnimation(null, fromColVec, toColVec, dropDuration * 0.5f);
	}
}
