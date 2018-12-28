using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class EditorScript : MonoBehaviour
{ 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [MenuItem("Tools/Add mesh colliders to all objects that don't have it already.")]
    private static void AddCollidersToAllMeshesWithoutColliders()
    {
        Object[] tempList = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        List<GameObject> objectsInScene = new List<GameObject>();
        GameObject temp;

        foreach (Object obj in tempList)
        {
            if (obj is GameObject)
            {
                temp = (GameObject)obj;
                if (temp.hideFlags == HideFlags.None)
                    objectsInScene.Add((GameObject)obj);
            }
        }

        foreach (GameObject obj in objectsInScene)
        {
            if (obj.GetComponent<MeshRenderer>() != null)
            {
                obj.AddComponent<MeshCollider>();
            }
        }
    }

    [MenuItem("Tools/Remove mesh colliders to all objects that have it.")]
    private static void RemoveCollidersFromAllMeshesWithoutColliders()
    {
        Object[] tempList = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        List<GameObject> objectsInScene = new List<GameObject>();
        GameObject temp;

        foreach (Object obj in tempList)
        {
            if (obj is GameObject)
            {
                temp = (GameObject)obj;
                if (temp.hideFlags == HideFlags.None)
                    objectsInScene.Add((GameObject)obj);
            }
        }

        foreach (GameObject obj in objectsInScene)
        {
            if (obj.GetComponent<MeshRenderer>() != null)
            {
                DestroyImmediate(obj.GetComponent<MeshCollider>());
            }
        }
    }
}
