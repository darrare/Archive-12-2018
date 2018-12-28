using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (FindObjectsOfType(GetType()).Length > 1) {
			DestroyImmediate (gameObject);
			return;
		}
		DontDestroyOnLoad (gameObject);
	}

	public void PlayOneShot(AudioClip clip)
	{
		GetComponent<AudioSource> ().clip = clip;
		GetComponent<AudioSource> ().Play ();
	}
}
