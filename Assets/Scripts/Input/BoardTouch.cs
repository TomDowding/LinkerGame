using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class BoardTouch : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	void Update() {


		foreach (Touch touch in Input.touches) {
			HandleTouch(touch);
		}


		if (Input.GetMouseButton(0) ) {
			Touch fakeTouch = new Touch();
			fakeTouch.fingerId = 10;
			fakeTouch.position = Input.mousePosition;
			fakeTouch.deltaTime = Time.deltaTime;
			fakeTouch.phase = (Input.GetMouseButtonDown(0) ? TouchPhase.Began : 
				(fakeTouch.deltaPosition.sqrMagnitude > 1f ? TouchPhase.Moved : TouchPhase.Stationary) );
			fakeTouch.tapCount = 1;

			HandleTouch(fakeTouch);
		}


	}

	private void HandleTouch(Touch touch) {
		if (touch.phase == TouchPhase.Began) {

			Debug.Log("Touch began at" + touch.position);
		}
		if (touch.phase == TouchPhase.Moved) {

			Debug.Log("Touch moved at" + touch.position);
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {

		Debug.Log("OnPointerEnter " + eventData.position);

	}

	public void OnPointerExit(PointerEventData eventData) {

	
	}

	public void OnPointerDown(PointerEventData eventData) {

	}

	public void OnPointerUp(PointerEventData eventData) {


	}

}
