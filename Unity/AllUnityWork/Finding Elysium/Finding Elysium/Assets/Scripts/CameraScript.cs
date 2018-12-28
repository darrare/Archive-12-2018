using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (GameManager.Instance.Ship != null)
        {
            transform.position = new Vector3(GameManager.Instance.Ship.transform.position.x, GameManager.Instance.Ship.transform.position.y, -50);
        }
	}
}
