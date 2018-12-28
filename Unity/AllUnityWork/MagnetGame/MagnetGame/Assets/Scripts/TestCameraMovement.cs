using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.position = new Vector3(ObjectiveScript.Instance.transform.position.x, Mathf.Lerp(transform.position.y, ObjectiveScript.Instance.transform.position.y, Time.deltaTime), transform.position.z);
	}
}
