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

	[SerializeField]
	private GameObject panelDeactivator;


	// Event delegate
	public delegate void PopupHandlerDelegate(PopupPanel source);
	public event PopupHandlerDelegate PopupButtonPressedEvent;
	private PopupHandlerDelegate delegateMethod;


	public void Enable() {

		panelDeactivator.SetActive(true);

		revealAnimator.SetBool("isHidden", false);
	}

	public void Disable() {

		// Remove any existing delegate event
		if(delegateMethod != null) {
			PopupButtonPressedEvent -= delegateMethod;
		}

		revealAnimator.SetBool("isHidden", true);
	}
		
	public void Setup(string title, string message, string button, PopupHandlerDelegate delegateMethod) {
		
		titleText.text = title;
		messageText.text = message;
		buttonText.text = button;

		this.delegateMethod = delegateMethod;
		PopupButtonPressedEvent += delegateMethod;
	}

	public void Popup_Btn() {

		if (PopupButtonPressedEvent != null) {
			PopupButtonPressedEvent(this);
		}

		Disable();
	}

	public void AnimationFinished() {
		
		if(revealAnimator.GetBool("isHidden")) {

			panelDeactivator.SetActive(false);
		}
	}
}
