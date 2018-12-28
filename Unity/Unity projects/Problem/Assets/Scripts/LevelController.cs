using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public GameObject player;
	public AudioClip bgm;
	public CameraControl cameraControl;
	Transform respawnPoint;
	List<RespawnableItem> itemsToRespawn = new List<RespawnableItem> ();

	Text timer;
	float time = 0;
	string minutes;
	string seconds;
	string milliseconds;

	float input;
	bool readyToMove = true;
	bool isTop = true;
	public GameObject pauseMenu, highLightPanel, resumeButton, mainMenuButton;
	bool isUp = false;

	// Use this for initialization
	void Awake () {
		CONSTANTS.levelController = this.GetComponent<LevelController> ();
		timer = transform.FindChild ("TimerPanel").GetChild (0).GetComponent<Text> ();
		if (bgm != null)
			CONSTANTS.backgroundMusic.PlayOneShot (bgm);
	}

	void Start()
	{
		SetRespawnPoint (player.transform);
	}
	
	// Update is called once per frame
	void Update () {
		//time control
		time += Time.deltaTime;
		time = Mathf.Round (time * 100f) / 100f;
		seconds = ((int)(time % 60)).ToString ();
		minutes = ((int)(time / 60)).ToString ();
		milliseconds = (time - (int)time).ToString (".##");
		timer.text = minutes + ":" + seconds + "" + milliseconds;
		//end time control

		if (InputManager.GetButtonDown("Start")) {
			TogglePause ();
			isUp = false;
		}
		else if (Time.timeScale == 0) {
			input = InputManager.GetAxisRaw ("Vertical");
			if (input == 0) {
				readyToMove = true;
			} else if (input == 1 && readyToMove) {
				readyToMove = false;
				if (!isTop) {
					isTop = !isTop;
					highLightPanel.GetComponent<RectTransform> ().anchoredPosition = resumeButton.GetComponent<RectTransform> ().anchoredPosition;
				}
			} else if (input == -1 && readyToMove) {
				readyToMove = false;
				if (isTop) {
					isTop = !isTop;
					highLightPanel.GetComponent<RectTransform> ().anchoredPosition = mainMenuButton.GetComponent<RectTransform> ().anchoredPosition;
				}
			}
			if (InputManager.GetButtonUp ("Start")) {
				isUp = true;
			}
			else if (InputManager.GetButtonDown ("Jump") && isUp || InputManager.GetButtonDown("Start") && isUp) {
				if (isTop) {
					TogglePause ();
				} else {
					BackToMainMenu ();
				}
			}
		}
	}

	public void TogglePause()
	{
		if (Time.timeScale == 0) {
			Time.timeScale = 1;
			pauseMenu.SetActive (false);
		} else {
			Time.timeScale = 0;
			pauseMenu.SetActive (true);
		}
	}

	public void BackToMainMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene ("MainMenu");
	}

	public float GetTime
	{
		get { return time; }
	}

	public GameObject Player
	{
		get { return player; }
	}

	public void AddItemToRespawnList(string name, Vector2 position, Quaternion rotation)
	{
		RespawnableItem obj = new RespawnableItem (name, position, rotation);
		itemsToRespawn.Add (obj);
	}

	public void SetRespawnPoint(Transform respawnPoint)
	{
		this.respawnPoint = respawnPoint;
	}

	public void StartRespawnTimer()
	{
		cameraControl.PauseCamera ();
		Destroy (player);
		Invoke ("RespawnPlayer", 1);
	}

	void RespawnPlayer()
	{
		player = Instantiate (Resources.Load ("Character")) as GameObject;
		player.name = "Character";
		if (respawnPoint != null) {
			player.transform.position = respawnPoint.position + Vector3.up * .5f;
		} else {
			player.transform.position = Vector2.zero;
		}

		cameraControl.UnPauseCamera (player);
		foreach (RespawnableItem obj in itemsToRespawn) {
			obj.Load ();
		}
		itemsToRespawn.Clear ();
	}

	struct RespawnableItem
	{
		string name;
		Vector2 position;
		Quaternion rotation;

		public RespawnableItem(string name, Vector2 position, Quaternion rotation)
		{
			this.name = name;
			this.position = position;
			this.rotation = rotation;
		}

		public void Load()
		{
			Instantiate (Resources.Load (name), position, rotation);
		}
	}
}
