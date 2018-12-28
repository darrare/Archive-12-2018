using UnityEngine;
using System.Collections;

public class AudioControl  : MonoBehaviour {
	public AudioClip iceBolt;
	public AudioClip meteor;
	public AudioClip lightning;
	AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = gameObject.GetComponent<AudioSource> ();
	}


	public void PlayIce()
	{
		audio.PlayOneShot (iceBolt);
	}

	public void PlayMeteor()
	{
		audio.PlayOneShot (meteor);
	}

	public void PlayLightning()
	{
		audio.PlayOneShot (lightning);
	}
}
