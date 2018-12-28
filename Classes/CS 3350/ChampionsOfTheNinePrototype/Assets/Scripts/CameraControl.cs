using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Character");
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.x >= 13.5 && player.transform.position.x <= 86.5) {
			transform.position = new Vector3 (player.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
		}
	}
}
