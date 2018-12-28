using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour {

	private CharacterController character;
	private RectTransform healthTransform;
	private float cachedY;
	private float minXValue;
	private float maxXValue;
	private float curHealth;
	private float maxHealth;
	public Image visualHealth;
	
	// Use this for initialization
	void Start () {
		character = GameObject.Find ("Character").GetComponent<CharacterController> ();
		healthTransform = GameObject.Find ("Health (1)").GetComponent<RectTransform> ();
		cachedY = healthTransform.position.y;
		maxXValue = healthTransform.position.x;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		maxHealth = character.MaxHealth ();
		curHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		ControlPosition ();
	}
	
	void ControlPosition()
	{
		curHealth = character.Health ();
		float percentHealth = curHealth / maxHealth;
		float distance = maxXValue - minXValue;
		float relativePosition = distance * percentHealth;
		float offset = distance - relativePosition;
		
		healthTransform.position = new Vector3 (maxXValue - offset, cachedY, 0);
	}
}