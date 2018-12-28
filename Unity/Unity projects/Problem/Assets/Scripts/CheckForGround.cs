using UnityEngine;
using System.Collections;

public class CheckForGround : MonoBehaviour {

	public PlayerController player;
	public LayerMask whatIsGround;

	void OnTriggerEnter2D(Collider2D collider)
	{
		player.GroundTransform = collider.transform;
		if (GetComponent<BoxCollider2D> ().IsTouchingLayers (whatIsGround)) {
			player.IsGrounded = true;
		} 
	}
	void OnTriggerStay2D(Collider2D collider)
	{
		player.GroundTransform = collider.transform;
		if (GetComponent<BoxCollider2D> ().IsTouchingLayers (whatIsGround)) {
			player.IsGrounded = true;
		} 
	}
	public void OnTriggerExit2D(Collider2D collider)
	{
		player.GroundTransform = null;
		if (!GetComponent<BoxCollider2D> ().IsTouchingLayers (whatIsGround)) {
			player.IsGrounded = false;
		} 
	}
}
