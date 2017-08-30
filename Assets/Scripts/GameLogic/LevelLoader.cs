using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using System;

public class LevelLoader : MonoBehaviour {

	[SerializeField]
	private string[] levelNames;



	public LevelData LoadLevel(int levelNum) {

		if(levelNum >= levelNames.Length) {
			Debug.LogError("Level number " + levelNum + " is too high for num levels: " + levelNames.Length);
			return null;
		}

		// Load in the json file to a text asset
		string filepath = "LevelFiles/" + levelNames[levelNum];
		TextAsset levelFile = Resources.Load<TextAsset>(filepath);

		if(levelFile == null) {
			Debug.LogWarning("Null level for filepath " + filepath);
			return null;
		}
			
		// Deserialize the json to a dictionary 
		Dictionary<string, object> levelDict = (Dictionary<string, object>)Json.Deserialize(levelFile.text); 
		LevelErrorChecker(levelDict);

		// Create the level object from the dictionary
		LevelData levelData = new LevelData();
		levelData.targetScore = System.Convert.ToInt32(levelDict["targetScore"]);
		levelData.moves = System.Convert.ToInt32(levelDict["moves"]);
		levelData.numColours = System.Convert.ToInt32(levelDict["colours"]);

		List<object> rowList = (List<object>)levelDict["tiles"];
		levelData.tiles = GetTileArrayFromRowList(rowList);

		levelData.numRows = rowList.Count;
		var firstRow = (List<object>)rowList[0];
		levelData.numCols = firstRow.Count;

		Debug.Log("level data " + levelData.numCols + " cols, " + levelData.numRows + " rows");
		Debug.Log("level data has target score: " + levelData.targetScore);
		Debug.Log("level data has moves: " + levelData.moves);
		Debug.Log("level data has colours: " + levelData.numColours);

		Resources.UnloadAsset(levelFile);

		return levelData;
	}


	private void LevelErrorChecker(Dictionary<string, object> levelDict) {

		var tilesList = (List<object>)levelDict["tiles"];

		int lastNumTilesInRow = -1;
	
		for (int rowIndex = 0; rowIndex < tilesList.Count; rowIndex++) {

			var row = (List<object>)tilesList[rowIndex];

			if(lastNumTilesInRow > 0 && lastNumTilesInRow != row.Count) {
				Debug.LogWarning("Not a square level, found rows with both " + lastNumTilesInRow + " and " + row.Count + " tiles");
			}

			lastNumTilesInRow = row.Count;
		}
	}

	private int[,] GetTileArrayFromRowList(List<object> rowList) {

		// Use the length of first row to size our 2d array of tiles
		var firstRow = (List<object>)rowList[0];
		int[,] tiles = new int[firstRow.Count, rowList.Count];

		// For each row and column pair set the int from the json
		for (int rowIndex = rowList.Count - 1; rowIndex >= 0; rowIndex--) {
			
			var row = (List<object>)rowList[rowIndex];

			for (int colIndex = 0; colIndex < row.Count; colIndex++) {

				int tileInt = System.Convert.ToInt32(row[colIndex]);

				// The json file is in top to bottom order of rows. 
				// Our board coordinate system has origin at bottom left so we convert the row index.
				int reversedRowIndex = (rowList.Count - 1) - rowIndex;
	
				tiles[colIndex, reversedRowIndex] = tileInt;

				//Debug.Log("Tile at (" + colIndex + ", " + reversedRowIndex + ") is " + tileInt);
			}
		}

		return tiles;
	}

	public int GetNumLevels() {

		return levelNames.Length;
	}
}
