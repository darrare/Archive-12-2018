using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LevelMessage : MessageBase {
	public float x;
	public float y;
	public float zRot;
	public string resourceName;
	public bool isReady;
}
