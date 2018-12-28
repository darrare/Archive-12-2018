using UnityEngine;
using System.Collections;

public static class CONSTANTS {
	public static string password = "SwingLineSport";
	public static int masterClient = 0;
	public static bool eventStarted = false;
	public static string bracketInfo;

	public static CustomDebug customDebug = GameObject.Find("DebugText").GetComponent<CustomDebug>();
}
