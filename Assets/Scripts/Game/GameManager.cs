using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	[SerializeField]
	private Board board;

	[SerializeField]
	private UIManager uiManager;

	[SerializeField]
	private GameObject dummyLinkObject;

	private ArrayList chain;

	private ArrayList links;

	[SerializeField]
	private float tileDisappearDelay = 0.4f;

	[SerializeField]
	private float tileDropDelay = 0.4f;

	[SerializeField]
	private float tileDropDuration = 0.25f;


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

		board.SetupForLevel(level);
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
			float distance = Vector2.Distance(new Vector2(lastTile.boardCoord.col, lastTile.boardCoord.row), new Vector2(tile.boardCoord.col, tile.boardCoord.row));
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

	private void ResetChain() {

		chain.Clear();

		ClearLinks();

		interactionEnabled = true;
	}
	#endregion 


	#region Links
	private void AddLink(Tile fromTile, Tile toTile) {

		// Create the link
		GameObject newLinkObject = Instantiate(dummyLinkObject) as GameObject;
		newLinkObject.SetActive(true);
		newLinkObject.name = "Linker_" + fromTile.boardCoord.description + "_to_" + toTile.boardCoord.description;

		// Attach link to the from tile
		Transform newLinkTransform = newLinkObject.transform;
		newLinkTransform.parent = fromTile.transform;
		newLinkTransform.localScale = dummyLinkObject.transform.localScale;
		newLinkTransform.localPosition = Vector3.zero;

		// Rotate link to point between two tiles
		Vector2 v1 = new Vector2(fromTile.boardCoord.col, fromTile.boardCoord.row);
		Vector2 v2 = new Vector2(toTile.boardCoord.col, toTile.boardCoord.row);
		float angle = Mathf.Atan2(v1.y - v2.y, v1.x - v2.x) * Mathf.Rad2Deg;
		angle += 180;
		newLinkTransform.eulerAngles = new Vector3(0, 0, angle);

		// Keep in array so we can remove them all later
		links.Add(newLinkObject);
	}

	private void ClearLinks() {

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

	private IEnumerator CompleteChain() {

		ClearLinks();

		// Remove tiles in the chain 
		foreach(Tile tile in chain) {

			// Visual remove
			tile.RemoveFromBoard();

			uiManager.AddScoreText(tile.transform.localPosition, 20);

			yield return new WaitForSeconds(tileDisappearDelay);

			// Data remove
			board.RemoveTile(tile);

			Destroy(tile.gameObject);
		}

		yield return new WaitForSeconds(tileDisappearDelay);

		// Fill in the gaps created by missing tiles
		ArrayList dropColumns = board.FillGaps();
		yield return StartCoroutine(DropTiles(dropColumns));

		// Get ready for next chain
		ResetChain();
	}

		
	private IEnumerator DropTiles(ArrayList dropColumns) {

		int mostRowsToDrop = 0;

		for(int col = 0; col < dropColumns.Count; col++) {

			ArrayList column = (ArrayList) dropColumns[col];

			if(column.Count > mostRowsToDrop) {
				mostRowsToDrop = column.Count;
			}

			StartCoroutine(DropColumn(column));
		}

		// Wait for columns to have dropped all their rows 
		yield return new WaitForSeconds(mostRowsToDrop * tileDropDuration);

		// Get ready for next chain
		ResetChain();
	}
		
	private IEnumerator DropColumn(ArrayList column) {

		for(int row = 0; row < column.Count; row++) {

			Tile tile = (Tile) column[row];

			tile.Drop(board.PositionForBoardCoord(tile.boardCoord), tileDropDuration);

			yield return new WaitForSeconds(tileDropDelay);
		}
	}
	#endregion
}
