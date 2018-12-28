using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CONSTANTS {

	public static List<GameStateInfo> gameStateList = new List<GameStateInfo> ();
	public static string tweetMessage = "Hey everyone! I've found this new awesome sport, check it out! #SwingLineSport";

	public static bool isStaff = false;
	public static string name = "";
	public static NetManager netManager;
	public static bool isConnectedToServer = false;
	public static UserHomeControl userHomeControl;
	public static MainMenuControl mainMenuControl;

	//ALL USER LOGIN RELATED INFORMATION
	public static string username = "";
	public static string teamName = "";

	//Score tracking stuff
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
	public static string homeTeamName = "Home Team";
	public static string awayTeamName = "Away Team";
	public static int fieldNum = 0;
	public static bool currentlyInGame = false;

	public static void ResetScoreTrackingConstants()
	{
		homeTeamScore = 0;
		awayTeamScore = 0;
		numOuts = 0;
		inning = 1;
		foulPressed = false;
		noPitchPressed = false;
		topOfInning = true;
		bases = new bool[3]{false, false, false};
		homeBatterIndex = 0;
		awayBatterIndex = 0;
		homeTeamName = "Home Team";
		awayTeamName = "Away Team";
		fieldNum = 0;
		currentlyInGame = false;
	}
}

public struct GameStateInfo
{
	public int homeTeamScore;
	public int awayTeamScore;
	public int numOuts;
	public int inning;
	public bool foulPressed;
	public bool noPitchPressed;
	public bool topOfInning;
	public bool[] bases;
	public int homeBatterIndex;
	public int awayBatterIndex;

	public GameStateInfo(int homeTeamScore, int awayTeamScore, int numOuts, int inning, bool foulPressed, bool noPitchPressed, bool topOfInning, bool[] bases, int homeBatterIndex, int awayBatterIndex)
	{
		this.homeTeamScore = homeTeamScore;
		this.awayTeamScore = awayTeamScore;
		this.numOuts = numOuts;
		this.inning = inning;
		this.foulPressed = foulPressed;
		this.noPitchPressed = noPitchPressed;
		this.topOfInning = topOfInning;
		this.bases = new bool[3];
		this.bases [0] = bases [0];
		this.bases [1] = bases [1];
		this.bases [2] = bases [2];
		this.homeBatterIndex = homeBatterIndex;
		this.awayBatterIndex = awayBatterIndex;
	}
}