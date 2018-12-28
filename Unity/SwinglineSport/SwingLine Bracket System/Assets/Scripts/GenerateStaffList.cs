using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenerateStaffList : MonoBehaviour {
	List<GameObject> staffButtons = new List<GameObject>();
	float startX, startY, offsetY;
	float width = 800;
	float height = 600;
	public InputField[] inputFields = new InputField[3];

	// Use this for initialization
	void Awake()
	{
		GameObject.Find ("NetManager").GetComponent<NetManager> ().ResetStaffList ();
		startX = -(width / 2) + (width / 8);
		startY = (height / 2) - (height / 6);
		offsetY = height / 20;
	}

	public void DrawStaffMember(StaffClient staff)
	{
		GameObject newObject = Instantiate (Resources.Load ("Staff")) as GameObject;
		newObject.transform.SetParent (GameObject.Find ("StaffList").transform);
		newObject.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		newObject.name = staff.username;
		newObject.transform.GetComponentInChildren<Text> ().text = staff.username;
		staffButtons.Add (newObject);
		DrawAllStaffMembers ();
	}

	void DrawAllStaffMembers()
	{
		for (int i = 0; i < staffButtons.Count; i++) {
			staffButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(startX, startY - offsetY * i, 0);
		}
	}

	public void UpdateList()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void SendCommandClick()
	{
		string name = GameObject.Find ("StaffNameText").GetComponent<Text> ().text;
		int connectionId = 0;
		for (int i = NetManager.staffClients.Count - 1; i >= 0; i--) {
			if (NetManager.staffClients[i].username == name)
			{
				connectionId = NetManager.staffClients[i].connectionId;
				NetManager.staffClients.RemoveAt (i);
			}
		}
		GameObject.Find ("NetManager").GetComponent<NetManager> ().SendInstructionToStaff (
			connectionId, inputFields[0].text, inputFields[1].text, inputFields[2].text);
		foreach (InputField text in inputFields) {
			text.text = "";
		}
		Destroy (GameObject.Find (name));
		GameObject.Find ("StaffNameText").GetComponent<Text> ().text = "";
	}
}
