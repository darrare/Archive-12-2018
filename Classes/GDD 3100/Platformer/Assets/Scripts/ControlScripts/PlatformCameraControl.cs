using UnityEngine;
using System.Collections;

public class PlatformCameraControl : MonoBehaviour {
	
	private GameObject player;
	private Vector3 spawn;
	float[] cameraLimits = new float[4];
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Character");
		spawn = transform.position;
	}

	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (
			Mathf.Clamp (player.transform.position.x, cameraLimits[0], cameraLimits[1]), 
			Mathf.Clamp (player.transform.position.y, cameraLimits[2], cameraLimits[3]), spawn.z);
	}

	public void SetCameraLimits(float[] limits)
	{
		cameraLimits [0] = limits[0];
		cameraLimits [1] = limits[1];
		cameraLimits [2] = limits[2];
		cameraLimits [3] = limits[3];
	}
}
