using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareSprite : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public void SetSprite(Sprite sprite) {
		spriteRenderer.sprite = sprite;
	}

	public BoardSquareSize GetSize() {
		BoardSquareSize boardSquareSize = new BoardSquareSize();
		boardSquareSize.width = spriteRenderer.bounds.size.x;
		boardSquareSize.height = spriteRenderer.bounds.size.y;
		return boardSquareSize;
	}

}
