using UnityEngine;
using System.Collections;

public class BossLevelController : MonoBehaviour {
	
	public BossControl bossControl;
	GameObject player;
	bool isReset = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null && !isReset) {
			bossControl.ResetBoss ();
			isReset = true;
		} 
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			player = collider.transform.gameObject;
			isReset = false;
			bossControl.PlayerEnteredRoom (player);
		}
	}
}
