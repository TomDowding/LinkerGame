using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	[SerializeField]
	private TileView tileView;

	public int tileType {get; private set;}

	public BoardCoord boardCoord {get; private set;}

	private bool inChain;

	private bool hovering;

	private GameManager gameManager;

	void Start () {
			
	}
		
	public void Setup(int tileType, BoardCoord boardCoord, GameManager gameManager) {

		this.gameManager = gameManager;
		this.boardCoord = boardCoord;
		this.tileType = tileType;

		gameObject.name = "Tile_" + boardCoord.col + "_" + boardCoord.row;

		Reset();
	}

	public void Reset() {
		
		inChain = false;
		hovering = false;
		tileView.Reset(tileType);
	}
		
	public void MoveCoord(BoardCoord boardCoord) {
		
		this.boardCoord = boardCoord;
		Reset();
	}
		
	public void AddToChain(Tile linkedToTile) {
		//Debug.Log("Adding tile to chain at " + boardCoord.description);
	
		inChain = true;
		tileView.ShowInChain(tileType);
	}

	public void RemoveFromChain() {
		//Debug.Log("Removing tile from chain at " + boardCoord.description);

		inChain = false;
		tileView.Reset(tileType);
	}

	public void RemoveFromBoard(int chainIndex) {
		//Debug.Log("Removing tile from board at " + boardCoord.description);

		tileView.ShowDisappear(chainIndex);
	}

	public void Drop(Vector2 toPosition) {
		//Debug.Log("Dropping tile at " + boardCoord.description + " to " + toPosition);

		tileView.ShowVisible();

		tileView.ShowDrop(toPosition);
	}
		
	public void PrepareForNewDrop() {
		tileView.ShowInvisible();
	}
		

	#region Called via touch input
	public void Select() {
		
		if(!gameManager.interactionEnabled) {
			return;
		}

		//Debug.Log("Select tile at " + boardCoord.description + ", inChain: " + inChain);

		if(inChain) {
			// If the tile is already in a chain, we are going back into it.
			// See if we should remove the tiles after it (undo)
			gameManager.TryRemoveTilesAfterTile(this);
		}
		else {
			// If not in a chain, see if we can add it.
			// Hover state on a successful add only.
			bool success = gameManager.TryAddTileToChain(this);
			if(success) {
				hovering = true;
				tileView.ShowHover();
			}
		}
	}

	public void Deselect() {
		
		if(!gameManager.interactionEnabled) {
			return;
		}

		//Debug.Log("Deselect tile at " + boardCoord.description + ", inChain: " + inChain);

		if(hovering) {
			hovering = false;
			tileView.ShowNoHover();
		}
	}

	public void LetGo() {
		if(!gameManager.interactionEnabled) {
			return;
		}

		gameManager.TryCompleteChain();
	}
	#endregion
}
