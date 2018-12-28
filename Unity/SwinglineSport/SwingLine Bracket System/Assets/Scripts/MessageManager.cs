using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using LgOctEngine.CoreClasses;

public static class MessageType
{
	public static short LOGIN_MSG = 1000;
	public static short MASTER_CLIENT_LOGIN = 1001;
	public static short STAFF_CLIENT = 1010;
	public static short REQ_STAFF_CLIENTS = 1011;
	public static short TEAM_INFO = 1020;
	public static short BRACKET_INFO = 1021;
	public static short GAME_RESULT = 1030;
	public static short ASSIGNMENT = 1040;
	public static short ERROR = 1050;
}

public static class MessageValue
{
	public static short LOGIN_FAILURE = 0;
	public static short LOGIN_SUCCESS = 1;
}

public class EmptyMessage : MessageBase
{

}

// Use messages of this type to send ANY JSON formatted message
// Once you pull the string from this object, you can then
// decode it based on the message type

interface IJsonable
{
	void HandleNewObject(int connectionId);
}

public class JsonMessage<T> : MessageBase
{
	public string message;
}

public class StaffClientMessage : LgJsonDictionary, IJsonable
{
	public string username { get { return GetValue<string>("username", ""); } set { SetValue<string>("username", value); } }
	public int connectionId { get { return GetValue<int> ("connectionId", 0); } set { SetValue<int> ("connectionId", value); } }
	
	public void HandleNewObject(int conn)
	{
		StaffClient staff = new StaffClient (username, connectionId);
		NetManager.staffClients.Add (staff);
		if (Application.loadedLevelName == "StaffManagement") {
			GameObject.Find ("Canvas").GetComponent<GenerateStaffList>().DrawStaffMember(staff);
		}
	}
}

public class TeamInfoMessage : LgJsonDictionary, IJsonable
{
	public string teamName { get { return GetValue<string>("teamName", ""); } set { SetValue<string>("teamName", value); } }
	public string player1 { get { return GetValue<string>("player1", ""); } set { SetValue<string>("player1", value); } }
	public string player2 { get { return GetValue<string>("player2", ""); } set { SetValue<string>("player2", value); } }
	public string player3 { get { return GetValue<string>("player3", ""); } set { SetValue<string>("player3", value); } }
	
	public void HandleNewObject(int connectionId)
	{
		NetManager.teams.Add (new TeamInfo (teamName, player1, player2, player3));
	}
}

public class GameResult : LgJsonDictionary, IJsonable
{
	public string result { get { return GetValue<string> ("result", ""); } set { SetValue<string> ("result", value); } }
	public string name { get { return GetValue<string> ("name", ""); } set { SetValue<string> ("name", value); } }
	public int connectionId { get { return GetValue<int> ("connectionId", 0); } set { SetValue<int> ("connectionId", value); } }
	public string home { get { return GetValue<string> ("home", ""); } set { SetValue<string> ("home", value); } }
	public string away { get { return GetValue<string> ("away", ""); } set { SetValue<string> ("away", value); } }

	public void HandleNewObject(int conn)
	{
		//GameObject.Find ("NetManager").GetComponent<NetManager> ().ResetStaffList ();
		if (result == "Home") {
			Debug.Log (name + " sent in result of " + home + " vs " + away + "... " + home + " wins.");
		} else if (result == "Away") {
			Debug.Log (name + " sent in result of " + home + " vs " + away + "... " + away + " wins.");
		} else if (result == "Tie") {
			Debug.Log (name + " sent in result of " + home + " vs " + away + "... It's a Tie.");
		}
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
}

public class BracketInfo : LgJsonDictionary, IJsonable
{
	public int numberOfTeams { get { return GetValue<int> ("numberOfTeams", 0); } set { SetValue<int> ("numberOfTeams", value); } }
	public int firstRowCount { get { return GetValue<int> ("firstRowCount", 0); } set { SetValue<int> ("firstRowCount", value); } }
	public int leftOvers { get { return GetValue<int> ("leftOvers", 0); } set { SetValue<int> ("leftOvers", value); } }
	public int tiers { get { return GetValue<int> ("tiers", 0); } set { SetValue<int> ("tiers", value); } }

	public void HandleNewObject(int connectionId)
	{

	}
}

public class StaffInstruction : LgJsonDictionary, IJsonable
{
	public string homeTeam { get { return GetValue<string> ("homeTeam", ""); } set { SetValue<string> ("homeTeam", value); } }
	public string awayTeam { get { return GetValue<string> ("awayTeam", ""); } set { SetValue<string> ("awayTeam", value); } }
	public int connectionId { get { return GetValue<int> ("connectionId", 0); } set { SetValue<int> ("connectionId", value); } }
	public int fieldNum { get { return GetValue<int> ("fieldNum", 0); } set { SetValue<int> ("fieldNum", value); } }

	public void HandleNewObject(int connectionId)
	{

	}
}















public class LevelObject : LgJsonDictionary, IJsonable
{
	public string id { get { return GetValue<string>("id", ""); } set { SetValue<string>("id", value); } }       // The unique string identifier that corresponds to the Prefab to load
	public int row { get { return GetValue<int>("row", 0); } set { SetValue<int>("row", value); } }             // The row of the "grid" that the object occupies
	public int column { get { return GetValue<int>("column", 0); } set { SetValue<int>("column", value); } }      // The column of the "grid" that the object occupies
	public float rotation { get { return GetValue<float>("rotation", 0); } set { SetValue<float>("rotation", value); } }  // The rotation of the object, in degrees, in a clockwise manner.  A zero rotation would be "upright".
	public int status { get { return GetValue<int>("status", 0); } set { SetValue<int>("status", value); } }     // The status of the object.
	
	public void HandleNewObject(int connectionId)
	{
		Debug.Log("Handling LevelObject");
		// TODO: put code that does something with this object
	}
}

public class Level : LgJsonDictionary, IJsonable
{
	public string LevelName { get { return GetValue<string> ("levelName", ""); } set { SetValue<string> ("levelName", value); } }
	public LgJsonArray<LevelObject> LevelObjectArray
	{
		get { return GetNode<LgJsonArray<LevelObject>>("Level"); }
		set { SetNode<LgJsonArray<LevelObject>>("Level", value); }
	}
	
	public void HandleNewObject(int connectionId)
	{
		Debug.Log("Handling Level");
	}
}

public class LevelArrayReq : LgJsonDictionary, IJsonable
{
	public LgJsonArray<Level> Levels
	{
		get { return GetNode<LgJsonArray<Level>>("Level"); }
		set { SetNode<LgJsonArray<Level>>("Level", value); }
	}
	
	public void HandleNewObject(int connectionId)
	{
		Debug.Log ("Handling level array");
		var msg = new JsonMessage<LevelArrayReq>();
		msg.message = Levels.Serialize ();
	}
}



public class Acknowledgement : LgJsonDictionary, IJsonable
{
	public int ack { get { return GetValue<int>("ack", 0); } set { SetValue<int>("ack", value); } }
	
	public void HandleNewObject(int connectionId)
	{
		Debug.Log("Handling Acknowledgement");
		// TODO: put code that does something with this object
	}
}
