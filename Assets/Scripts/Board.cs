using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoardSize {
	// Unit is number of squares
	public int width;	
	public int height;
}

public struct BoardCoord {
	// A coordinate position on the board, in number of squares
	public int x;
	public int y;

	public BoardCoord(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public string Description() {
		return "(" + x + "," + y + ")";
	}
}

public class Board: Singleton<Board> {

	[SerializeField]
	private GameObject dummyBoardSquareObject;

	[SerializeField]
	private Transform boardSquareHolder;

	[SerializeField]
	private GameObject dummyTileObject;

	[SerializeField]
	private Transform tileHolder;

	[SerializeField] 
	private BoardSize boardSize;

	[SerializeField] 
	private Canvas canvas;

	private Tile[,] tiles;

	private BoardSquareSprite[,] boardSquares;

	private Vector2 boardSquareSize;

	private Vector2 boardOrigin;


	void Awake () {
		
	}

	void Start () {

	}

	public void SetupForLevel(Level level) {

		// Destroy our working prefabs instances in scene
		for(int i = 0; i < boardSquareHolder.childCount; i++) {
			DestroyImmediate(boardSquareHolder.GetChild(i).gameObject);
		}
		for(int i = 0; i < tileHolder.childCount; i++) {
			DestroyImmediate(tileHolder.GetChild(i).gameObject);
		}

		CreateLevel(level);
	}

	private void CreateLevel(Level level) {

		// Get the board square size from the dummy board square, to help with tile positioning
		BoardSquare boardSquare = dummyBoardSquareObject.GetComponent<BoardSquare>();
		boardSquareSize = boardSquare.GetSize();
		Debug.Log("boardSquareSize size: " + boardSquareSize.x + ", " + boardSquareSize.y);
	
		// Find the appropriate board start point for our board size
		// We want centered horizontally, but anchored to bottom of screen vertically
		RectTransform canvasRect = canvas.GetComponent<RectTransform>();
		boardOrigin = new Vector2((boardSquareSize.x * boardSize.width) * -0.5f, (canvasRect.rect.size.y  * -0.5f) + 5);

		// Create an initial array of tiles
		boardSquares = new BoardSquareSprite[boardSize.width, boardSize.height];
		tiles = new Tile[boardSize.width, boardSize.height];

		for (int y = 0; y < boardSize.height; y++) {
			for (int x = 0; x < boardSize.width; x++) {

				// TODO: Check if want a tile here or not (different levels)



				BoardCoord boardCoord = new BoardCoord(x, y);

				boardSquares[x, y] = CreateBoardSquare(boardCoord, boardSquareSize, boardOrigin);

				tiles[x, y] = CreateTile(boardCoord, boardSquareSize, boardOrigin);
			}
		}
	}

	private BoardSquareSprite CreateBoardSquare(BoardCoord boardCoord, Vector2 tileSize, Vector2 boardStartPos) {

		Vector2 tilePos = PositionForTile(tileSize, boardCoord);
		tilePos += boardStartPos;

		GameObject newBoardSquareObject = Instantiate(dummyBoardSquareObject) as GameObject;
		newBoardSquareObject.SetActive(true);
		newBoardSquareObject.name = "BoardSquare_" + boardCoord.x + "_" + boardCoord.y;

		Transform newBoardSquareTransform = newBoardSquareObject.transform;
		newBoardSquareTransform.parent = boardSquareHolder;
		newBoardSquareTransform.localPosition = new Vector3(tilePos.x, tilePos.y, 0);
		newBoardSquareTransform.localScale = Vector3.one;

		BoardSquareSprite newBoardSquareSprite = newBoardSquareObject.GetComponent<BoardSquareSprite>();
		//newBoardSquareSprite.SetSprite(tileSprites[tileType]);

		return newBoardSquareSprite;
	}

	private Tile CreateTile(BoardCoord boardCoord, Vector2 tileSize, Vector2 boardStartPos) {
		
		Vector2 tilePos = PositionForTile(tileSize, boardCoord);
		tilePos += boardStartPos;

		GameObject newTileObject = Instantiate(dummyTileObject) as GameObject;
		newTileObject.SetActive(true);
		newTileObject.name = "Tile_" + boardCoord.x + "_" + boardCoord.y;

		Transform newTileTransform = newTileObject.transform;
		newTileTransform.parent = tileHolder;
		newTileTransform.localPosition = new Vector3(tilePos.x, tilePos.y, 0);
		newTileTransform.localScale = Vector3.one;

		Tile newTile = newTileObject.GetComponent<Tile>();
		int tileType = GetRandomTileType();
		newTile.SetupTile(tileType, boardCoord);

		return newTile;
	}

	private int GetRandomTileType() {
		return Random.Range(0, SpriteManager.Instance.NumTileTypes());
	}
		
	private Vector2 PositionForTile(Vector2 tileSize, BoardCoord boardCoord) {
		return new Vector2((boardCoord.x * tileSize.x) + (tileSize.x * 0.5f), (boardCoord.y * tileSize.y) + (tileSize.y * 0.5f));
	}

	private Vector2 ScreenSpaceToBoardSpace(Vector2 screenPos) {

		float x = screenPos.x + boardOrigin.x;
		float y = screenPos.y + boardOrigin.y;
		return new Vector2(x, y);
	}
		


	private void ShuffleBoard() {

	}
}
