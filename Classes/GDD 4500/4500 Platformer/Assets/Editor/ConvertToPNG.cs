using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class ConvertToPNG : ScriptableWizard {
	public GameObject objectToConvert;
	public string fileName = "TileMap";

	[MenuItem("GameObject/Pokemon Engine/Convert to PNG")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard ("Convert to PNG", typeof(ConvertToPNG));
	}

	void OnWizardCreate()
	{
		if (!Directory.Exists (Application.dataPath + "/SavedTileMaps"))
		{
			AssetDatabase.CreateFolder ("Assets", "SavedTileMaps");
		}

		byte[] data = objectToConvert.GetComponent<SpriteRenderer> ().sprite.texture.EncodeToPNG ();
		File.WriteAllBytes (Application.dataPath + "/SavedTileMaps/" + fileName + ".png", data);
	}
}
