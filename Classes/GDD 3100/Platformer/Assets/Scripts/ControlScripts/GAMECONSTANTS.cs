using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GAMECONSTANTS : MonoBehaviour {
	public static int enemiesDestroyed = 0;
	public static float timeTaken = 0;
	public static int bandsFound = 0;
	public static List<Quest> activeQuests = new List<Quest>();
	public static List<Quest> completedQuests = new List<Quest> ();
	public static Vector2 playerMapPosition = new Vector2(9999, 9999);
	public static float[] playerLocation = new float[2] {9999, 9999};
	public static string mapName = "";
}
