using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {
	public GameObject images;
	float timeToDecay = 2.5f; //2.5 looks good

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < images.transform.childCount; i++) {
			images.transform.GetChild (i).GetComponent<RawImage> ().color = new Color (1, 1, 1, (Time.time - .5f) / 2);
		}
		if (Time.time >= timeToDecay) {
			SceneManager.LoadScene ("MainMenu");
		}
	}
}
