using UnityEngine;
using System.Collections;

public class EnemyCastle : MonoBehaviour {
	GameObject healthBar;
	float health = 2000;
	float maxHealth = 2000;
	GameObject youWonMessage;

	// Use this for initialization
	void Start () {
		youWonMessage = GameObject.Find ("RawImage (7)");
		youWonMessage.SetActive (false);
		healthBar = new GameObject ("HealthBar");
		healthBar.AddComponent<SpriteRenderer> ();
		healthBar.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("HealthBarVisual", typeof(Sprite)) as Sprite;
		healthBar.transform.parent = this.transform;
		healthBar.transform.position = new Vector2 (transform.position.x, transform.position.y + .75f);
		healthBar.transform.localScale = new Vector3 ((health / maxHealth) * 8, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {

		if (health <= 0) {
			youWonMessage.SetActive (true);
			Destroy (GameObject.Find ("EnemySpawnLocation"));
			Destroy (this.gameObject);
		}
	}

	public void RemoveHealth()
	{
		health -= 250;
		healthBar.transform.localScale = new Vector3 ((health / maxHealth) * 8, 1, 1);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.transform.name == "Meteor(Clone)")
		{
			RemoveHealth ();
//			health -= 250;
//			healthBar.transform.localScale = new Vector3 (health / maxHealth, 1, 1);
		}
	}
}
