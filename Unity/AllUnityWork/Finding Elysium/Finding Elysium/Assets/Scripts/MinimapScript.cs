using UnityEngine;
using System.Collections;

public class MinimapScript : MonoBehaviour {

    Camera cam;
	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.Instance.Ship)
        {
            transform.position = new Vector3(GameManager.Instance.Ship.transform.position.x, GameManager.Instance.Ship.transform.position.y, transform.position.z);
            //cam.orthographicSize = Relative to speed of ship
        }
	}
}
