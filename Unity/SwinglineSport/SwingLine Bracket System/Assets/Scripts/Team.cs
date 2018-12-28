using UnityEngine;
using System.Collections;

public class Team {
	public string teamName;
	public string captainName;
	public string player1Name;
	public string player2Name;

	public Team(string teamName, string captainName, string player1Name, string player2Name)
	{
		this.teamName = teamName;
		this.captainName = captainName;
		this.player1Name = player1Name;
		this.player2Name = player2Name;
	}

	public string TeamName
	{
		get { return teamName; }
		set { teamName = value;}
	}

	public string CaptainName
	{
		get { return captainName; }
		set { captainName = value;}
	}

	public string Player1Name
	{
		get { return player1Name; }
		set { player1Name = value;}
	}

	public string Player2Name
	{
		get { return player2Name; }
		set { player2Name = value;}
	}
}
