using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareSprite : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public void SetSprite(Sprite sprite) {
		spriteRenderer.sprite = sprite;
	}

	public Vector2 GetSize() {
		return spriteRenderer.bounds.size;
	}

}
