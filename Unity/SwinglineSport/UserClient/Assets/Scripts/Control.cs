using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour {

	public GameObject homeTeamText;
	public GameObject awayTeamText;
	public GameObject homeTeamPanel, awayTeamPanel;
	public GameObject foulButton, noPitchButton;

	protected int homeTeamScore = CONSTANTS.homeTeamScore;
	protected int awayTeamScore = CONSTANTS.awayTeamScore;

	private int numOuts = CONSTANTS.numOuts;
	private int inning = CONSTANTS.inning;
	private bool foulPressed = CONSTANTS.foulPressed;
	private bool noPitchPressed = CONSTANTS.noPitchPressed;
	private bool topOfInning = CONSTANTS.topOfInning;
	private bool gameHasWinner = false;

	protected bool ranOnce = false;

	private bool[] bases = new bool[3];

	Text homeTeam;
	Text awayTeam;
	Text inningText;
	Text outs;

    protected GameObject winnerPannel;

    private GameObject[] basesObjects = new GameObject[3];
	private int homeBatterIndex = CONSTANTS.homeBatterIndex;
	private int awayBatterIndex = CONSTANTS.awayBatterIndex;
	private GameObject[] homeBatter = new GameObject[3];
	private GameObject[] awayBatter = new GameObject[3];
	private Color atBatColor = new Color(1, 1, 0, 1);
	private Color notAtBatColor = new Color(1, 1, 1, .39f);

	protected virtual void Start ()
	{
		homeTeam = GameObject.Find ("HomeScore").GetComponentInChildren<Text> ();
		awayTeam = GameObject.Find ("AwayScore").GetComponentInChildren<Text> ();
		ChangeNames ();
		inningText = GameObject.Find ("Inning").GetComponentInChildren<Text> ();
		outs = GameObject.Find ("Outs").GetComponentInChildren<Text> ();

		bases [0] = CONSTANTS.bases [0];
		bases [1] = CONSTANTS.bases [1];
		bases [2] = CONSTANTS.bases [2];

		basesObjects[0] = GameObject.Find ("First");
		basesObjects[1] = GameObject.Find ("Second");
		basesObjects[2] = GameObject.Find ("Third");

		homeBatter [0] = GameObject.Find ("Home1");
		homeBatter [1] = GameObject.Find ("Home2");
		homeBatter [2] = GameObject.Find ("Home3");

		awayBatter [0] = GameObject.Find ("Away1");
		awayBatter [1] = GameObject.Find ("Away2");
		awayBatter [2] = GameObject.Find ("Away3");

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

	protected virtual void ChangeNames()
	{
		homeTeamText.GetComponentInParent<InputField> ().text = CONSTANTS.homeTeamName;
		awayTeamText.GetComponentInParent<InputField> ().text = CONSTANTS.awayTeamName;
	}

	// Update is called once per frame
	protected virtual void Update () 
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
				awayTeamPanel.GetComponent<Image> ().color = atBatColor;
				homeTeamPanel.GetComponent<Image> ().color = notAtBatColor;
			}
			else if (!topOfInning && inning > 4)
			{
				inningText.text = "OT Bottom " + (inning - 4).ToString ();
				awayTeamPanel.GetComponent<Image> ().color = atBatColor;
				homeTeamPanel.GetComponent<Image> ().color = notAtBatColor;
			}
			else if (topOfInning) {
				inningText.text = "Top of the " + inning.ToString ();
				awayTeamPanel.GetComponent<Image> ().color = atBatColor;
				homeTeamPanel.GetComponent<Image> ().color = notAtBatColor;
			} else {
				inningText.text = "Bottom of the " + inning.ToString ();
				homeTeamPanel.GetComponent<Image> ().color = atBatColor;
				awayTeamPanel.GetComponent<Image> ().color = notAtBatColor;
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
				
			if (foulPressed) {
				foulButton.GetComponent<Image> ().color = notAtBatColor;
			} else {
				foulButton.GetComponent<Image> ().color = Color.white;
			}
			
			if (noPitchPressed) {
				noPitchButton.GetComponent<Image> ().color = notAtBatColor;
			} else {
				noPitchButton.GetComponent<Image> ().color = Color.white;
			}
		} else if (!ranOnce) {
			DisplayWinnerMessage();
		}
	}

	protected virtual void DisplayWinnerMessage()
	{
		ranOnce = true;
		if (homeTeamScore > awayTeamScore)
		{
			winnerPannel.SetActive (true);
			winnerPannel.transform.GetChild (0).GetComponent<Text>().text = GameObject.Find ("HomeTeam").transform.FindChild("Text").GetComponent<Text>().text + " wins!";
		}
		else if (homeTeamScore < awayTeamScore)
		{
			winnerPannel.SetActive (true);
			winnerPannel.transform.GetChild (0).GetComponent<Text>().text = GameObject.Find ("AwayTeam").transform.FindChild("Text").GetComponent<Text>().text + " wins!";
		}
		else
		{
			winnerPannel.SetActive (true);
			winnerPannel.transform.GetChild (0).GetComponent<Text>().text = "Its a tie!";
		}
	}

	public void Hit()
	{
		SavePreviousState ();
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
		SavePreviousState ();
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
		SavePreviousState ();
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
		if (!foulPressed) {
			SavePreviousState ();
			foulPressed = true;
		} else {
			foulPressed = false;
			noPitchPressed = false;
			Out ();
		}
	}

	public void NoPitch()
	{
		if (!noPitchPressed) {
			SavePreviousState ();
			noPitchPressed = true;
		} else {
			noPitchPressed = false;
			foulPressed = false;
			Out ();
		}
	}

	public void OpenRules()
	{
		CONSTANTS.homeTeamScore = homeTeamScore;
		CONSTANTS.awayTeamScore = awayTeamScore;
		CONSTANTS.numOuts = numOuts;
		CONSTANTS.inning = inning;
		CONSTANTS.foulPressed = foulPressed;
		CONSTANTS.noPitchPressed = noPitchPressed;
		CONSTANTS.topOfInning = topOfInning;
		CONSTANTS.bases = bases;
		CONSTANTS.homeBatterIndex = homeBatterIndex;
		CONSTANTS.awayBatterIndex = awayBatterIndex;
		CONSTANTS.homeTeamName = homeTeamText.GetComponent<Text>().text;
		CONSTANTS.awayTeamName = awayTeamText.GetComponent<Text>().text;
		SceneManager.LoadScene ("Rules");
	}

	public void BackToMenu()
	{
		CONSTANTS.homeTeamScore = homeTeamScore;
		CONSTANTS.awayTeamScore = awayTeamScore;
		CONSTANTS.numOuts = numOuts;
		CONSTANTS.inning = inning;
		CONSTANTS.foulPressed = foulPressed;
		CONSTANTS.noPitchPressed = noPitchPressed;
		CONSTANTS.topOfInning = topOfInning;
		CONSTANTS.bases = bases;
		CONSTANTS.homeBatterIndex = homeBatterIndex;
		CONSTANTS.awayBatterIndex = awayBatterIndex;
		CONSTANTS.homeTeamName = homeTeamText.GetComponent<Text>().text;
		CONSTANTS.awayTeamName = awayTeamText.GetComponent<Text>().text;
		SceneManager.LoadScene ("MainMenu");
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

	public virtual void ResetEverything()
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
		ranOnce = false;
		CONSTANTS.gameStateList.Clear ();
	}

	void SavePreviousState()
	{
		CONSTANTS.gameStateList.Add (new GameStateInfo (homeTeamScore, awayTeamScore, numOuts, inning, foulPressed, noPitchPressed, topOfInning, bases, homeBatterIndex, awayBatterIndex));
	}

	public void LoadPreviousState()
	{
		homeTeamScore = CONSTANTS.gameStateList [CONSTANTS.gameStateList.Count - 1].homeTeamScore;
		awayTeamScore = CONSTANTS.gameStateList [CONSTANTS.gameStateList.Count - 1].awayTeamScore;
		numOuts = CONSTANTS.gameStateList [CONSTANTS.gameStateList.Count - 1].numOuts;
		inning = CONSTANTS.gameStateList [CONSTANTS.gameStateList.Count - 1].inning;
		foulPressed = CONSTANTS.gameStateList [CONSTANTS.gameStateList.Count - 1].foulPressed;
		noPitchPressed = CONSTANTS.gameStateList [CONSTANTS.gameStateList.Count - 1].noPitchPressed;
		topOfInning = CONSTANTS.gameStateList [CONSTANTS.gameStateList.Count - 1].topOfInning;
		bases = CONSTANTS.gameStateList [CONSTANTS.gameStateList.Count - 1].bases;
		homeBatterIndex = CONSTANTS.gameStateList [CONSTANTS.gameStateList.Count - 1].homeBatterIndex;
		awayBatterIndex = CONSTANTS.gameStateList [CONSTANTS.gameStateList.Count - 1].awayBatterIndex;
		CONSTANTS.gameStateList.RemoveAt (CONSTANTS.gameStateList.Count - 1);
	}
}

