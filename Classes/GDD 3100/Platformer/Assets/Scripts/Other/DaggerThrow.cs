using UnityEngine;
using System.Collections;

public class DaggerThrow : MonoBehaviour {
	Transform top, middle, bottom; 
	Transform target;
	RaycastHit2D hitInformation;

	float speed = 5;

	// Use this for initialization
	void Start () {
		target = transform.GetChild (1).transform;
		transform.localScale = new Vector2 (1, GameObject.Find ("Character").transform.localScale.x);
		transform.position = GameObject.Find ("Character").transform.Find ("KunaiSpawnLocation").transform.position;
		top = transform.GetChild (0).transform;
		middle = transform.GetChild (1).transform;
		bottom = transform.GetChild (2).transform;
		FindTarget ();
		OrientSelf ();
		SetVelocity ();
		Invoke ("KillSwitch", .75f);
	}

	void Update()
	{
		SetVelocity ();
	}

	void FindTarget()
	{
		if (Physics2D.Linecast (transform.position, middle.position, 1 << LayerMask.NameToLayer ("Enemy"))) {
			hitInformation = Physics2D.Linecast (transform.position, middle.position, 1 << LayerMask.NameToLayer ("Enemy"));
			target = hitInformation.transform;
		} else if (Physics2D.Linecast (transform.position, top.position, 1 << LayerMask.NameToLayer ("Enemy"))) {
			hitInformation = Physics2D.Linecast (transform.position, top.position, 1 << LayerMask.NameToLayer ("Enemy"));
			target = hitInformation.transform;
		} else if (Physics2D.Linecast (transform.position, bottom.position, 1 << LayerMask.NameToLayer ("Enemy"))) {
			hitInformation = Physics2D.Linecast (transform.position, bottom.position, 1 << LayerMask.NameToLayer ("Enemy"));
			target = hitInformation.transform;
		} else {
			target.position = middle.position;
		}
	}

	void OrientSelf()
	{
		float rotation = Mathf.Atan2 ((target.position.y - transform.position.y), (target.position.x - transform.position.x)) * Mathf.Rad2Deg;
		transform.Rotate (new Vector3 (0, 0, rotation));
		if (transform.localScale.y == -1) {
			transform.Rotate (new Vector3(0, 0, 180));
		}
	}

	void SetVelocity()
	{
		Vector2 direction = target.position - transform.position;
		direction.Normalize ();
		GetComponent<Rigidbody2D> ().velocity = direction * speed;
	}

	void KillSwitch()
	{
		Destroy (gameObject);
	}
}
