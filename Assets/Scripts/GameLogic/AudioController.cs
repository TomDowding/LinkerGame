using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController> {

	[SerializeField]
	private AudioSource audioSourceSmash;

	[SerializeField]
	private AudioSource audioSourceSelect;

	[SerializeField]
	private AudioSource audioSourceDrop;

	[SerializeField]
	private AudioSource audioSourceButton;


	public void SmashTile(int chainIndex) {

		audioSourceSmash.Play();
	}

	public void SelectTile() {

		audioSourceSelect.Play();
	}

	public void DropTile() {

		audioSourceDrop.Play();
	}

	public void Button() {

		audioSourceButton.Play();
	}
}
