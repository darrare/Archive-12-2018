using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallpaperShaderDriver : MonoBehaviour {

    [Range(0, 1)] public float insanity;

    [SerializeField]
    float offset = 0.01f;
    float change = 0.01f;

    // Use this for initialization
    void Start () {
        offset = Random.value;
	}
	
	// Update is called once per frame
	void Update () {
        offset += change;
        GetComponent<Renderer>().material.SetFloat("_Offset", offset);
        GetComponent<Renderer>().material.SetFloat("_Insanity", insanity);
    }
}
