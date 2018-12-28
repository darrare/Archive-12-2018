using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	_Image[] imageArray;

	Vector3 iVector;
	Vector3 iPlusOneVector;

	GameObject newObject;


	// Use this for initialization
	void Start () {

		Sprite[] sprites = Resources.LoadAll<Sprite> ("Sprites");
		imageArray = new _Image[sprites.Length];

		//creates a new array "imageArray" that stores all image objects with methods r, g, and b.
		for (int i = 0; i < imageArray.Length; i++)
		{
			imageArray[i] = new _Image(sprites[i]);
		}

		GameObject prefab = Resources.Load<GameObject> ("SpritePrefab");
		newObject = Instantiate (prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

		//float initialLocation = 0;
		//for (int i = 0; i < 386; i++){
		//	GameObject newObject0 = Instantiate (prefab, new Vector3(initialLocation, 0, 0), Quaternion.identity) as GameObject;
		//	newObject0.GetComponent<SpriteRenderer>().sprite = imageArray[i].sprite ();
		//	newObject0.name = imageArray[i].sprite().name;
		//	initialLocation += .64f;
		//}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("R=" + imageArray[0].r () + "; G=" + imageArray[0].g () + "; B=" + imageArray[0].b ());
			newObject.GetComponent<SpriteRenderer>().sprite = imageArray[0].sprite();
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			Debug.Log ("R=" + imageArray[1].r () + "; G=" + imageArray[1].g () + "; B=" + imageArray[1].b ());
			newObject.GetComponent<SpriteRenderer>().sprite = imageArray[1].sprite();
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			Debug.Log ("R=" + imageArray[2].r () + "; G=" + imageArray[2].g () + "; B=" + imageArray[2].b ());
			newObject.GetComponent<SpriteRenderer>().sprite = imageArray[2].sprite();
		}

		if (Input.GetKeyDown (KeyCode.F)) {
			_Image temp;
			for (int j = 0; j < imageArray.Length; j++)
			{
				for(int i = 0; i < imageArray.Length - 1; i++)
				{
					iVector = new Vector3(imageArray[i].r (), imageArray[i].g (), imageArray[i].b ());
					iPlusOneVector = new Vector3(imageArray[i + 1].r (), imageArray[i + 1].g (), imageArray[i + 1].b ());
					iVector.Normalize ();
					iPlusOneVector.Normalize ();

					if (((iVector.x * 200) + (iVector.y * 100) + (iVector.z * 10)) < ((iPlusOneVector.x * 200) + (iPlusOneVector.y * 100) + (iPlusOneVector.z * 10)))
					{
						temp = imageArray[i];
						imageArray[i] = imageArray[i + 1];
						imageArray[i + 1] = temp;
					}
					else if (((iVector.x * 10) + (iVector.y * 100) + (iVector.z * 200)) > ((iPlusOneVector.x * 10) + (iPlusOneVector.y * 100) + (iPlusOneVector.z * 200)))
					{
						temp = imageArray[i];
						imageArray[i] = imageArray[i + 1];
						imageArray[i + 1] = temp;
					}
				}
			}
			Debug.Log ("Done sorting");
		}

		if (Input.GetKeyDown (KeyCode.G)){
			float initialLocation = 0;
			GameObject prefab = Resources.Load <GameObject> ("SpritePrefab");
			for (int i = 0; i < imageArray.Length; i++){
				GameObject newObject = Instantiate (prefab, new Vector3(initialLocation, 0, 0), Quaternion.identity) as GameObject;
				newObject.GetComponent<SpriteRenderer>().sprite = imageArray[i].sprite();
				newObject.name = imageArray[i].sprite ().name;
				initialLocation += .64f;
			}
		}

		if (Input.GetKeyDown (KeyCode.H))
		{
			GameObject prefab = Resources.Load <GameObject> ("SpritePrefab");
			int x = 0, y = 0;
			for (int i = 0; i < imageArray.Length; i++){
				GameObject newObject = Instantiate (prefab, new Vector3(x * .80f, y * .80f, 0), Quaternion.identity) as GameObject;
				newObject.GetComponent<SpriteRenderer>().sprite = imageArray[i].sprite();
				newObject.name = imageArray[i].sprite ().name;
				if (y == 21)
				{
					y = 0;
					x++;
				}
				else 
					y++;
			}
		}
	}
}


//float initialLocation = 0;
//GameObject prefab = Resources.Load <GameObject> ("SpritePrefab");
//for (int i = 0; i < arrayOfImages.Length; i++){
//	GameObject newObject = Instantiate (prefab, new Vector3(initialLocation, 0, 0), Quaternion.identity) as GameObject;
//	newObject.GetComponent<SpriteRenderer>().sprite = arrayOfImages[i];
//	initialLocation += .64f;
//}