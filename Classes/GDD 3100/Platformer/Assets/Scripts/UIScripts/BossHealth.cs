using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {
	static GameObject bossHealth;
	static Text bossName;
	static RectTransform healthTransform;
	static float maxHealth;
	static float curHealth;
	static float cachedY;
	static float minXValue;
	static float maxXValue;

	// Use this for initialization
	void Start () {
		bossHealth = GameObject.Find ("BossHealth");
		bossName = GameObject.Find ("BossName").GetComponent<Text> ();
		healthTransform = GameObject.Find ("HealthBarRed").GetComponent<RectTransform> ();
		cachedY = healthTransform.position.y;
		maxXValue = healthTransform.position.x;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		bossHealth.SetActive (false);
	}

	public static void EnableBossBar(string bossTitle, float health)
	{
		bossHealth.SetActive (true);
		bossName.text = bossTitle;
		maxHealth = health;
		curHealth = maxHealth;
		bossName.text = bossTitle;
		DamageDealt (0);
	}

	public static void DamageDealt(float amount)
	{
		curHealth -= amount;
		if (curHealth <= 0) {
			bossHealth.SetActive (false);
		}
		float percentHealth = curHealth / maxHealth;
		float distance = maxXValue - minXValue;
		float relativePosition = distance * percentHealth;
		float offset = distance - relativePosition;
		
		healthTransform.position = new Vector3 (maxXValue - offset, cachedY, 0);
	}
}
