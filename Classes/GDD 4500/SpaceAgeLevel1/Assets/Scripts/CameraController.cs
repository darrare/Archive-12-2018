using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Character");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
	}
}
