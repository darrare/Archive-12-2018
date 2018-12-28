using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Stats : MonoBehaviour {
	Character character;
	RectTransform healthBar;
	RectTransform oxygen;
	RectTransform battery;

	float maxResource = 1000;

	float healthMaxX;
	float oxygenMaxX;
	float batteryMaxX;

	// Use this for initialization
	void Start () {
		character = GameObject.Find ("Character").GetComponent<Character> ();
		healthBar = GameObject.Find ("red").GetComponent<RectTransform> ();
		oxygen = GameObject.Find ("blue").GetComponent<RectTransform> ();
		battery = GameObject.Find ("yellow").GetComponent<RectTransform> ();

		healthMaxX = healthBar.position.x;
		oxygenMaxX = oxygen.position.x;
		batteryMaxX = battery.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (character.Battery > 1000) {
			character.Battery = 1000;
		}
		if (character.Oxygen > 1000) {
			character.Oxygen = 1000;
		}
		float percentHealth = character.Health / maxResource;
		float percentOxygen = character.Oxygen / maxResource;
		float percentBattery = character.Battery / maxResource;

		float healthDistance = healthBar.position.x - (healthBar.position.x - healthBar.rect.width);
		float oxygenDistance = oxygen.position.x - (oxygen.position.x - oxygen.rect.width);
		float batteryDistance = battery.position.x - (battery.position.x - battery.rect.width);

		float relativeHealth = healthDistance * percentHealth;
		float relativeOxygen = oxygenDistance * percentOxygen;
		float relativeBattery = batteryDistance * percentBattery;

		float healthOffset = healthDistance - relativeHealth;
		float oxygenOffset = oxygenDistance - relativeOxygen;
		float batteryOffset = batteryDistance - relativeBattery;

		healthBar.transform.position = new Vector3 (healthMaxX - healthOffset, healthBar.position.y, 0);
		oxygen.transform.position = new Vector3 (oxygenMaxX - oxygenOffset, oxygen.position.y, 0);
		battery.transform.position = new Vector3 (batteryMaxX - batteryOffset, battery.position.y, 0);
	}
}
