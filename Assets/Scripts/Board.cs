using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct BoardCoord {
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
	private Sprite[] tileSprites;

	[SerializeField] 
	private int boardSize = 9;

	private Tile[,] tiles;

	private BoardSquareSprite[,] boardSquares;

	private Vector2 boardSquareSize;

	private Vector2 boardOrigin;


	void Awake () {

	
	}

	void Start () {

		CreateInitialTiles();
	}
	
	void Update () {
		
	}

	private void CreateInitialTiles() {

		// Get the tile size from the dummy tile, to help with tile positioning
		BoardSquareSprite boardSquareSprite = dummyBoardSquareObject.GetComponent<BoardSquareSprite>();
		boardSquareSize = boardSquareSprite.GetTileSize();
		Debug.Log("boardSquareSize size: " + boardSquareSize.x + ", " + boardSquareSize.y);

		// Find the appropriate board start point for our board size
		boardOrigin = new Vector2((boardSquareSize.x * boardSize) * -0.5f, (boardSquareSize.y * boardSize) * -0.5f);
	
		// Create an initial array of tiles
		boardSquares = new BoardSquareSprite[boardSize, boardSize];
		tiles = new Tile[boardSize, boardSize];

		for (int y = 0; y < boardSize; y++) {
			for (int x = 0; x < boardSize; x++) {

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
		newTile.SetupTile(tileType, boardCoord, tileSprites[tileType]);

		return newTile;
	}

	private int GetRandomTileType() {
		return Random.Range(0, tileSprites.Length);
	}

	private Sprite GetRandomTileSprite() {
		int randomTypeIndex = Random.Range(0, tileSprites.Length);
		return tileSprites[randomTypeIndex];
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
