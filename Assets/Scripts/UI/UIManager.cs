using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[SerializeField]
	private GameObject dummyScoreObject;

	[SerializeField]
	private Transform floatingScoresHolder;

	[SerializeField]
	private Text scoreText;

	[SerializeField]
	private Text movesRemainingText;

	[SerializeField]
	private Image progressLine;

	[SerializeField]
	private Image progressNode;

	[SerializeField]
	private RectTransformMoveEaser scoreNodeEaser;

	[SerializeField]
	private Animator movesRemainingAnimator;


	public void ResetForLevel(LevelData level) {

		SetScore(0, level.targetScore, false);

		SetMovesRemaining(level.moves, false);
	}

	public void SetScore(int score, int target, bool animated = true) {

		// We never show the score being more than the target
		score = Mathf.Min(score, target);

		// Set the text
		string currentCodeString = score.ToString("D3");
		string targetCodeString = target.ToString("D3");
		scoreText.text = currentCodeString + "/" + targetCodeString;

		// Move the progress node
		float completeFactor = (float) score / (float) target;
		float maxLength = progressLine.rectTransform.rect.width;
		float nodeXPos = maxLength * completeFactor;
		Vector2 newPos = new Vector2(nodeXPos, progressNode.rectTransform.anchoredPosition.y);

		if(animated) {
			scoreNodeEaser.StartAnimation(null, progressNode.rectTransform.anchoredPosition, newPos, 0.25f);
		}
		else {
			progressNode.rectTransform.anchoredPosition = newPos;
		}
	}

	public void AddScoreText(Vector3 position, int score) {

		// Create the link
		GameObject newScoreTextObject = Instantiate(dummyScoreObject) as GameObject;
		newScoreTextObject.SetActive(true);
		newScoreTextObject.name = "ScoreText";

		// Attach link to the from tile
		Transform newScoreTextTransform = newScoreTextObject.transform;
		newScoreTextTransform.SetParent(floatingScoresHolder, false);
		newScoreTextTransform.localScale = Vector3.one;
		newScoreTextTransform.localPosition = position;

		Text text = newScoreTextObject.GetComponent<Text>();
		text.text = score.ToString();
	}

	public void SetMovesRemaining(int numMoves, bool animated = true) {

		movesRemainingText.text = numMoves.ToString();

		if(animated) {
			movesRemainingAnimator.SetTrigger("Go");
		}
			
	}
}
