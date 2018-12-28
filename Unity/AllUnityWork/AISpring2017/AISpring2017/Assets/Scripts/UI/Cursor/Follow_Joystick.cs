using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Joystick : MonoBehaviour
{
    public GameObject select;
    public Vector3 updatePosition;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((Input.GetJoystickNames().Length > 0) && (Input.GetJoystickNames()[0] != ""))
        {
            updatePosition.x = Input.GetAxis("SelectJoystickHorizontal");
            updatePosition.y = Input.GetAxis("SelectJoystickVertical");

            transform.position += new Vector3(updatePosition.x, updatePosition.y, 0);
        }
        else
        {
            transform.position = select.transform.position;
        }
    }
}
