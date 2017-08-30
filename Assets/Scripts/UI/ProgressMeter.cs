using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressMeter : MonoBehaviour {

	[SerializeField]
	private Image progressLine;

	[SerializeField]
	private Image progressNode;

	[SerializeField]
	private RectTransformMoveEaser scoreNodeEaser;


	public void MoveMeter (float value, float max, bool animated) {

		float completeFactor = value / max;
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
}
