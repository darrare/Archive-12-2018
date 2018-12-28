using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Stats : MonoBehaviour {
	int gold = 1000;
	int xp = 0;
	
	public int GetGold
	{
		get { return gold; }
	}

	// Update is called once per frame
	void Update () {
		this.transform.FindChild ("Panel (3)").FindChild ("Text").FindChild ("Text").GetComponent<Text> ().text = "Gold: " + gold.ToString ();
		this.transform.FindChild ("Panel (3)").FindChild ("Text").FindChild ("Text (1)").GetComponent<Text> ().text = "XP: " + xp.ToString ();
	}

	public void ChangeGold(int amount)
	{
		gold += amount;
	}

	public void ChangeXP(int amount)
	{
		xp += amount;
	}
}
