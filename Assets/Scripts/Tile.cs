using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	[SerializeField]
	private TileSprite tileSprite;

	public int tileType;
	public BoardCoord boardCoord;
	public bool inLink;

	void Start () {
			
	}
		
	public void SetupTile(int tileType, BoardCoord boardCoord) {
		inLink = false;

		this.boardCoord = boardCoord;
		this.tileType = tileType;

		tileSprite.Reset(tileType);
	}
		
	public void OnPointerEnter(PointerEventData eventData) {
		//Debug.Log("Touch enter " + boardCoord.Description());

		tileSprite.ShowInLink(tileType);

		tileSprite.ShowHover();
	}

	public void OnPointerExit(PointerEventData eventData) {
		//Debug.Log("Touch exit " + boardCoord.Description());

		tileSprite.ShowNoHover();

		if(!inLink) {
			tileSprite.ShowNotInLink(tileType);
		}
	}
}
