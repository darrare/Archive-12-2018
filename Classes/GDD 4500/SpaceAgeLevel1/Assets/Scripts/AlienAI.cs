using UnityEngine;
using System.Collections;

public class AlienAI : MonoBehaviour {

	private float velocity = .7f;
	private float JumpVelocity = 3;
	private bool movingRight = true;
	private GameObject player;
	private bool facingLeft = true;
	
	
	// Use this for initialization
	void Start () {
		InvokeRepeating ("RandomDelay", 0, 1);
		player = GameObject.Find ("Character");
	}
	
	// Update is called once per frame
	void Update () {
		if (movingRight) {
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(-velocity, gameObject.GetComponent<Rigidbody2D>().velocity.y);
		} else if (!movingRight) {
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(velocity, gameObject.GetComponent<Rigidbody2D>().velocity.y);
		} 

		if (movingRight) {
			Flip (0);
		} 
		else {
			Flip (1);
		}
	}
	
	void Flip(int code){
		if (code == 0) {
			facingLeft = false;
			transform.localScale = new Vector3 (1, 1, 1);
		} else {
			facingLeft = true;
			transform.localScale = new Vector3 (-1, 1, 1);
		}
	}


	
	void RandomDelay()
	{
		int random = Random.Range (1, 3); //1 or 2
		if (random == 1) {
			movingRight = true;
		} else {
			movingRight = false;
		}
		
		
		if (gameObject.transform.position.y < -1) {
			random = Random.Range (2, 6); //2 3 4 5
		} else {
			random = Random.Range (1, 6); //1 2 3 4 5
		}
		
		if (random == 1 || random == 2) {
			Jump (JumpVelocity);
		}
	}
	
	void Jump(float velocity)
	{
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(gameObject.GetComponent<Rigidbody2D>().velocity.x, velocity);
	}
}
