using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare : MonoBehaviour {

	[SerializeField]
	private BoardSquareSprite boardSquareSprite;


	public Vector2 GetSize() {
		return boardSquareSprite.GetSize();
	}
}
