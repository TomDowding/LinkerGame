using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteServer: Singleton<SpriteServer> {

	[SerializeField] 
	private Sprite[] tileSprites;

	[SerializeField] 
	private Sprite[] tileOutlineSprites;

	[SerializeField] 
	private Sprite[] boardSquareSprites;

	public int NumTileTypes() {
		return tileSprites.Length;
	}

	public Sprite NormalSpriteForTileType(int type) {
		if(type >= tileSprites.Length) {
			Debug.LogWarning("Trying to get normal sprite index " + type + " of " + tileSprites.Length);
			return null;
		}
		return tileSprites[type];
	}

	public Sprite OutlineSpriteForTileType(int type) {
		if(type >= tileOutlineSprites.Length) {
			Debug.LogWarning("Trying to get outline sprite index " + type + " of " + tileOutlineSprites.Length);
			return null;
		}
		return tileOutlineSprites[type];
	}

	public Sprite BoardSquareSpriteForType(BoardSquareType boardSquareType) {

		return boardSquareSprites[15];

		/*
		if(boardSquareType == BoardSquareType.Normal) {
			return boardSquareSprites[1];
		}
		else {
			return boardSquareSprites[0];
		}
		//*/
	}

	public Sprite BoardSquareSpriteEdge(int edgeValue) {
		return boardSquareSprites[edgeValue];
	}
}
