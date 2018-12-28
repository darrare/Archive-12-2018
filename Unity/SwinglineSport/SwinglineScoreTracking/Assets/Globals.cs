using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {
	public static int homeTeamScore = 0;
	public static int awayTeamScore = 0;
	public static int numOuts = 0;
	public static int inning = 1;
	public static bool foulPressed = false;
	public static bool noPitchPressed = false;
	public static bool topOfInning = true;
	public static bool[] bases = {false, false, false};
	public static int homeBatterIndex = 0;
	public static int awayBatterIndex = 0;

}
