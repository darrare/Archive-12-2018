using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Control : MonoBehaviour {
	private int homeTeamScore = Globals.homeTeamScore;
	private int awayTeamScore = Globals.awayTeamScore;
	private int numOuts = Globals.numOuts;
	private int inning = Globals.inning;
	private bool foulPressed = Globals.foulPressed;
	private bool noPitchPressed = Globals.noPitchPressed;
	private bool topOfInning = Globals.topOfInning;
	private bool gameHasWinner = false;
	private bool[] bases = Globals.bases;
	Text homeTeam;
	Text awayTeam;
	Text inningText;
	Text outs;
	private GameObject[] basesObjects = new GameObject[3];
	private GameObject winnerPannel;
	private int homeBatterIndex = Globals.homeBatterIndex;
	private int awayBatterIndex = Globals.awayBatterIndex;
	private GameObject[] homeBatter = new GameObject[3];
	private GameObject[] awayBatter = new GameObject[3];

	private Color atBatColor = new Color(1, 1, 0, 1);
	private Color notAtBatColor = new Color(1, 1, 1, .39f);

	private Override isOverride;

	void Start ()
	{
		homeTeam = GameObject.Find ("HomeScore").GetComponentInChildren<Text> ();
		awayTeam = GameObject.Find ("AwayScore").GetComponentInChildren<Text> ();
		inningText = GameObject.Find ("Inning").GetComponentInChildren<Text> ();
		outs = GameObject.Find ("Outs").GetComponentInChildren<Text> ();

		basesObjects[0] = GameObject.Find ("First");
		basesObjects[1] = GameObject.Find ("Second");
		basesObjects[2] = GameObject.Find ("Third");

		homeBatter [0] = GameObject.Find ("Home1");
		homeBatter [1] = GameObject.Find ("Home2");
		homeBatter [2] = GameObject.Find ("Home3");

		awayBatter [0] = GameObject.Find ("Away1");
		awayBatter [1] = GameObject.Find ("Away2");
		awayBatter [2] = GameObject.Find ("Away3");

		isOverride = GameObject.Find ("Override").GetComponent<Override> ();
		winnerPannel = GameObject.Find ("WinnerPannel");
		winnerPannel.SetActive (false);

		foreach (GameObject batter in homeBatter) {
			batter.GetComponent<Image>().color = new Color(1, 1, 1);
		}
		foreach (GameObject batter in awayBatter) {
			batter.GetComponent<Image>().color = new Color(1, 1, 1);
		}
		homeBatter[homeBatterIndex].GetComponent<Image>().color = new Color(.91f, .24f, .24f);
		awayBatter[awayBatterIndex].GetComponent<Image>().color = new Color(.91f, .24f, .24f);

		if (foulPressed) {
			Color color = GameObject.Find ("Foul").GetComponent<Image> ().color;
			color.a = .39f;
			GameObject.Find ("Foul").GetComponent<Image> ().color = color;
		}
		if (noPitchPressed) {
			Color color = GameObject.Find ("NoPitch").GetComponent<Image> ().color;
			color.a = .39f;
			GameObject.Find ("NoPitch").GetComponent<Image> ().color = color;
		}
	}
	// Update is called once per frame
	void Update () 
	{
		if (!gameHasWinner) {
			if (inning > 4 && !topOfInning)
			{
				if (homeTeamScore > awayTeamScore)
				{
					gameHasWinner = true;
				}
			}
			homeTeam.text = homeTeamScore.ToString ();
			awayTeam.text = awayTeamScore.ToString ();
			if (topOfInning && inning > 4)
			{
				inningText.text = "OT Top " + (inning - 4).ToString ();
				GameObject.Find ("AwayTeam").GetComponent<Image> ().color = atBatColor;
				GameObject.Find ("HomeTeam").GetComponent<Image> ().color = notAtBatColor;
			}
			else if (!topOfInning && inning > 4)
			{
				inningText.text = "OT Bottom " + (inning - 4).ToString ();
				GameObject.Find ("AwayTeam").GetComponent<Image> ().color = atBatColor;
				GameObject.Find ("HomeTeam").GetComponent<Image> ().color = notAtBatColor;
			}
			else if (topOfInning) {
				inningText.text = "Top of the " + inning.ToString ();
				GameObject.Find ("AwayTeam").GetComponent<Image> ().color = atBatColor;
				GameObject.Find ("HomeTeam").GetComponent<Image> ().color = notAtBatColor;
			} else {
				inningText.text = "Bottom of the " + inning.ToString ();
				GameObject.Find ("HomeTeam").GetComponent<Image> ().color = atBatColor;
				GameObject.Find ("AwayTeam").GetComponent<Image> ().color = notAtBatColor;
			}
			outs.text = numOuts.ToString () + " outs";
			
			for (int i = 0; i < 3; i++) {
				if (bases [i]) {
					basesObjects [i].SetActive (true);
				} else {
					basesObjects [i].SetActive (false);
				}
			}
			
			foreach (GameObject batter in homeBatter) {
				if (batter == homeBatter [homeBatterIndex]) {
					homeBatter [homeBatterIndex].GetComponent<Image> ().color = new Color (.91f, .24f, .24f);
				} else if (batter != homeBatter [homeBatterIndex]) {
					batter.GetComponent<Image> ().color = new Color (1, 1, 1);
				}
			}
			
			foreach (GameObject batter in awayBatter) {
				if (batter == awayBatter [awayBatterIndex]) {
					awayBatter [awayBatterIndex].GetComponent<Image> ().color = new Color (.91f, .24f, .24f);
				} else if (batter != awayBatter [awayBatterIndex]) {
					batter.GetComponent<Image> ().color = new Color (1, 1, 1);
				}
			}
			
			Color color = GameObject.Find ("Hit").GetComponent<Image> ().color;
			Color color2 = GameObject.Find ("Hit").GetComponent<Image> ().color;
			color2.a = .39f;
			if (foulPressed) {
				GameObject.Find ("Foul").GetComponent<Image> ().color = color2;
			} else {
				GameObject.Find ("Foul").GetComponent<Image> ().color = color;
			}
			
			if (noPitchPressed) {
				GameObject.Find ("NoPitch").GetComponent<Image> ().color = color2;
			} else {
				GameObject.Find ("NoPitch").GetComponent<Image> ().color = color;
			}
		} else {
			if (homeTeamScore > awayTeamScore)
			{
				winnerPannel.SetActive (true);
				GameObject.Find ("WinnerPannel").transform.GetChild (0).GetComponent<Text>().text = GameObject.Find ("HomeTeam").transform.FindChild("Text").GetComponent<Text>().text + " wins!";
			}
			else if (homeTeamScore < awayTeamScore)
			{
				winnerPannel.SetActive (true);
				GameObject.Find ("WinnerPannel").transform.GetChild (0).GetComponent<Text>().text = GameObject.Find ("AwayTeam").transform.FindChild("Text").GetComponent<Text>().text + " wins!";
			}
			else
			{
				winnerPannel.SetActive (true);
				GameObject.Find ("WinnerPannel").transform.GetChild (0).GetComponent<Text>().text = "Its a tie!";
			}
		}
	}

	public void Hit()
	{
		foulPressed = false;
		noPitchPressed = false;
		if (bases[2])
		{
			if (topOfInning)
			{
				awayTeamScore++;
			}
			else
			{
				homeTeamScore++;
			}
		}
		for (int i = 0; i < 3; i++) {
			if (!bases[i])
			{
				bases[i] = true;
				break;
			}
		}

		//batter control
		if (topOfInning)
		{
			AwayBatterIndexIncrement();
		}
		else
		{
			HomeBatterIndexIncrement();
		}
	}

	public void Out()
	{
		foulPressed = false;
		noPitchPressed = false;
		//batter control
		if (topOfInning)
		{
			AwayBatterIndexIncrement();
		}
		else
		{
			HomeBatterIndexIncrement();
		}

		if (numOuts == 2) {
			if (!topOfInning) {
				inning++;
			}
			topOfInning = !topOfInning;

			if (inning > 4 && topOfInning)
			{
				if (homeTeamScore != awayTeamScore)
				{
					gameHasWinner = true;
				}
			}
			if (inning > 6)
			{
				gameHasWinner = true;
			}

			for (int i = 0; i < 3; i++)
			{
				bases[i] = false;
			}
			numOuts = 0;
		} else {
			numOuts++;
		}
	}

	public void HomeRun()
	{
		foulPressed = false;
		noPitchPressed = false;
		for (int i = 0; i < 3; i++) {
			if (bases[i])
			{
				if (topOfInning)
				{
					awayTeamScore++;
				}
				else
				{
					homeTeamScore++;
				}
			}
			bases[i] = false;
		}
		if (topOfInning)
		{
			awayTeamScore++;
		}
		else
		{
			homeTeamScore++;
		}

		//batter control
		if (topOfInning)
		{
			AwayBatterIndexIncrement();
		}
		else
		{
			HomeBatterIndexIncrement();
		}
	}

	public void AwayBatterIndexIncrement()
	{
		if (awayBatterIndex == 2)
		{
			awayBatterIndex = 0;
		}
		else
		{
			awayBatterIndex++;
		}
	}

	public void HomeBatterIndexIncrement()
	{
		if (homeBatterIndex == 2)
		{
			homeBatterIndex = 0;
		}
		else
		{
			homeBatterIndex++;
		}
	}

	public void FoulBall()
	{
		if (isOverride.IsOverride ()) {
			foulPressed = !foulPressed;
			return;
		}
		if (!foulPressed) {
			foulPressed = true;
		} else {
			foulPressed = false;
			noPitchPressed = false;
			Out ();
		}
	}

	public void NoPitch()
	{
		if (isOverride.IsOverride ()) {
			noPitchPressed = !noPitchPressed;
			return;
		}
		if (!noPitchPressed) {
			noPitchPressed = true;
		} else {
			noPitchPressed = false;
			foulPressed = false;
			Out ();
		}
	}

	public void OpenRules()
	{
		Globals.homeTeamScore = homeTeamScore;
		Globals.awayTeamScore = awayTeamScore;
		Globals.numOuts = numOuts;
		Globals.inning = inning;
		Globals.foulPressed = foulPressed;
		Globals.noPitchPressed = noPitchPressed;
		Globals.topOfInning = topOfInning;
		Globals.bases = bases;
		Globals.homeBatterIndex = homeBatterIndex;
		Globals.awayBatterIndex = awayBatterIndex;
		Application.LoadLevel ("Rules");
	}

	public int HomeTeamScore
	{
		get { return homeTeamScore;}
		set { homeTeamScore = value; }
	}

	public int AwayTeamScore
	{
		get { return awayTeamScore;}
		set { awayTeamScore = value; }
	}

	public int Outs
	{
		get { return numOuts; }
		set { numOuts = value; }
	}

	public void ChangeInning(int value)
	{
		if (value == 1) {
			if (!topOfInning) {
				inning++;
			}
			topOfInning = !topOfInning;
		} else if (value == -1) {
			if (topOfInning){
				inning--;
			}
			topOfInning = !topOfInning;
		}
	}

	public void DiamondControl()
	{
		if (bases [0] && bases [1] && bases [2]) {
			for (int j = 0; j < 3; j++) {
				bases [j] = false;
			}
		} else {
			for (int i = 0; i < 3; i++) {
				if (!bases[i])
				{
					bases[i] = true;
					break;
				}
			}
		}
	}

	public void ResetEverything()
	{
		gameHasWinner = false;
		winnerPannel.SetActive (false);
		homeTeamScore = 0;
		awayTeamScore = 0;
		numOuts = 0;
		inning = 1;
		foulPressed = false;
		noPitchPressed = false;
		topOfInning = true;
		for (int i = 0; i < 3; i++) {
			bases[i] = false;
		}
		homeBatterIndex = 0;
		awayBatterIndex = 0;
	}
}
