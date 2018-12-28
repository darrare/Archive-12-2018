using UnityEngine;
using System.Collections;
using LgOctEngine.CoreClasses;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

	public void LevelButtonClick()
	{
		foreach (string str in NetManager.levels) {
			//JsonMessage<Level> jsonMessage = str;
			Level obj = LgJsonNode.CreateFromJsonString<Level>(str);
			if (obj.LevelName == gameObject.name)
			{
				GameObject.Find ("Levels").GetComponent<Text>().text = obj.LevelObjectArray.Serialize ();
			}
		}
	}
}
