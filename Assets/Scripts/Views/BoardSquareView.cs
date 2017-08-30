using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareView : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public void SetSpriteForBoardSquare(BoardSquareType boardSquareType) {

		spriteRenderer.sprite = SpriteServer.Instance.BoardSquareSpriteForType(boardSquareType);

		if(boardSquareType == BoardSquareType.Blocker) {
			spriteRenderer.sortingLayerName = "Blockers";
			spriteRenderer.color = Color.white;
		}
		else if(boardSquareType == BoardSquareType.Edge) {
			spriteRenderer.sortingLayerName = "Board";
			spriteRenderer.color = Color.clear;
		}
		else {
			spriteRenderer.sortingLayerName = "Board";
			spriteRenderer.color = Color.white;
		}
	}


	public BoardSquareSize GetSpriteSize() {
		BoardSquareSize boardSquareSize = new BoardSquareSize();
		boardSquareSize.width = spriteRenderer.bounds.size.x;
		boardSquareSize.height = spriteRenderer.bounds.size.y;
		return boardSquareSize;
	}

}
