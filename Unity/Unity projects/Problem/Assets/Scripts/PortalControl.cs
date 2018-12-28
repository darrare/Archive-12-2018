using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PortalControl : MonoBehaviour {

	public string levelName;
	public bool saveLocation = false;
	public bool isUseable = true;
	float timer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!isUseable) {
			timer += Time.deltaTime;
			if (timer > 1) {
				isUseable = true;
			}
		}

	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player" && InputManager.GetAxisRaw("Vertical") == 1 && isUseable) {
			if (saveLocation) {
				CONSTANTS.levelSelectSpawnPosition = new Vector3 (transform.position.x, transform.position.y + 1, 0);
			} else {
				if (CONSTANTS.levelInfo.ContainsKey (SceneManager.GetActiveScene().name)) {
					CONSTANTS.levelInfo [SceneManager.GetActiveScene ().name].SaveNewTime (CONSTANTS.levelController.GetTime);
				} else {
					CONSTANTS.levelInfo.Add (SceneManager.GetActiveScene().name, new LevelSaveInfo(CONSTANTS.levelController.GetTime));
				}
			}
			SaveLoad.Save ();
			SceneManager.LoadScene (levelName);
		}
	}
}
