using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareView : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public void SetSpriteForBoardSquare(BoardSquare boardSquare) {

		spriteRenderer.sprite = SpriteServer.Instance.BoardSquareSpriteForType(boardSquare.boardSquareType);

		if(boardSquare.boardSquareType == BoardSquareType.Blocker) {
			spriteRenderer.color = new Color(0, 0, 0, 0);
		}
		else {
			spriteRenderer.color = new Color(1, 1, 1, 1);
		}

		/*
		if(boardSquare.boardSquareType == BoardSquareType.Normal) {
			spriteRenderer.sortingLayerName = "Board";
			spriteRenderer.sortingOrder = 0;
		}
		else {
			spriteRenderer.sortingLayerName = "Blockers";
			spriteRenderer.sortingOrder = 0;
		}
		//*/
	}

	public void SetSpriteForEdge(int edgeValue) {

		/*
		spriteRenderer.sprite = SpriteServer.Instance.BoardSquareSpriteEdge(edgeValue);
		spriteRenderer.sortingLayerName = "Board";
		spriteRenderer.sortingOrder = 0;

		if(edgeValue == 0 || edgeValue == 6 || edgeValue == 9) {
			spriteRenderer.color = new Color(0, 0, 0, 0);
		}
		//*/
	}

	public BoardSquareSize GetSpriteSize() {
		BoardSquareSize boardSquareSize = new BoardSquareSize();
		boardSquareSize.width = spriteRenderer.bounds.size.x;
		boardSquareSize.height = spriteRenderer.bounds.size.y;
		return boardSquareSize;
	}

}
