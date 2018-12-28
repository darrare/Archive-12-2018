using UnityEngine;
using System.Collections;

public class WeaponControl : MonoBehaviour {
	// THESE ARE THE TUNING VARIABLES FOR THE WEAPONS
	private static float iceManaCost = 10;
	private static float fireManaCost = 100;
	private static float lightningManaCost = 30;
	private static float iceDamage = 15;
	private static float fireDamage = 250;
	private static float lightningDamage = 15;
	
	private enum WeaponChoice{Ice, Fire, Lightning};
	private WeaponChoice weaponChoice = WeaponChoice.Ice;
	public Sprite iceIcon;
	public Sprite fireIcon;
	public Sprite lightningIcon;
	private SpriteRenderer iconLocation;
	private CharacterController character;

	// Use this for initialization
	void Start () {
		iconLocation = this.transform.GetChild(1).GetComponent <SpriteRenderer> ();
		character = GameObject.Find ("Character").GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			iconLocation.sprite = iceIcon;
			weaponChoice = WeaponChoice.Ice;
		}
		else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			iconLocation.sprite = fireIcon;
			weaponChoice = WeaponChoice.Fire;
		}
		else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			iconLocation.sprite = lightningIcon;
			weaponChoice = WeaponChoice.Lightning;
		}

		if (Input.GetMouseButtonDown(0))
		{
			FireWeapon();
		}
		
	}

	public static float IceDamage
	{
		get {return iceDamage;}
	}
	public static float FireDamage
	{
		get {return fireDamage;}
	}
	public static float LightningDamage
	{
		get {return lightningDamage;}
	}

	void FireWeapon()
	{
		if (weaponChoice == WeaponChoice.Ice) {
			if (character.Mana () > iceManaCost)
			{
				IceAttack();
				character.RemoveMana (iceManaCost);
			}
		}
		else if (weaponChoice == WeaponChoice.Fire) {
			if (character.Mana () > fireManaCost)
			{
				FireAttack();
				character.RemoveMana (fireManaCost);
			}
		}
		else if (weaponChoice == WeaponChoice.Lightning) {
			if (character.Mana () > lightningManaCost)
			{
				LightningAttack();
				character.RemoveMana (lightningManaCost);
			}
		}
	}

	void IceAttack()
	{
		Vector3 cursorInWorldPos3 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 cursorInWorldPos = new Vector2 (cursorInWorldPos3.x, cursorInWorldPos3.y);
		GameObject IceBall = Instantiate(Resources.Load("Iceball")) as GameObject;
		IceBall.transform.position = transform.GetChild(2).transform.position;
		Vector2 direction = new Vector2 (cursorInWorldPos.x - transform.GetChild (2).position.x, cursorInWorldPos.y - transform.GetChild (2).position.y);
		direction.Normalize ();
		IceBall.GetComponent<Rigidbody2D> ().velocity = direction * 10;
	}

	void FireAttack()
	{
		Vector3 cursorInWorldPos3 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		GameObject meteor = Instantiate (Resources.Load ("Meteor")) as GameObject;
		meteor.transform.position = new Vector3 (cursorInWorldPos3.x - 10, cursorInWorldPos3.y + 20, 0);
		Vector2 direction = new Vector2 (cursorInWorldPos3.x - meteor.transform.position.x, cursorInWorldPos3.y - meteor.transform.position.y);
		direction.Normalize ();
		meteor.GetComponent<Rigidbody2D> ().velocity = direction * 20;
	}

	void LightningAttack()
	{
		Vector3 cursorInWorldPos3 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 cursorInWorldPos = new Vector2 (cursorInWorldPos3.x, cursorInWorldPos3.y);
		GameObject lightning = Instantiate(Resources.Load("Lightning")) as GameObject;
		lightning.transform.parent = GameObject.Find ("Character").transform;
		lightning.transform.position = new Vector3 (lightning.transform.parent.position.x + 4.5f,lightning.transform.parent.position.y, 0);
		Vector2 direction = new Vector2 (cursorInWorldPos.x - transform.GetChild (2).position.x, cursorInWorldPos.y - transform.GetChild (2).position.y);
		direction.Normalize ();

		lightning.transform.RotateAround (transform.GetChild (2).transform.position, Vector3.forward, Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);
	}
}
