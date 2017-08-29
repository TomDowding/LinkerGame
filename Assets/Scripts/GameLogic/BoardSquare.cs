﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardSquareType {
	Blocker = 0,
	Normal = 1,
}

public class BoardSquare : MonoBehaviour {

	[SerializeField]
	private BoardSquareView boardSquareView;

	public BoardSquareType boardSquareType {get; private set;}


	public void Setup(BoardCoord boardCoord, BoardSquareType boardSquareType) {

		this.boardSquareType = boardSquareType;

		gameObject.name = boardSquareType.ToString() + "_" + boardCoord.col + "_" + boardCoord.row;

		boardSquareView.SetSpriteForBoardSquare(this);
	}

	public void SetupEdge(int edgeValue) {

		boardSquareView.SetSpriteForEdge(edgeValue);
	}

	public BoardSquareSize GetSize() {
		
		return boardSquareView.GetSpriteSize();
	}
}
