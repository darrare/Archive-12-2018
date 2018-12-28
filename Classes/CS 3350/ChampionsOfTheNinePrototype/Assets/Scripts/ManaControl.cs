using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManaControl : MonoBehaviour {

	private CharacterController character;
	private RectTransform manaTransform;
	private float cachedY;
	private float minXValue;
	private float maxXValue;
	private float curMana;
	private float maxMana;
	public Image visualMana;

	// Use this for initialization
	void Start () {
		character = GameObject.Find ("Character").GetComponent<CharacterController> ();
		manaTransform = GameObject.Find ("Mana (1)").GetComponent<RectTransform> ();
		cachedY = manaTransform.position.y;
		maxXValue = manaTransform.position.x;
		minXValue = manaTransform.position.x - manaTransform.rect.width;
		maxMana = character.MaxMana ();
		curMana = maxMana;
	}
	
	// Update is called once per frame
	void Update () {
		ControlPosition ();
	}

	void ControlPosition()
	{
		curMana = character.Mana ();
		float percentMana = curMana / maxMana;
		float distance = maxXValue - minXValue;
		float relativePosition = distance * percentMana;
		float offset = distance - relativePosition;

		manaTransform.position = new Vector3 (maxXValue - offset, cachedY, 0);
	}
}
