using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private Board board;

	[SerializeField]
	private UIManager uiManager;

	[SerializeField]
	private LevelLoader levelLoader;

	[SerializeField]
	private GameObject dummyLinkObject;

	[SerializeField]
	private float tileDisappearDelay = 0.4f;

	[SerializeField]
	private AudioSource levelWinClip;


	// Game data holders
	private ArrayList chain;

	private ArrayList links;

	// Game progress
	private int currentLevelNum;

	private LevelData currentLevel;

	private int currentScore;

	private int currentNumMovesMade;

	public bool interactionEnabled {get; private set;}

	// Game constants
	private const int minChainLength = 3;

	private const int scorePerGem = 20;

	private const int extraScoreForLongChainGem = 10;


	void Start () {
		
		chain = new ArrayList();
		links = new ArrayList();

		StartGame();
	}
		
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
			Destroy(linkObject);
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

		// Diagonal links are a hypoteneus and need to be longer by ratio of approx. sqrt 2
		if(angle % 90 != 0) {
			newLinkTransform.localScale = new Vector3(newLinkTransform.localScale.x * 1.41f, newLinkTransform.localScale.y, newLinkTransform.localScale.z);
		}


		// Keep in array so we can remove them all later
		links.Add(newLinkObject);
	}

	private void ClearLinks() {

		for(int i = links.Count - 1; i >= 0; i--) {
			GameObject link = (GameObject) links[i];
			Destroy(link);
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
		
		// Remove chain links from scene
		ClearLinks();

		// Remove tiles in the chain 
		//foreach(Tile tile in chain) {
		for(int chainIndex = 0; chainIndex < chain.Count; chainIndex++) {
			
			Tile tile = (Tile) chain[chainIndex];

			tile.RemoveFromBoard(chainIndex);

			AddTileScore(tile, chainIndex);

			// Small delay so they disappear in a chain
			yield return new WaitForSeconds(tileDisappearDelay);

			board.RemoveTile(tile);

			Destroy(tile.gameObject);
		}

		yield return new WaitForSeconds(tileDisappearDelay);

		// Fill in the gaps created by missing tiles with the existing tiles above
		ArrayList dropTileColumns = board.FillGapsWithExistingTiles();
		yield return StartCoroutine(DropColumns(dropTileColumns, 0.1f));

		// Try for level success now. If it is true we don't need to distract with new tiles
		if(TryLevelSuccess()) {
			UseMove();
			yield break;
		}
	
		// Fill in the remaining gaps with new tiles
		ArrayList newTileColumns = board.FillGapsWithNewTiles(currentLevel, this);
		yield return StartCoroutine(DropColumns(newTileColumns, 0.18f));

		UseMove();

		ResetChain();
	}
		
	private IEnumerator DropColumns(ArrayList dropColumns, float dropDelay) {

		int maxNumRows = 0;

		// Drop all columns simultaneously.
		for(int col = 0; col < dropColumns.Count; col++) {

			ArrayList column = (ArrayList) dropColumns[col];
			StartCoroutine(DropColumn(column, dropDelay));
			if(column.Count > maxNumRows) {
				maxNumRows = column.Count;
			}
		}
			
		// Wait for all columns to have completed all rows before continuing
		float finishedDelay = maxNumRows * dropDelay;

		yield return new WaitForSeconds(finishedDelay);
	}
		
	private IEnumerator DropColumn(ArrayList column, float dropDelay) {

		// Drop all rows in the column with a small delay in between each one
		for(int row = 0; row < column.Count; row++) {

			Tile tile = (Tile) column[row];

			tile.Drop(board.PositionForBoardCoord(tile.boardCoord));

			yield return new WaitForSeconds(dropDelay);
		}
	}
	#endregion


	#region Game progress
	private void StartGame() {

		currentLevelNum = 0;

		PrepareLevel(currentLevelNum);
	}

	private void PrepareLevel(int levelNum, bool isRetry = false) {

		PrepareLevel(levelLoader.LoadLevel(levelNum), levelNum, isRetry);
	}

	private void PrepareLevel(LevelData level, int levelNum, bool isRetry = false) {
		
		interactionEnabled = false;

		currentLevel = level;

		currentScore = 0;

		currentNumMovesMade = 0;

		uiManager.ResetForLevel(level);

		ResetChain();

		board.SetupLevel(level, this);

		if(isRetry) {
			StartLevel();
		}
		else {
			PopupPanel.PopupHandlerDelegate delegateMethod = LevelIntroPopupPressed;
			uiManager.ShowLevelIntro(level, levelNum, delegateMethod);
		}
	}
		
	private void StartLevel() {

		interactionEnabled = true;
	}

	private void RetryLevel() {
		
		PrepareLevel(currentLevel, currentLevelNum, true);
	}

	private void NextLevel() {
		
		currentLevelNum ++;

		// Check for all levels completed
		if(!levelLoader.LevelExistsAtIndex(currentLevelNum)) {
			PopupPanel.PopupHandlerDelegate delegateMethod = GameCompletePopupPressed;
			uiManager.ShowGameComplete(delegateMethod);
		}
		else {
			PrepareLevel(currentLevelNum);
		}
	}

	private void UseMove() {

		currentNumMovesMade++;

		uiManager.SetMovesRemaining(currentLevel.moves - currentNumMovesMade);

		// Test for lost level
		if((currentNumMovesMade == currentLevel.moves) && (currentScore < currentLevel.targetScore)) {
			LevelFailure();
		}
	}

	private void AddTileScore(Tile tile, int chainIndex) {

		// Calculate new score for this tile
		int tileScore = scorePerGem;
		if(chainIndex >= minChainLength) {
			tileScore = scorePerGem + (extraScoreForLongChainGem * ((chainIndex + 1) - minChainLength));
		}

		// Add on the tile score to current total score
		currentScore += tileScore;
		currentScore = Mathf.Min(currentScore, currentLevel.targetScore);

		// Show the UI for the new score
		uiManager.AddFloatingScore(tile.transform.localPosition, tileScore);
		uiManager.SetScore(currentScore, currentLevel.targetScore);

		// Check for level win. We'll start the music now,
		// but won't do anything else until all tile scores added
		if(CheckForLevelWin()) {
			Debug.Log("level win score " + levelWinClip.isPlaying);
			if(!levelWinClip.isPlaying) {
				levelWinClip.Play();
			}
		}
	}
		
	private bool TryLevelSuccess() {
		if(CheckForLevelWin()) {
			LevelSuccess();
			return true;
		}
		return false;
	}

	private bool CheckForLevelWin() {
		return currentScore >= currentLevel.targetScore;
	}
		
	private void LevelSuccess() {
		
		interactionEnabled = false;

		PopupPanel.PopupHandlerDelegate delegateMethod = LevelSuccessPopupPressed;
		uiManager.ShowLevelSuccess(delegateMethod);
	}

	private void LevelFailure() {

		interactionEnabled = false;

		PopupPanel.PopupHandlerDelegate delegateMethod = LevelFailurePopupPressed;
		uiManager.ShowLevelFailure(delegateMethod);
	}
	#endregion


	#region Popup Delegate methods
	public void LevelIntroPopupPressed(PopupPanel source) {

		StartLevel();
	}

	public void LevelSuccessPopupPressed(PopupPanel source) {

		StartCoroutine(LevelSuccessPopupPressedDelay());
	}

	public IEnumerator LevelSuccessPopupPressedDelay() {
		
		yield return new WaitForSeconds(1.0f);
		NextLevel();
	}

	public void LevelFailurePopupPressed(PopupPanel source) {

		RetryLevel();
	}

	public void GameCompletePopupPressed(PopupPanel source) {
		
		StartCoroutine(GameCompletePopupPressedDelay());
	}

	public IEnumerator GameCompletePopupPressedDelay() {

		yield return new WaitForSeconds(1.0f);
		StartGame();
	}
	#endregion
}
