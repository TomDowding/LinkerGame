using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroller : MonoBehaviour {

	[SerializeField]
	private float speed = 0.01f;

	private Material material;


	void Start () {
		material = GetComponent<Renderer>().sharedMaterial;
	}

	void Update () {
		material.mainTextureOffset = new Vector2 (Time.time * speed, 0);
	}
}
