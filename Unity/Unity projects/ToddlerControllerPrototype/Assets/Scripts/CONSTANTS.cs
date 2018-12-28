using UnityEngine;
using System.Collections;

public static class CONSTANTS {
	public static int layerCounter = 0;
	public static Transform splatParent = GameObject.Find ("SplatParent").transform;
	public static Color[] colors = { Color.blue, Color.red, Color.green, Color.yellow };
	public static BalloonSpawner balloonSpawner;
}
