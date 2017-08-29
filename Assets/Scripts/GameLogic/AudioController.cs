using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	[SerializeField]
	private AudioSource audioSourceSmash;

	[SerializeField]
	private AudioSource audioSourceSelect;

	[SerializeField]
	private AudioSource audioSourceDeselect;


	public void SmashTile(int chainIndex) {

		audioSourceSmash.Play();
	}

	public void SelectTile() {

		audioSourceSelect.Play();
	}

	public void DeselectTile() {

		audioSourceDeselect.Play();
	}
}
