using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	[SerializeField]
	private TileSprite tileSprite;

	[SerializeField]
	private MoveAnimator moveAnimator;

	public int tileType {get; private set;}

	public BoardCoord boardCoord;

	private bool inChain;

	private bool hovering;

	void Start () {
			
	}
		
	public void Setup(int tileType, BoardCoord boardCoord) {

		this.boardCoord = boardCoord;
		this.tileType = tileType;

		gameObject.name = "Tile_" + boardCoord.col + "_" + boardCoord.row;

		Reset();
	}

	public void Reset() {
		
		inChain = false;
		hovering = false;
		tileSprite.Reset(tileType);
	}
		
	public void AddToChain(Tile linkedToTile) {
		Debug.Log("Adding tile to chain at " + boardCoord.description);

		inChain = true;
		tileSprite.ShowInChain(tileType);
	}

	public void RemoveFromChain() {
		Debug.Log("Removing tile from chain at " + boardCoord.description);

		inChain = false;
		tileSprite.Reset(tileType);
	}

	public void RemoveFromBoard() {
		Debug.Log("Removing tile from board at " + boardCoord.description);

		tileSprite.ShowDisappear();
	}

	public void Drop(Vector2 toPosition) {
		Debug.Log("Dropping tile at " + boardCoord.description + " to " + toPosition);

		moveAnimator.Move(transform.localPosition, toPosition, 1.0f);
	}



	#region Called via touch input
	public void Select() {
		
		if(!GameManager.Instance.interactionEnabled) {
			return;
		}

		Debug.Log("Select tile at " + boardCoord.description + ", inChain: " + inChain);

		if(inChain) {
			// If the tile is already in a chain, we are going back into it.
			// See if we should remove the tiles after it (undo)
			GameManager.Instance.TryRemoveTilesAfterTile(this);
		}
		else {
			// If not in a chain, see if we can add it.
			// Hover state on a successful add only.
			bool success = GameManager.Instance.TryAddTileToChain(this);
			if(success) {
				hovering = true;
				tileSprite.ShowHover();
			}
		}
	}

	public void Deselect() {
		
		if(!GameManager.Instance.interactionEnabled) {
			return;
		}

		Debug.Log("Deselect tile at " + boardCoord.description + ", inChain: " + inChain);

		if(hovering) {
			hovering = false;
			tileSprite.ShowNoHover();
		}
	}

	public void LetGo() {
		if(!GameManager.Instance.interactionEnabled) {
			return;
		}

		GameManager.Instance.TryCompleteChain();
	}
	#endregion
}
