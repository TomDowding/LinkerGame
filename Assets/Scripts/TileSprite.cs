using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSprite : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public void SetSprite(Sprite sprite) {
		spriteRenderer.sprite = sprite;
	}

	public void ShowHover() {

		this.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
	}

	public void ShowNormal() {

		this.transform.localScale = Vector3.one;
	}
}
