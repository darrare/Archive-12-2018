using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundEffects : MonoBehaviour {

	public AudioSource[] sources = new AudioSource[10];
	int index = 0;
	int maxIndex;

	// Use this for initialization
	void Start () {
		if (FindObjectsOfType(GetType()).Length > 1) {
			DestroyImmediate (gameObject);
			return;
		}
		maxIndex = sources.Length;
		DontDestroyOnLoad (transform.gameObject);
	}

	public void PlayOneShot(AudioClip clip)
	{
		sources [index].PlayOneShot (clip);
		index++;
		if (index >= maxIndex) {
			index = 0;
		}
	}
}
