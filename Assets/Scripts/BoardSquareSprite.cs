using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareSprite : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public void SetSprite(Sprite sprite) {
		spriteRenderer.sprite = sprite;
	}

	public Vector2 GetTileSize() {
		return spriteRenderer.bounds.size;
	}

}
