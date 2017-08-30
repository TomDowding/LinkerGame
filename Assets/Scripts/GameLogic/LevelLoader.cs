using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using System;

public class LevelLoader : MonoBehaviour {

	[SerializeField]
	private string[] levelNames;

	void Start() {
		CheckAllLevelsForErrors();
	}
		
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
	
		// Create the level object from the dictionary
		LevelData levelData = new LevelData(levelDict);

		// Unload the resource
		Resources.UnloadAsset(levelFile);

		return levelData;
	}
			
	public bool LevelExistsAtIndex(int levelIndex) {

		return levelIndex < levelNames.Length;
	}
		
	private void CheckAllLevelsForErrors() {

		for(int i = 0; i < levelNames.Length; i++) {

			LevelData levelData = LoadLevel(i);
			levelData.CheckForError(levelNames[i]);
		}
	}
}
