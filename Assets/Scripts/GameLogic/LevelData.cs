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
}


