using UnityEngine;
using System.Collections;

public class SimpleCharacterController : MonoBehaviour {
    Vector3 velocity = Vector3.zero;
    float speed = 1500f;
    Rigidbody rbody;

	// Use this for initialization
	void Start () {
        rbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Handle simple input
        velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            velocity += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity -= transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += transform.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity -= transform.right;
        }
        velocity.Normalize();
        velocity *= speed * Time.deltaTime;

        rbody.velocity = new Vector3(velocity.x, rbody.velocity.y, velocity.z);


        //Handle jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rbody.velocity = transform.up * 20f;
        }


    }
}
