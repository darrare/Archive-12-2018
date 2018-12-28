using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControlledObject : MonoBehaviour {
	float maxHealth = 500;
	protected float curHealth;
	protected Text hpText;
	protected string baseText;

	// Use this for initialization
	protected virtual void Start () {
		curHealth = maxHealth;
		hpText = GameObject.Find ("EmptyText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		hpText.text = baseText + " " + curHealth;

		if (curHealth <= 0) {
			//destroy and remake player
		}
	}

	public void TakeDamage(float amount)
	{
		curHealth -= amount;
	}
}
