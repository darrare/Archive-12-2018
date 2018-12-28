using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateSpriteGrid : ScriptableWizard {

	public string spriteGroupName = "New Sprite Cluster";
	public Sprite spriteImage;
	public int gridWidth = 1;
	public int gridHeight = 1;
	public int startingXCoordinate = 0;
	public int startingYCoordinate = 0;
	
	[MenuItem("GameObject/Pokemon Engine/Create Grid")]
	static void CreateGrid()
	{
		ScriptableWizard.DisplayWizard ("Create Grid", typeof(CreateSpriteGrid));
	}
	
	void OnWizardCreate()
	{
		GameObject parent = new GameObject (spriteGroupName);
		
		for (int x = startingXCoordinate; x < startingXCoordinate + gridWidth; x++) {
			for (int y = startingYCoordinate; y < startingYCoordinate + gridHeight; y++)
			{
				GameObject sprite = new GameObject("Sprite: " +  (x - startingXCoordinate) + " " + (y - startingYCoordinate));
				SpriteRenderer renderer = sprite.AddComponent<SpriteRenderer>();
				renderer.sprite = spriteImage;
				
				sprite.transform.position = new Vector3(x, y, 0);
				sprite.transform.parent = parent.transform;
				
				//				GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
				//				quad.transform.position = new Vector3(x, 0, z);
				//				quad.transform.Rotate(90, 0, 0);
				//				quad.name = "Quad: " +  x + ", " + z;
				//				quad.transform.parent = parent.transform;
			}
		}
	}
}
