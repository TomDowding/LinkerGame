using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	[SerializeField]
	private TileSprite tileSprite;

	public int tileType {get; private set;}

	public BoardCoord boardCoord {get; private set;}

	private bool inChain;

	private bool hovering;

	void Start () {
			
	}
		
	public void SetupTile(int tileType, BoardCoord boardCoord) {

		this.boardCoord = boardCoord;
		this.tileType = tileType;

		Reset();
	}

	public void Reset() {
		
		inChain = false;
		hovering = false;
		tileSprite.Reset(tileType);
	}
		
	public void AddToChain(Tile linkedToTile) {
		Debug.Log("Adding tile to chain at " + boardCoord.Description());

		inChain = true;
		tileSprite.ShowInChain(tileType);
	}

	public void RemoveFromChain() {
		Debug.Log("Removing tile from chain at " + boardCoord.Description());

		inChain = false;
		tileSprite.Reset(tileType);
	}

	public void Select() {
		Debug.Log("Select tile at " + boardCoord.Description() + ", inChain: " + inChain);

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
		Debug.Log("Deselect tile at " + boardCoord.Description() + ", inChain: " + inChain);

		if(hovering) {
			hovering = false;
			tileSprite.ShowNoHover();
		}
	}

	public void RemoveFromBoard() {
		tileSprite.ShowDisappear();
	}
}
