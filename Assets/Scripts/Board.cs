using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Custom structs to help define board properties
[System.Serializable]
public struct BoardSize {
	public int numCols;	
	public int numRows;
}

public struct BoardCoord {
	public int col;
	public int row;
	public string description {
		get {return "(" + col + "," + row + ")";}
		private set{;}
	}

	public BoardCoord(int col, int row) {
		this.col = col;
		this.row = row;
	}
}

public struct BoardSquareSize {
	public float width;	
	public float height;
}
#endregion


public class Board: MonoBehaviour {

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

	private BoardSquare[,] boardSquares;

	private BoardSquareSize boardSquareSize;

	private Vector2 boardOrigin;


	void Awake () {
		
	}

	void Start () {

	}

	public void SetupForLevel(Level level) {

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
		Debug.Log("boardSquareSize size: " + boardSquareSize.width + ", " + boardSquareSize.height);
	
		// Find the appropriate board start point for our board size
		// We want centered horizontally, but anchored to bottom of screen vertically
		RectTransform canvasRect = canvas.GetComponent<RectTransform>();
		boardOrigin = new Vector2((boardSquareSize.width * boardSize.numCols) * -0.5f, (canvasRect.rect.size.y  * -0.5f) + 15);

		// Create an initial array of tiles
		boardSquares = new BoardSquare[boardSize.numCols, boardSize.numRows];
		tiles = new Tile[boardSize.numCols, boardSize.numRows];

		for (int row = 0; row < boardSize.numRows; row++) {
			for (int col = 0; col < boardSize.numCols; col++) {

				// TODO: Check if want a tile here or not (different levels)

				if(col == 2 && row % 2 == 0) {
					continue;
				}

				BoardCoord boardCoord = new BoardCoord(col, row);

				boardSquares[col, row] = CreateBoardSquare(boardCoord);

				tiles[col, row] = CreateTile(boardCoord);
			}
		}
	}

	private BoardSquare CreateBoardSquare(BoardCoord boardCoord) {

		Vector2 pos = PositionForBoardCoord(boardCoord);
	
		GameObject newBoardSquareObject = Instantiate(dummyBoardSquareObject) as GameObject;
		newBoardSquareObject.SetActive(true);
	
		Transform newBoardSquareTransform = newBoardSquareObject.transform;
		newBoardSquareTransform.parent = boardSquareHolder;
		newBoardSquareTransform.localPosition = new Vector3(pos.x, pos.y, 0);
		newBoardSquareTransform.localScale = Vector3.one;

		BoardSquare newBoardSquare = newBoardSquareObject.GetComponent<BoardSquare>();
		newBoardSquare.Setup(boardCoord);
	
		return newBoardSquare;
	}

	private Tile CreateTile(BoardCoord boardCoord) {
		
		Vector2 tilePos = PositionForBoardCoord(boardCoord);
	
		GameObject newTileObject = Instantiate(dummyTileObject) as GameObject;
		newTileObject.SetActive(true);
	
		Transform newTileTransform = newTileObject.transform;
		newTileTransform.parent = tileHolder;
		newTileTransform.localPosition = new Vector3(tilePos.x, tilePos.y, 0);
		newTileTransform.localScale = Vector3.one;

		Tile newTile = newTileObject.GetComponent<Tile>();
		newTile.Setup(GetRandomTileType(), boardCoord);

		return newTile;
	}

	private int GetRandomTileType() {
		return Random.Range(0, SpriteManager.Instance.NumTileTypes());
	}
		
	public Vector2 PositionForBoardCoord(BoardCoord boardCoord) {
		Vector2 pos = new Vector2((boardCoord.col * boardSquareSize.width) + (boardSquareSize.width * 0.5f), (boardCoord.row * boardSquareSize.height) + (boardSquareSize.height * 0.5f));
		pos += boardOrigin;
		return pos;
	}
		
	public void RemoveTile(Tile tile) {
		// Creates a 'gap' in the tiles array
		tiles[tile.boardCoord.col, tile.boardCoord.row] = null;
	}

	private void MoveTile(Tile tile, BoardCoord fromCoord, BoardCoord toCoord) {
		tile.Setup(tile.tileType, toCoord);
		tiles[toCoord.col, toCoord.row] = tile;
		tiles[fromCoord.col, fromCoord.row] = null;
	}

	public ArrayList FillGaps() {

		// This array will hold all the tiles that have been moved, split into ordered columns.
		// We use this for animating the drop down after we change the data model here.
		ArrayList columns = new ArrayList();

		for(int col = 0; col < boardSize.numCols; col++) {

			ArrayList column = new ArrayList();
			columns.Add(column);

			for(int row = 0; row < boardSize.numRows; row++) {

				// A gap exists if there is a board square but no tile
				if(boardSquares[col, row] != null && tiles[col, row] == null) {

					Debug.Log("Gap at (" + col + ", " + row + ")");

					// Start scanning upwards to find first tile above gap
					for(int scanRow = row + 1; scanRow < boardSize.numRows; scanRow++) {
						
						if(tiles[col, scanRow] != null) {

							// Move found tile to the gap
							Tile foundTile = tiles[col, scanRow];
							MoveTile(foundTile,  new BoardCoord(col, scanRow),  new BoardCoord(col, row));
							column.Add(foundTile);

							break;
						}
					}
				}
			}
		}

		for(int col = 0; col < columns.Count; col++) {

			ArrayList column = (ArrayList) columns[col];

			for(int row = 0; row < column.Count; row++) {

				Debug.Log("Tile to drop at (" + col + ", " + row + ")");
			}

		}

		return columns;
	}
}
