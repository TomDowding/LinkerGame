using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TileTouch : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	[SerializeField]
	private Tile tile;


	public void OnPointerEnter(PointerEventData eventData) {

		Debug.Log("OnPointerEnter " + tile.boardCoord.description);

		tile.Select();
	}

	public void OnPointerExit(PointerEventData eventData) {

		Debug.Log("OnPointerExit " + tile.boardCoord.description);

		tile.Deselect();
	}

	public void OnPointerDown(PointerEventData eventData) {

	}

	public void OnPointerUp(PointerEventData eventData) {

		Debug.Log("OnPointerUp " + tile.boardCoord.description);

		tile.Deselect();

		GameManager.Instance.TryCompleteChain();
	}

}
