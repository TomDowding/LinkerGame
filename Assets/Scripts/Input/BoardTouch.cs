using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTouch : MonoBehaviour {

	[SerializeField]
	private Board board;

	[SerializeField]
	private BoxCollider boxCollider;

	private Tile selectedTile = null;


	void Update () {
		
		// Handle native touch events
		foreach (Touch touch in Input.touches) {
			HandleTouch(touch.fingerId, (touch.position), touch.phase);
		}

		// Simulate touch events from mouse events
		if (Input.touchCount == 0) {
			if (Input.GetMouseButtonDown(0) ) {
				HandleTouch(10, (Input.mousePosition), TouchPhase.Began);
			}
			if (Input.GetMouseButton(0) ) {
				HandleTouch(10, (Input.mousePosition), TouchPhase.Moved);
			}
			if (Input.GetMouseButtonUp(0) ) {
				HandleTouch(10, (Input.mousePosition), TouchPhase.Ended);
			}
		}
	}

	public void ResizeColliderForBoard(BoardSize boardSize, BoardSquareSize boardSquareSize) {

		boxCollider.size = new Vector2(boardSize.numCols * boardSquareSize.width, boardSize.numRows * boardSquareSize.height);
	}

	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase) {

		Ray ray = Camera.main.ScreenPointToRay(touchPosition);
		RaycastHit hit;

		switch (touchPhase) {
		case TouchPhase.Began:
			
			if(boxCollider.Raycast(ray, out hit, 1000.0f)) {
				TrySelectTileWithHit(hit);
			}

			break;
		case TouchPhase.Moved:

			if(boxCollider.Raycast(ray, out hit, 1000.0f)) {
				TrySelectTileWithHit(hit);
			}

			break;

		case TouchPhase.Ended:

			if(selectedTile != null) {
				selectedTile.Deselect();
				selectedTile.LetGo();

				selectedTile = null;
			}

			break;
		}
	}
		
	private void TrySelectTileWithHit(RaycastHit hit) {

		Vector3 localPointInCollider = transform.InverseTransformPoint(hit.point); 

		float x = (localPointInCollider.x + boxCollider.size.x * 0.5f) / boxCollider.size.x;
		float y = (localPointInCollider.y + boxCollider.size.y * 0.5f) / boxCollider.size.y;
		Vector2 normalisedPoint = new Vector2(x, y);

		Tile hitTile = board.TileForTouchPosition(normalisedPoint);
		if(hitTile != null) {

			if(selectedTile != null) {

				if(selectedTile != hitTile) {
					selectedTile.Deselect();
					hitTile.Select();
					selectedTile = hitTile;
				}
			}
			else {
				hitTile.Select();
				selectedTile = hitTile;
			}
		}
	}
}
