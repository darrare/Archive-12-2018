using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {
	Transform lineStart, lineEnd;
	GameObject healthBar;
	float health = 100;
	float maxHealth = 100;

	GameObject character;
	Stats stats;

	// Use this for initialization
	void Start () {
		character = GameObject.Find ("Character");
		stats = GameObject.Find ("Canvas").GetComponent<Stats> ();
		lineStart = transform.FindChild ("Start").gameObject.transform;
		lineEnd = transform.FindChild ("End").gameObject.transform;
		healthBar = new GameObject ("HealthBar");
		healthBar.AddComponent<SpriteRenderer> ();
		healthBar.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("HealthBarVisual", typeof(Sprite)) as Sprite;
		healthBar.transform.parent = this.transform;
		healthBar.transform.position = new Vector2 (transform.position.x, transform.position.y + .75f);
		InvokeRepeating ("FireWeapon", 0, .75f);
	}
	
	// Update is called once per frame
	void Update () {


		//if there is a character in front of this entity
		if (Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Player")) || Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Enemy"))) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, GetComponent<Rigidbody2D> ().velocity.y);
		} else if (Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Ground"))){
			GetComponent<Rigidbody2D> ().velocity = new Vector2(GetComponent<Rigidbody2D> ().velocity.x, 8);
		} else {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-2, GetComponent<Rigidbody2D> ().velocity.y);
		}
	}

	public void RemoveHealth(float amount)
	{
		health -= amount;
		healthBar.transform.localScale = new Vector3 (health / maxHealth, 1, 1);
		if (health <= 0) {
			stats.ChangeXP (10);
			stats.ChangeGold (25);
			GameObject gold = Instantiate (Resources.Load ("gold")) as GameObject;
			gold.transform.position = new Vector2(transform.position.x, transform.position.y + .3f);
			GameObject xp = Instantiate (Resources.Load ("xp")) as GameObject;
			xp.transform.position = new Vector2(transform.position.x, transform.position.y + .5f);
			Destroy (this.gameObject);
		}
	}

	void FireWeapon()
	{
		if (Vector2.Distance (character.transform.position, transform.position) < 10) {
			//attack player
			GameObject IceBall = Instantiate(Resources.Load("Iceball")) as GameObject;
			//IceBall.GetComponent<IceBolt>().SetTarget = "Player";
			IceBall.transform.position = transform.FindChild ("AttackSpawn").transform.position;
			Vector2 direction = new Vector2 (character.transform.position.x - IceBall.transform.position.x, character.transform.position.y - IceBall.transform.position.y);
			direction.Normalize ();
			IceBall.GetComponent<Rigidbody2D> ().velocity = direction * 10;
		}
	}
}
