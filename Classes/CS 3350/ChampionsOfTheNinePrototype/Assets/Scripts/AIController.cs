using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

	Transform lineStart, lineEnd;
	GameObject healthBar;
	float health = 250;
	float maxHealth = 250;
	
	GameObject character;
	Stats stats;

	Animator animateState;
	
	// Use this for initialization
	void Start () {
		animateState = transform.gameObject.GetComponent<Animator> ();
		character = GameObject.Find ("Character");
		stats = GameObject.Find ("Canvas").GetComponent<Stats> ();
		lineStart = transform.FindChild ("Start").gameObject.transform;
		lineEnd = transform.FindChild ("End").gameObject.transform;
		healthBar = new GameObject ("HealthBar");
		healthBar.AddComponent<SpriteRenderer> ();
		healthBar.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("HealthBarVisual", typeof(Sprite)) as Sprite;
		healthBar.transform.parent = this.transform;
		healthBar.transform.position = new Vector2 (transform.position.x, transform.position.y + .75f);


		if (transform.FindChild ("AttackSpawn") != null) {
			//this is a caster
			health = 100;
			maxHealth = 100;
			InvokeRepeating ("FireWeapon", 0, .75f);
		} else { //this is a knight
			InvokeRepeating ("Attack", 0, .75f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//if there is a character in front of this entity
		if (Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Player")) || Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Enemy"))) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, GetComponent<Rigidbody2D> ().velocity.y);
		} else if (Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Ground"))) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, 8);
		} else if (transform.tag == "Enemy") {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-2, GetComponent<Rigidbody2D> ().velocity.y);
		} else {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (2, GetComponent<Rigidbody2D> ().velocity.y);
		}
	}

	public void RemoveHealth(float amount)
	{
		health -= amount;
		healthBar.transform.localScale = new Vector3 (health / maxHealth, 1, 1);
		if (health <= 0 && transform.tag == "Enemy") {
			stats.ChangeXP (10);
			stats.ChangeGold (25);
			GameObject gold = Instantiate (Resources.Load ("gold")) as GameObject;
			gold.transform.position = new Vector2 (transform.position.x, transform.position.y + .3f);
			GameObject xp = Instantiate (Resources.Load ("xp")) as GameObject;
			xp.transform.position = new Vector2 (transform.position.x, transform.position.y + .5f);
			Destroy (this.gameObject);
		} else if (health <= 0) {
			Destroy (this.gameObject);
		}
	}

	void Attack()
	{
		RaycastHit2D hit = Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Player"));
		if (transform.tag == "Enemy" && Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Player")) && hit.collider.transform.name == "Character") {
			hit.collider.transform.gameObject.GetComponent<CharacterController>().RemoveHealth(20);
			animateState.SetBool ("isAttacking", true);
		}
		else if (transform.tag == "Enemy" && Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Player"))) {
			hit.collider.transform.gameObject.GetComponent<AIController>().RemoveHealth(20);
			animateState.SetBool ("isAttacking", true);
		} else if (transform.tag == "Player" && Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Enemy"))) {
			hit = Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Enemy"));
			hit.collider.transform.gameObject.GetComponent<AIController>().RemoveHealth(20);
			animateState.SetBool ("isAttacking", true);
		} else {
			animateState.SetBool ("isAttacking", false);
		}
	}

	void FireWeapon()
	{
		if (transform.tag == "Enemy") {
			float distance = 100;
			Transform target = transform;
			bool targetFound = false;
			if (Vector2.Distance (this.transform.position, character.transform.position) < 10)
			{
				target = character.transform;
				targetFound = true;
			}
			if (!targetFound)
			{
				for(int i = 0; i < GameObject.Find ("AllySpawnLocation").transform.childCount; i++)
				{
					if (Vector2.Distance (this.transform.position, GameObject.Find ("AllySpawnLocation").transform.GetChild(i).transform.position) < 10) {
						if (Vector2.Distance (this.transform.position, GameObject.Find ("AllySpawnLocation").transform.GetChild(i).transform.position) < distance)
						{
							distance = Vector2.Distance (this.transform.position, GameObject.Find ("AllySpawnLocation").transform.GetChild(i).transform.position);
							target = GameObject.Find ("AllySpawnLocation").transform.GetChild(i).transform;
							targetFound = true;
						}
					}
				}
			}
			if (targetFound)
			{
				GameObject IceBall = Instantiate(Resources.Load("Iceball")) as GameObject;
				IceBall.GetComponent<IceBolt>().SetTarget = target;
				IceBall.transform.position = transform.FindChild ("AttackSpawn").transform.position;
				Vector2 direction = new Vector2 (target.position.x - IceBall.transform.position.x, target.position.y - IceBall.transform.position.y);
				direction.Normalize ();
				IceBall.GetComponent<Rigidbody2D> ().velocity = direction * 10;
			}
		}
		else if (transform.tag == "Player") {
			float distance = 100;
			Transform target = transform;
			bool targetFound = false;
			for(int i = 0; i < GameObject.Find ("EnemySpawnLocation").transform.childCount; i++)
			{
				if (Vector2.Distance (this.transform.position, GameObject.Find ("EnemySpawnLocation").transform.GetChild(i).transform.position) < 10) {
					if (Vector2.Distance (this.transform.position, GameObject.Find ("EnemySpawnLocation").transform.GetChild(i).transform.position) < distance)
					{
						distance = Vector2.Distance (this.transform.position, GameObject.Find ("EnemySpawnLocation").transform.GetChild(i).transform.position);
						target = GameObject.Find ("EnemySpawnLocation").transform.GetChild(i).transform;
						targetFound = true;
					}
				}
			}
			if (targetFound)
			{
				GameObject IceBall = Instantiate(Resources.Load("Iceball")) as GameObject;
				IceBall.GetComponent<IceBolt>().SetTarget = target;
				IceBall.transform.position = transform.FindChild ("AttackSpawn").transform.position;
				Vector2 direction = new Vector2 (target.position.x - IceBall.transform.position.x, target.position.y - IceBall.transform.position.y);
				direction.Normalize ();
				IceBall.GetComponent<Rigidbody2D> ().velocity = direction * 10;
			}
		}
//		if (Vector2.Distance (character.transform.position, transform.position) < 10) {
//			//attack player
//			GameObject IceBall = Instantiate(Resources.Load("Iceball")) as GameObject;
//			IceBall.GetComponent<IceBolt>().SetTarget = "Player";
//			IceBall.transform.position = transform.FindChild ("AttackSpawn").transform.position;
//			Vector2 direction = new Vector2 (character.transform.position.x - IceBall.transform.position.x, character.transform.position.y - IceBall.transform.position.y);
//			direction.Normalize ();
//			IceBall.GetComponent<Rigidbody2D> ().velocity = direction * 10;
//		}
	}
}
