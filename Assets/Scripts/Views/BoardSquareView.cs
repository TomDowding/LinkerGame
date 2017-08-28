using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareView : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public void SetSpriteForBoardSquare(BoardSquare boardSquare) {
		
		spriteRenderer.sprite = SpriteServer.Instance.BoardSquareSpriteForType(boardSquare.boardSquareType);

		if(boardSquare.boardSquareType == BoardSquareType.Normal) {
			spriteRenderer.sortingLayerName = "Board";
			spriteRenderer.sortingOrder = 0;
		}
		else {
			spriteRenderer.sortingLayerName = "Blockers";
			spriteRenderer.sortingOrder = 0;
		}
	}

	public BoardSquareSize GetSpriteSize() {
		BoardSquareSize boardSquareSize = new BoardSquareSize();
		boardSquareSize.width = spriteRenderer.bounds.size.x;
		boardSquareSize.height = spriteRenderer.bounds.size.y;
		return boardSquareSize;
	}

}
