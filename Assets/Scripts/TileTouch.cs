using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TileTouch : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	[SerializeField]
	private Tile tile;


	public void OnPointerEnter(PointerEventData eventData) {


		tile.Select();
	}

	public void OnPointerExit(PointerEventData eventData) {

		tile.Deselect();
	}

	public void OnPointerDown(PointerEventData eventData) {

	}

	public void OnPointerUp(PointerEventData eventData) {

		tile.Deselect();

		GameManager.Instance.TryCompleteChain();
	}

}
