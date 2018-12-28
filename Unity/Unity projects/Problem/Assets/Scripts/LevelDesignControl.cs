using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelDesignControl : MonoBehaviour {
	enum GameState { Editing, Testing };
	GameState gameState = GameState.Testing;
	GameObject newObject;
	public Transform newObjectParent;
	Vector2 mouseLocation;
	bool isUsable = true;
	string prefabName;

	public GameObject testingObject, editingObject;
	GameObject referenceObj;
	public GameObject customObjects;

	public InputField posX, posY, rotZ;
	public InputField levelName;

	// Use this for initialization
	void Start () {
		ToggleGameState ();
	}
	
	// Update is called once per frame
	void Update () {
		//If the mouse is not in the control window rect, allow mouse control to be used
		//else if mouse is in the control window rect, change controls to menu control
		if (gameState == GameState.Editing) {
			mouseLocation = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            mouseLocation = new Vector2(Mathf.Round(mouseLocation.x), Mathf.Round(mouseLocation.y));
            if (newObject != null) {
                newObject.transform.position = mouseLocation;
				posX.text = newObject.transform.position.x.ToString ();
				posY.text = newObject.transform.position.y.ToString ();
				//rotZ.text = newObject.transform.rotation.z.ToString();
			}

			if (Input.GetMouseButton (0) && isUsable) {
				Collider2D[] col = Physics2D.OverlapPointAll (mouseLocation);
				int count = 0;
				foreach (Collider2D collider in col) {
					if (collider.name.Contains ("(Clone)")) {
						count++;
					}
				}
				if (count < 1 && newObject != null) {
					GameObject realObject = Instantiate (Resources.Load (prefabName), newObject.transform.position, Quaternion.identity) as GameObject;
					realObject.transform.eulerAngles = newObject.transform.eulerAngles;
					realObject.transform.SetParent (newObjectParent);
				} else {
					CloseCustomObjectsWindow ();
					referenceObj = null;
					foreach (Collider2D collider in col) {
						if (collider.gameObject != newObject && collider.name.Contains("(Clone)")) {
							HandleCustomObject (collider.gameObject);
							return;
						}
					}
				}
			} else if (Input.GetMouseButton (1) && isUsable) {
				Collider2D[] col = Physics2D.OverlapPointAll (mouseLocation);
				foreach (Collider2D collider in col) {
					if (collider.gameObject != newObject) {
						if (collider.transform.parent != newObjectParent) {
							Destroy (collider.transform.parent.gameObject);
						} else {
							Destroy (collider.gameObject);
						}
					}
				}
			} else if (Input.GetKeyDown (KeyCode.R) && isUsable) {
				float rotation = Convert.ToSingle (rotZ.text);
				rotation += 90;
				if (rotation >= 360) {
					rotation -= 360;
				}
				rotZ.text = rotation.ToString ();
				ChangeRotation ();
			}

		} else if (gameState == GameState.Testing) {

		}
	}

    void HandleCustomObject(GameObject obj)
    {
        if(!obj.name.Contains("(Clone)"))
        {
            return;
        }
        else if (obj.name == "rotatingLaserMount(Clone)")
        {
			referenceObj = obj;
			OpenCustomObjectsWindow ("RotatingLaser");
        }
        else if (obj.name == "turretMount(Clone)")
        {
			referenceObj = obj;
			OpenCustomObjectsWindow ("Turret");
        }
        else if (obj.name == "MovingPlatform(Clone)")
        {
			referenceObj = obj;
			OpenCustomObjectsWindow ("MovingPlatform");
        }

    }

	void OpenCustomObjectsWindow(string value)
	{
		customObjects.SetActive (true);
		if (value == "RotatingLaser") {
			customObjects.transform.FindChild ("RotatingLaser").gameObject.SetActive (true);
			customObjects.transform.FindChild ("RotatingLaser").FindChild ("RotatingLaserToggle").GetComponent<Toggle> ().isOn = referenceObj.GetComponent<RotatingLaser> ().IsClockwise;
		} else if (value == "Turret") {
			customObjects.transform.FindChild ("Turret").gameObject.SetActive (true);
			customObjects.transform.FindChild ("Turret").FindChild ("TurretFireRate").GetComponent<InputField> ().text = referenceObj.GetComponent<Turret> ().FireRate.ToString ();
		} else if (value == "MovingPlatform") {
			customObjects.transform.FindChild ("MovingPlatform").gameObject.SetActive (true);
			customObjects.transform.FindChild ("MovingPlatform").FindChild ("Distance").GetComponent<InputField> ().text = referenceObj.GetComponent<MovingPlatform> ().TravelDistance.ToString ();
			customObjects.transform.FindChild ("MovingPlatform").FindChild ("IsVerticalToggle").GetComponent<Toggle> ().isOn = referenceObj.GetComponent<MovingPlatform> ().IsVertical;
			customObjects.transform.FindChild ("MovingPlatform").FindChild ("StartNormalDirectionToggle").GetComponent<Toggle> ().isOn = referenceObj.GetComponent<MovingPlatform> ().NormDirection;
		}
		rotZ.text = referenceObj.transform.eulerAngles.z.ToString ();
	}

	void CloseCustomObjectsWindow()
	{
		customObjects.transform.FindChild ("MovingPlatform").gameObject.SetActive (false);
		customObjects.transform.FindChild ("Turret").gameObject.SetActive (false);
		customObjects.transform.FindChild ("RotatingLaser").gameObject.SetActive (false);
		customObjects.SetActive (false);
	}

	public void RotatingLaserClockwiseToggle(Toggle toggle)
	{
		referenceObj.GetComponent<RotatingLaser> ().SetClockwise (toggle.isOn);
	}

	public void TurretFireRateModify(InputField input)
	{
		referenceObj.GetComponent<Turret> ().FireRate = Convert.ToSingle (input.text);
	}

	public void MovingPlatformMoveDistanceModify(InputField input)
	{
		referenceObj.GetComponent<MovingPlatform> ().TravelDistance = Convert.ToSingle(input.text);
	}

	public void MovingPlatformIsVerticalToggle(Toggle toggle)
	{
		referenceObj.GetComponent<MovingPlatform> ().isVertical = toggle.isOn;
	}

	public void MovingPlatformIsNormDirectionToggle(Toggle toggle)
	{
		referenceObj.GetComponent<MovingPlatform> ().NormDirection = toggle.isOn;
	}

	public void AttachToCursor(string prefabName)
	{
		CloseCustomObjectsWindow ();
		this.prefabName = prefabName;
		Destroy (newObject);
		newObject = Instantiate(Resources.Load (prefabName)) as GameObject;
		newObject.name = "MouseObject";
		rotZ.text = "0";
	}


	public void SetUsable(bool value)
	{
		isUsable = value;
	}

	public void ChangeRotation()
	{
		if (referenceObj != null && newObject == null) {
			referenceObj.transform.eulerAngles = new Vector3 (0, 0, Convert.ToSingle(rotZ.text));
		} else {
			newObject.transform.eulerAngles = new Vector3 (0, 0, Convert.ToSingle(rotZ.text));
		}
	}

	public void ToggleGameState()
	{
		if (gameState == GameState.Editing) {
			testingObject.SetActive (true);
			editingObject.SetActive (false);
			gameState = GameState.Testing;
			Destroy (newObject);
			testingObject.transform.FindChild ("Character").transform.position = Vector2.zero;
            Time.timeScale = 1;
        } else {
			GameObject.Find ("Character").transform.SetParent (testingObject.transform);
			testingObject.SetActive (false);
			editingObject.SetActive (true);
			gameState = GameState.Editing;
            Time.timeScale = 0;
        }
	}

	public void BackToMainMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene ("MainMenu");
	}

	public void Save()
	{
		using (StreamWriter writer = new StreamWriter (Application.dataPath + "/" + levelName.text + ".txt")) {
			foreach (Transform obj in newObjectParent) {
				if (obj.name == "rotatingLaserMount(Clone)") {
					writer.WriteLine ("Name[" + obj.name + "] : PositionX[" + obj.position.x + "] : PositionY[" + obj.position.y + "] : RotationZ[" + obj.eulerAngles.z + "] : LaserRotation[" + (obj.GetComponent<RotatingLaser> ().CurrentRotation - obj.eulerAngles.z) + "] : Clockwise[" + obj.GetComponent<RotatingLaser> ().IsClockwise + "]");
				} else if (obj.name == "turretMount(Clone)") {
					writer.WriteLine ("Name[" + obj.name + "] : PositionX[" + obj.position.x + "] : PositionY[" + obj.position.y + "] : RotationZ[" + obj.eulerAngles.z + "] : FireRate[" + obj.GetComponent<Turret> ().FireRate + "]");
				} else if (obj.name == "MovingPlatform(Clone)") {
					writer.WriteLine ("Name[" + obj.name + "] : PositionX[" + obj.position.x + "] : PositionY[" + obj.position.y + "] : RotationZ[" + obj.eulerAngles.z + "] : TravelDistance[" + obj.GetComponent<MovingPlatform>().TravelDistance + "] : IsVertical[" + obj.GetComponent<MovingPlatform> ().IsVertical + "] : NormDirection[" + obj.GetComponent<MovingPlatform> ().NormDirection + "]");
				} else {
					writer.WriteLine ("Name[" + obj.name + "] : PositionX[" + obj.position.x + "] : PositionY[" + obj.position.y + "] : RotationZ[" + obj.eulerAngles.z + "]");
				}

			}
		}
	}

	public void Load()
	{
		string line;
		string prefabName, posX, posY, rotZ;
		string[] slices;
		char[] delimiterChars = { '[' , ']' };

		//Clear the scene before adding all the new objects
		foreach (Transform obj in newObjectParent) {
			Destroy (obj.gameObject);
		}
		using (StreamReader reader = new StreamReader (Application.dataPath + "/" + levelName.text + ".txt")) {
			while ((line = reader.ReadLine ()) != null) {
				slices = line.Split (delimiterChars);
				prefabName = slices [1];
				prefabName = prefabName.Split (new Char[] { '(' })[0];
				posX = slices [3];
				posY = slices [5];
				rotZ = slices [7];
				GameObject newObject = Instantiate (Resources.Load (prefabName), new Vector3 (Convert.ToSingle (posX), Convert.ToSingle (posY), 0), Quaternion.identity) as GameObject;
				if (prefabName == "rotatingLaserMount") {
					newObject.GetComponent<RotatingLaser> ().CurrentRotation = Convert.ToSingle (slices [9]);
					newObject.GetComponent<RotatingLaser> ().IsClockwise = Convert.ToBoolean (slices [11]);
				} else if (prefabName == "turretMount") {
					newObject.GetComponent<Turret> ().FireRate = Convert.ToSingle (slices [9]);
				} else if (prefabName == "MovingPlatform") {
					newObject.GetComponent<MovingPlatform> ().travelDistance = Convert.ToSingle (slices [9]);
					newObject.GetComponent<MovingPlatform> ().IsVertical = Convert.ToBoolean (slices [11]);
					newObject.GetComponent<MovingPlatform> ().NormDirection = Convert.ToBoolean (slices [13]);
				}
				newObject.transform.eulerAngles = new Vector3 (0, 0, Convert.ToSingle (rotZ));
				newObject.transform.SetParent (newObjectParent);
			}
		}
	}
}
	