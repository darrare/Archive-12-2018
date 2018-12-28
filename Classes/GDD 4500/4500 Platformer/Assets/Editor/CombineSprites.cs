using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

public class CombineSprites : ScriptableWizard {

	public GameObject spritesToCombine;
	private Sprite[] textures;
	private List<Vector2> locations = new List<Vector2> ();
	private List<Vector3> vertices = new List<Vector3> ();
	private List<int> triangles = new List<int> ();
	private int column = 0;
	private int row = 0;
	private int pixelSize;

	[MenuItem("GameObject/Pokemon Engine/Combine Sprites")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard ("Combine Sprite", typeof(CombineSprites));
	}

	void OnWizardCreate()
	{
		if (spritesToCombine != null) 
		{
			pixelSize = spritesToCombine.GetComponentInChildren <SpriteRenderer>().sprite.texture.width;

			//assign each individual sprite on the grid into the textures array.
			textures = new Sprite[spritesToCombine.transform.childCount];
			for (int i = 0; i < spritesToCombine.transform.childCount; i++) {
				textures[i] = spritesToCombine.transform.GetChild (i).GetComponent<SpriteRenderer>().sprite;
			}

			//Get each vertex of all the box colliders and add them to a new mesh.
			for (int i = 0; i < spritesToCombine.transform.childCount; i++)
			{
				if (spritesToCombine.transform.GetChild (i).GetComponent<PolygonCollider2D>() != null)
				{
					locations.Add (spritesToCombine.transform.GetChild (i).position);
				}
			}

			Mesh mesh = new Mesh();
			if (locations.Count > 0)
			{
				//creates the mesh which we will later apply to the single gameObject in order to have collisions.
				foreach (Vector2 vector in locations)
				{
					vertices.Add (new Vector2(vector.x - 0, vector.y - 0));
					vertices.Add (new Vector2(vector.x + 1, vector.y - 0));
					vertices.Add (new Vector2(vector.x - 0, vector.y + 1));
					vertices.Add (new Vector2(vector.x + 1, vector.y + 1));
					
					triangles.Add ((vertices.Count - 1) - 3);
					triangles.Add ((vertices.Count - 1) - 0);
					triangles.Add ((vertices.Count - 1) - 1);
					
					triangles.Add ((vertices.Count - 1) - 3);
					triangles.Add ((vertices.Count - 1) - 2);
					triangles.Add ((vertices.Count - 1) - 0);
				}
				

				mesh.vertices = vertices.ToArray ();
				mesh.triangles = triangles.ToArray ();
			}


			//get the width and height of the grid via the name of the last child
			string childNameFirst = spritesToCombine.transform.GetChild (0).name;
			string[] integersFirst = childNameFirst.Split (' ');
			string childName = spritesToCombine.transform.GetChild (spritesToCombine.transform.childCount - 1).name;
			string[] integers = childName.Split (' ');

			int width = (Convert.ToInt32 (integers[1]) - Convert.ToInt32 (integersFirst[1])) + 1;
			int height = (Convert.ToInt32 (integers[2]) - Convert.ToInt32 (integersFirst[2])) + 1;

			//create one large texture that is the size of all the others combined.
			Texture2D atlas = new Texture2D(width * pixelSize, height * pixelSize, TextureFormat.RGBA32, false);

			//loop through texture array and apply colors to the atlas.
			for (int i = 0; i < textures.Length; i++)
			{
				for (int x = column * pixelSize; x < (column * pixelSize) + pixelSize; x++)
				{
					for (int y = row * pixelSize; y < (row * pixelSize) + pixelSize; y++)
					{
						if (spritesToCombine.transform.GetChild (i).GetComponent<SpriteRenderer>().sprite != null)
							atlas.SetPixel (x, y, textures[i].texture.GetPixel (x - (column * pixelSize), y - (row * pixelSize)));
						else
							atlas.SetPixel (x, y, new Color(0,0,0,0));
					}
				}
				if (row == height - 1)
				{
					row = 0;
					column++;
				}
				else
				{
					row++;
				}
			}

			atlas.Apply ();
			GameObject combinedSprites = new GameObject();
			combinedSprites.transform.position = spritesToCombine.transform.position;
			if (locations.Count > 0)
			{
				combinedSprites.AddComponent<MeshCollider>();
				combinedSprites.GetComponent<MeshCollider>().sharedMesh = mesh;
			}
			combinedSprites.AddComponent<SpriteRenderer>();
			combinedSprites.GetComponent<SpriteRenderer>().sprite = Sprite.Create (atlas, new Rect(0, 0, atlas.width, atlas.height), new Vector2(0, 0), pixelSize);
			combinedSprites.GetComponent<SpriteRenderer>().sprite.texture.filterMode = FilterMode.Point;
		}
	}
}
