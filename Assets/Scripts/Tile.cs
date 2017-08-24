using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	[SerializeField]
	private TileSprite tileSprite;

	public int tileType;
	public BoardCoord boardCoord;

	void Start () {
		
	}
		
	public void SetupTile(int tileType, BoardCoord boardCoord, Sprite sprite) {

		this.boardCoord = boardCoord;
		this.tileType = tileType;

		this.tileSprite.SetSprite(sprite);
	}
		
	public void OnPointerEnter(PointerEventData eventData) {
		Debug.Log("Touch enter " + boardCoord.Description());
		tileSprite.ShowHover();
	}

	public void OnPointerExit(PointerEventData eventData) {
		Debug.Log("Touch exit " + boardCoord.Description());
		tileSprite.ShowNormal();
	}
}
