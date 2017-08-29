using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopupPanel : MonoBehaviour {

	[SerializeField]
	private Text titleText;

	[SerializeField]
	private Text messageText;

	[SerializeField]
	private Text buttonText;

	[SerializeField]
	private Animator revealAnimator;


	// Event delegates
	public delegate void PopupHandlerDelegate(PopupPanel source);
	public event PopupHandlerDelegate PopupButtonPressedEvent;

	private PopupHandlerDelegate currentDelegateMethod;


	public void Enable(bool immediate = false) {
		gameObject.SetActive(true);

		revealAnimator.SetBool("isHidden", false);
	}

	public void Disable(bool immediate = false) {

		// Remove any existing delegate event
		if(currentDelegateMethod != null) {
			PopupButtonPressedEvent -= currentDelegateMethod;
		}

		revealAnimator.SetBool("isHidden", true);
	}

	public void Setup(string title, string message, string button, PopupHandlerDelegate buttonEvent) {
		titleText.text = title;
		messageText.text = message;
		buttonText.text = button;

		currentDelegateMethod = buttonEvent;
		PopupButtonPressedEvent += currentDelegateMethod;
	}

	public void Popup_Btn() {
		if (PopupButtonPressedEvent != null) {
			PopupButtonPressedEvent(this);
		}
						
		Disable(true);
	}

	public void AnimationFinished() {
		Debug.Log("Animation Finished");
		if(revealAnimator.GetBool("isHidden")) {
			gameObject.SetActive(false);
		}
	}
}
