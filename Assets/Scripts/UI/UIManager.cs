using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[SerializeField]
	private GameObject dummyScoreObject;

	[SerializeField]
	private Transform scoreTextHolder;


	public void AddScoreText(Vector3 position, int score) {

		// Create the link
		GameObject newScoreTextObject = Instantiate(dummyScoreObject) as GameObject;
		newScoreTextObject.SetActive(true);
		newScoreTextObject.name = "ScoreText";

		// Attach link to the from tile
		Transform newScoreTextTransform = newScoreTextObject.transform;
		newScoreTextTransform.SetParent(scoreTextHolder, false);
		newScoreTextTransform.localScale = Vector3.one;
		newScoreTextTransform.localPosition = position;

		Text text = newScoreTextObject.GetComponent<Text>();
		text.text = score.ToString();
	}
}
