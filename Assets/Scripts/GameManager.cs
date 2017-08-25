using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	[SerializeField]
	private GameObject dummyLinkObject;

	private ArrayList chain;

	private ArrayList links;

	[SerializeField]
	private float tileDisappearDelay = 0.4f;

	private const int minChainLength = 3;

	public bool interactionEnabled {get; private set;}


	void Start () {
		
		chain = new ArrayList();
		links = new ArrayList();

		Level level = new Level();
		StartLevel(level);
	}

	#region Level
	private void StartLevel(Level level) {

		ResetChain();

		Board.Instance.SetupForLevel(level);
		interactionEnabled = true;
	}
	#endregion


	#region Chain
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

		if(linkedToTile != null) {
			AddLink(linkedToTile, tile);
		}
	}

	public void TryRemoveTilesAfterTile(Tile tile) {

		// Try and find the index of the tile in the chain
		int chainIndex = -1;
		for(int i = 0; chainIndex < chain.Count; i++) {
			Tile chainTile = (Tile) chain[i];
		
			if(chainTile == tile) {
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

		// Remove the relevant links as well.
		for(int i = links.Count - 1; i >= chainIndex; i--) {
			GameObject linkObject = (GameObject) links[i];
			links.Remove(linkObject);
			DestroyImmediate(linkObject);
		}
	}

	public void ResetChain() {

		chain.Clear();

		ClearLinks();
	}
	#endregion 


	#region Links
	private void AddLink(Tile fromTile, Tile toTile) {

		// Create the link
		GameObject newLinkObject = Instantiate(dummyLinkObject) as GameObject;
		newLinkObject.SetActive(true);
		newLinkObject.name = "Linker_" + fromTile.boardCoord.Description() + "_to_" + toTile.boardCoord.Description();

		// Attach link to the from tile
		Transform newLinkTransform = newLinkObject.transform;
		newLinkTransform.parent = fromTile.transform;
		newLinkTransform.localScale = dummyLinkObject.transform.localScale;
		newLinkTransform.localPosition = Vector3.zero;

		// Rotate link to point between two tiles
		Vector2 v1 = new Vector2(fromTile.boardCoord.x, fromTile.boardCoord.y);
		Vector2 v2 = new Vector2(toTile.boardCoord.x, toTile.boardCoord.y);
		float angle = Mathf.Atan2(v1.y - v2.y, v1.x - v2.x) * Mathf.Rad2Deg;
		angle += 180;
		newLinkTransform.eulerAngles = new Vector3(0, 0, angle);

		// Keep in array so we can remove them all later
		links.Add(newLinkObject);
	}

	public void ClearLinks() {

		for(int i = links.Count - 1; i >= 0; i--) {
			GameObject link = (GameObject) links[i];
			DestroyImmediate(link);
		}

		links.Clear();
	}
	#endregion


	#region Complete chain
	public void TryCompleteChain() {

		if(chain.Count >= minChainLength) {
			interactionEnabled = false;

			StartCoroutine(CompleteChain());
		}
		else {

			foreach(Tile tile in chain) {
				tile.RemoveFromChain();
			}

			ResetChain();
		}
	}

	public IEnumerator CompleteChain() {

		ClearLinks();

		foreach(Tile tile in chain) {

			tile.RemoveFromBoard();

			UIManager.Instance.AddScoreText(tile.transform.localPosition, 20);

			yield return new WaitForSeconds(tileDisappearDelay);
		}

		yield return new WaitForSeconds(chain.Count * tileDisappearDelay);

		// Temp code to put tiles back for now
		foreach(Tile tile in chain) {
			tile.Reset();
		}

		ResetChain();

		interactionEnabled = true;
	}


	#endregion
}
