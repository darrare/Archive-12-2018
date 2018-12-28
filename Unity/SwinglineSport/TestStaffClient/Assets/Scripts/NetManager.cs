using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using LgOctEngine.CoreClasses;

public class NetManager : NetworkManager {

	// Use this for initialization
	void Start () {
		Debug.Log ("NetMangager:Start()");
		StartClient ();
		client.RegisterHandler (MessageType.LOGIN_MSG, OnServerReceiveMessage<Acknowledgement>);
		client.RegisterHandler (MessageType.ASSIGNMENT, OnServerReceiveMessage<StaffInstruction>);
	}
	
	void OnServerReceiveMessage<T>(NetworkMessage netMsg) where T : LgJsonDictionary, IJsonable, new()
	{
		JsonMessage<T> jsonMessage = netMsg.ReadMessage<JsonMessage<T>>();
		Debug.Log("OnServerReceiveMessage: Received Message " + jsonMessage.message);
		T obj = LgJsonNode.CreateFromJsonString<T>(jsonMessage.message);
		obj.HandleNewObject(netMsg.conn.connectionId);
	}

	public void SubmitClick()
	{
		CONSTANTS.name = GameObject.Find ("Name").GetComponent<InputField> ().text;
		StaffLoginAttempt staff = LgJsonNode.Create<StaffLoginAttempt> ();
		staff.username = CONSTANTS.name;
		staff.password = GameObject.Find ("Password").GetComponent<InputField> ().text;
		JsonMessage<StaffLoginAttempt> msg = new JsonMessage<StaffLoginAttempt> ();
		msg.message = staff.Serialize ();
		client.Send(MessageType.LOGIN_MSG, msg);
	}

	public void HomeWinClick()
	{
		GameResult result = LgJsonNode.Create<GameResult> ();
		result.result = "Home";
		result.connectionId = 0;
		result.name = CONSTANTS.name;
		JsonMessage<GameResult> msg = new JsonMessage<GameResult> ();
		msg.message = result.Serialize ();
		client.Send (MessageType.GAME_RESULT, msg);
	}

	public void AwayWinClick()
	{
		GameResult result = LgJsonNode.Create<GameResult> ();
		result.result = "Away";
		result.connectionId = 0;
		result.name = CONSTANTS.name;
		JsonMessage<GameResult> msg = new JsonMessage<GameResult> ();
		msg.message = result.Serialize ();
		client.Send (MessageType.GAME_RESULT, msg);
	}
}
