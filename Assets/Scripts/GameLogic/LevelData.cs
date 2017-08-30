using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelData {

	public int[,] tiles;
	public int numCols;
	public int numRows;
	public int targetScore;
	public int moves;
	public int numColours;

	public int GetRandomTileType() {
		return UnityEngine.Random.Range(0, numColours);
	}

	public LevelData(Dictionary<string, object> levelDict) {

		targetScore = System.Convert.ToInt32(levelDict["targetScore"]);
		moves = System.Convert.ToInt32(levelDict["moves"]);
		numColours = System.Convert.ToInt32(levelDict["colours"]);
		List<object> rowList = (List<object>)levelDict["tiles"];

		TilesErrorChecker(rowList);

		tiles = GetTileArrayFromRowList(rowList);

		numRows = rowList.Count;
		var firstRow = (List<object>)rowList[0];
		numCols = firstRow.Count;
	}

	private int[,] GetTileArrayFromRowList(List<object> rowList) {

		// Use the length of first row to size our 2d array of tiles
		var firstRow = (List<object>)rowList[0];
		int[,] tiles = new int[firstRow.Count, rowList.Count];

		for (int rowIndex = rowList.Count - 1; rowIndex >= 0; rowIndex--) {

			var row = (List<object>)rowList[rowIndex];

			for (int colIndex = 0; colIndex < row.Count; colIndex++) {

				// For each row and column pair set the int from the json
				int tileInt = System.Convert.ToInt32(row[colIndex]);

				// The json file is in top to bottom order of rows. 
				// Our board coordinate system has origin at bottom left so we convert the row index.
				int reversedRowIndex = (rowList.Count - 1) - rowIndex;

				tiles[colIndex, reversedRowIndex] = tileInt;
			}
		}

		return tiles;
	}

	#region Error checking
	private void TilesErrorChecker(List<object> rowList) {

		int lastNumTilesInRow = -1;

		for (int rowIndex = rowList.Count - 1; rowIndex >= 0; rowIndex--) {

			var row = (List<object>)rowList[rowIndex];

			// Find if this row has a different amount of columns to the last one
			if(lastNumTilesInRow > 0 && lastNumTilesInRow != row.Count) {
				Debug.LogWarning("Not all rows equal, both " + lastNumTilesInRow + " and " + row.Count + " columns");
			}
			lastNumTilesInRow = row.Count;
		}
	}


	public bool CheckForError(string levelName) {

		if(numCols > GameManager.maxNumCols) {
			Debug.LogWarning(levelName + " has too many cols (" + numCols + ")");
			return false;
		}
		if(numRows > GameManager.maxNumRows) {
			Debug.LogWarning(levelName + " has too many rows (" + numRows + ")");
			return false;
		}

		for(int col = 0; col < numCols; col++) {

			for(int row = 0; row < numRows; row++) {

				if(tiles[col, row] > 1) {
					Debug.LogWarning(levelName + " has strange tile index " + tiles[col, row] + " at (" + col + ", " + row + ")");
					return false;
				}
			}
		}
			
		return true;
	}
	#endregion
}


