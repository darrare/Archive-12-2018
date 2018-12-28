using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [MenuItem("My Project/Create Simple Prefab")]
    static void DoCreateSimplePrefab()
    {
        Transform[] transforms = Selection.transforms;
        foreach (Transform t in transforms)
        {
            Object prefab = PrefabUtility.CreatePrefab("Assets/Generated Prefabs/" + t.gameObject.name + ".prefab", t.gameObject);
        }
    }
}
