using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using LgOctEngine.CoreClasses;

public class NetManager : NetworkManager {
	public static List<StaffClient> staffClients = new List<StaffClient> ();
	public static List<TeamInfo> teams = new List<TeamInfo>();
	
	// Use this for initialization
	void Start () {
		Debug.Log ("NetMangager:Start()");
		StartClient ();
		client.RegisterHandler(MessageType.STAFF_CLIENT, OnServerReceiveMessage<StaffClientMessage>);
		client.RegisterHandler(MessageType.TEAM_INFO, OnServerReceiveMessage<TeamInfoMessage>);
		client.RegisterHandler(MessageType.GAME_RESULT, OnServerReceiveMessage<GameResult>);
		client.RegisterHandler (MessageType.REQ_STAFF_CLIENTS, OnServerReceiveMessage<StaffClientMessage>);
		//NetworkServer.RegisterHandler(MessageType.ASSIGNMENT, OnServerReceiveMessage<Assignment>);
		//NetworkServer.RegisterHandler(MessageType.ERROR, OnServerReceiveMessage<Error>);
	}
	

	void OnServerReceiveMessage<T>(NetworkMessage netMsg) where T : LgJsonDictionary, IJsonable, new()
	{
		JsonMessage<T> jsonMessage = netMsg.ReadMessage<JsonMessage<T>>();
		Debug.Log("OnServerReceiveMessage: Received Message " + jsonMessage.message);
		T obj = LgJsonNode.CreateFromJsonString<T>(jsonMessage.message);
		obj.HandleNewObject(netMsg.conn.connectionId);
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		client.Send (MessageType.MASTER_CLIENT_LOGIN, new EmptyMessage());
	}

	public void ReturnBracketInfo()
	{
		BracketInfo msg = LgJsonNode.Create<BracketInfo> ();
		msg.numberOfTeams = BRACKET.numberOfTeams;
		msg.firstRowCount = BRACKET.firstRowCount;
		msg.leftOvers = BRACKET.leftOvers;
		msg.tiers = BRACKET.tiers;
		JsonMessage<BracketInfo> sendMsg = new JsonMessage<BracketInfo> ();
		sendMsg.message = msg.Serialize ();
		client.Send(MessageType.BRACKET_INFO, sendMsg);
	}

	public void ResetStaffList()
	{
		staffClients.Clear ();
		client.Send (MessageType.REQ_STAFF_CLIENTS, new EmptyMessage ());
	}

	public void SendInstructionToStaff(int connectionId, string homeTeam, string awayTeam, string fieldNumber)
	{
		StaffInstruction msg = LgJsonNode.Create<StaffInstruction> ();
		msg.homeTeam = homeTeam;
		msg.awayTeam = awayTeam;
		msg.connectionId = connectionId;
		msg.fieldNum = int.Parse (fieldNumber);
		JsonMessage<StaffInstruction> sendMsg = new JsonMessage<StaffInstruction> ();
		sendMsg.message = msg.Serialize ();
		client.Send (MessageType.ASSIGNMENT, sendMsg);
	}
}

public class StaffClient
{
	public string username;
	public int connectionId;
	
	public StaffClient(string username, int connectionId)
	{
		this.username = username;
		this.connectionId = connectionId;
	}
}

public class TeamInfo
{
	string teamName, player1, player2, player3;
	
	public TeamInfo(string teamName, string player1, string player2, string player3)
	{
		this.teamName = teamName;
		this.player1 = player1;
		this.player2 = player2;
		this.player3 = player3;
	}
}