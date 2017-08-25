using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TileTouch : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	[SerializeField]
	private Tile tile;


	public void OnPointerEnter(PointerEventData eventData) {

		if(!GameManager.Instance.interactionEnabled) {
			return;
		}

		tile.Select();
	}

	public void OnPointerExit(PointerEventData eventData) {

		if(!GameManager.Instance.interactionEnabled) {
			return;
		}

		tile.Deselect();
	}

	public void OnPointerDown(PointerEventData eventData) {

		if(!GameManager.Instance.interactionEnabled) {
			return;
		}
	}

	public void OnPointerUp(PointerEventData eventData) {

		if(!GameManager.Instance.interactionEnabled) {
			return;
		}

		tile.Deselect();

		GameManager.Instance.TryCompleteChain();
	}

}
