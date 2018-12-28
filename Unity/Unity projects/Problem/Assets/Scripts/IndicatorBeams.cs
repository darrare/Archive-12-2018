using UnityEngine;
using System.Collections;

public class IndicatorBeams : MonoBehaviour {
	bool isActive = true;
	// Use this for initialization
	void Start () {
		transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			transform.localScale = new Vector3 (Mathf.PingPong (Time.time * 2, 1) / 2, 1, 1);
		}

	}

	public void Deactivate()
	{
		isActive = false;
		transform.localScale = Vector3.zero;
	}
}
