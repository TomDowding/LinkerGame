using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	private ArrayList chain;

	public bool interactionEnabled {get; private set;}


	void Start () {

		Level level = new Level();
		StartLevel(level);
	}
		
	private void StartLevel(Level level) {
		chain = new ArrayList();
		Board.Instance.SetupForLevel(level);
		interactionEnabled = true;
	}

	public void EmptyChain() {
		chain.Clear();
	}

	public bool TryAddTileToChain(Tile tile) {
		//Debug.Log("Try to add tile of type " + tile.tileType + " at coord " + tile.boardCoord.Description() + " to chain, " + chain.Count + " in chain");

		// Can add if chain is empty
		if(chain.Count == 0) {
			AddTileToChain(tile, null);
			return true;
		}

		// Can add if tile matches last tile in chain and they are neighbours
		Tile lastTile = (Tile) chain[chain.Count - 1];
	
		if(lastTile.tileType == tile.tileType) {
			float distance = Vector2.Distance(new Vector2(lastTile.boardCoord.x, lastTile.boardCoord.y), new Vector2(tile.boardCoord.x, tile.boardCoord.y));
			if(distance < 1.5f) { 
				AddTileToChain(tile, lastTile);
				return true;
			}
		}
			
		// Otherwise, can't add
		return false;
	}

	private void AddTileToChain(Tile tile, Tile linkedToTile) {
		chain.Add(tile);
		tile.AddToChain(linkedToTile);
	}

	public void TryRemoveTilesAfterTile(Tile tile) {

		// Try and find the index of the tile in the chain
		int chainIndex = -1;
		for(int i = 0; chainIndex < chain.Count; i++) {
			Tile chainTile = (Tile) chain[i];
		
			if(chainTile == tile) {
				Debug.Log("Found tile in chain at index " + i);
				chainIndex = i;
				break;
			}
		}

		// If we didn't find tile in chain, don't need to remove anything
		if(chainIndex < 0) {
			return;
		}

		// Otherwise, remove all tiles after the index in chain
		for(int i = chain.Count - 1; i > chainIndex; i--) {
			Tile chainTile = (Tile) chain[i];
			chainTile.RemoveFromChain();
			chain.Remove(chainTile);
		}
	}


	public void FinishedChaining() {

		Debug.Log("Finished chaining!");
		foreach(Tile tile in chain) {

			tile.RemoveFromChain();
		}

		EmptyChain();
	}
}
