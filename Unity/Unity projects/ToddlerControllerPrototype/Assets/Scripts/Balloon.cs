using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class Balloon : MonoBehaviour {
	Rigidbody2D rigidBody;
	float control = 0;
	float random;
	int randomValue;

	Vector3 screenPoint;
	bool onScreen = true;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		random = Random.Range (1.5f, 2.5f);
	}
	
	// Update is called once per frame
	void Update () {
		control = Mathf.PingPong (Time.time / random, 1);
		rigidBody.velocity = new Vector2 (control * 2 - 1, random);
		transform.eulerAngles = new Vector3 (0, 0, -control * 10 + 5);
		screenPoint = Camera.main.WorldToViewportPoint (transform.position);
		onScreen = screenPoint.y < 1.1f;
		if (!onScreen) {
			CONSTANTS.balloonSpawner.RemoveFromQueue (randomValue);
			Destroy (this.gameObject);
		}
	}

	void OnMouseDown()
	{
		int choice = Random.Range (0, 3);
		GameObject newSplat = new GameObject();
		switch (choice) {
		case 0:
			newSplat = Instantiate (Resources.Load ("0")) as GameObject;
			break;
		case 1:
			newSplat = Instantiate (Resources.Load ("1")) as GameObject;
			break;
		case 2:
			newSplat = Instantiate (Resources.Load ("2")) as GameObject;
			break;
		}
		newSplat.transform.position = this.transform.position;
		newSplat.GetComponent<PaintSplat> ().SetColor (GetComponent<SpriteRenderer> ().color);
		Destroy (this.gameObject);
	}

	public void DestroyBalloon()
	{
		OnMouseDown ();
	}

	public void SetBalloon(Color color, Sprite sprite, int randomValue)
	{
		this.randomValue = randomValue;
		//GetComponent<SpriteRenderer> ().color = CONSTANTS.colors [Random.Range (0, CONSTANTS.colors.Length)];
		GetComponent<SpriteRenderer> ().color = color;
		transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = sprite;
	}
}
