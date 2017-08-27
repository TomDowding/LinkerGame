using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare : MonoBehaviour {

	[SerializeField]
	private BoardSquareView boardSquareView;


	public void Setup(BoardCoord boardCoord) {

		gameObject.name = "BoardSquare_" + boardCoord.col + "_" + boardCoord.row;
	}

	public BoardSquareSize GetSize() {
		
		return boardSquareView.GetSpriteSize();
	}
}
