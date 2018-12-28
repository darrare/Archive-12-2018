using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TNT : MonoBehaviour {
	public float timeBeforeDetonation;
	public Text timerText;
	public Transform counterRotation;
	float timer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		counterRotation.localEulerAngles = new Vector3 (0, 0, -transform.eulerAngles.z);
		timer += Time.deltaTime;
		if (timer >= timeBeforeDetonation) {
			//explode
			GameObject newExplosion = Instantiate(Resources.Load("KillerExplosion"), this.transform.position, Quaternion.identity) as GameObject;
			newExplosion.transform.localScale = new Vector3 (3, 3, 3);
			Destroy(this.gameObject);
		}

		timerText.text = (timeBeforeDetonation - (int)timer - 1).ToString();
	}
}
