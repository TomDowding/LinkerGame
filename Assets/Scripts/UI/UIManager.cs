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
	private ProgressMeter scoreProgressMeter;

	[SerializeField]
	private Animator movesRemainingAnimator;

	[SerializeField]
	private PopupPanel levelIntroPopupPanel;

	[SerializeField]
	private PopupPanel levelOutroPopupPanel;



	public void ResetForLevel(LevelData level) {

		SetScore(0, level.targetScore, false);

		SetMovesRemaining(level.moves, false);
	}

	#region Top HUD
	public void SetScore(int score, int target, bool animated = true) {

		// We never show the score being more than the target
		score = Mathf.Min(score, target);

		// Set the text. Padded zeroes of score matches the length of the target score string
		string targetCodeString = target.ToString();
		string currentCodeString = score.ToString("D" + targetCodeString.Length);
		scoreText.text = currentCodeString + "/" + targetCodeString;

		// Move the meter
		scoreProgressMeter.MoveMeter((float) score, (float) target, animated);
	}

	public void AddFloatingScore(Vector3 position, int score) {

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
	#endregion


	#region Popup Panel
	public void ShowLevelSuccess( PopupPanel.PopupHandlerDelegate delegateMethod) {

		string popupTitle = "Level Complete!";
		string popupMessage = "ROAR!!!";
		string buttonText = "Next Level";
		levelOutroPopupPanel.Setup(popupTitle, popupMessage, buttonText, delegateMethod);
		levelOutroPopupPanel.Enable();
	}

	public void ShowLevelFailure(PopupPanel.PopupHandlerDelegate delegateMethod) {

		string popupTitle = "No Moves Left!";
		string popupMessage = "AAARGH!";
		string buttonText = "Retry";
		levelOutroPopupPanel.Setup(popupTitle, popupMessage, buttonText, delegateMethod);
		levelOutroPopupPanel.Enable();
	}

	public void ShowLevelIntro(LevelData level, int levelNum, PopupPanel.PopupHandlerDelegate delegateMethod) {

		string popupTitle = "Level " + (levelNum + 1).ToString();
		string popupMessage = "Target: " + level.targetScore + "\nMoves: " + level.moves;
		string buttonText = "Play";

		levelIntroPopupPanel.Setup(popupTitle, popupMessage, buttonText, delegateMethod);
		levelIntroPopupPanel.Enable();
	}

	public void ShowGameComplete( PopupPanel.PopupHandlerDelegate delegateMethod) {

		string popupTitle = "Game Complete!";
		string popupMessage = "King of\nthe Jungle!";
		string buttonText = "Restart";

		levelIntroPopupPanel.Setup(popupTitle, popupMessage, buttonText, delegateMethod);
		levelIntroPopupPanel.Enable();
	}
	#endregion
}
