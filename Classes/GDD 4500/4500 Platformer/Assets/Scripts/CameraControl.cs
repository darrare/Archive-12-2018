using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	private GameObject player;
	private Vector3 spawn;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Character");
		spawn = transform.position;
	}
	
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (player.transform.position.x, spawn.y, spawn.z);
	}
}
