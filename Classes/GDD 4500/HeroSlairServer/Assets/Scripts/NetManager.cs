using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using LgOctEngine.CoreClasses;
using System.IO;

public class NetManager : NetworkManager {
	public static List<Connection> networkConnections = new List<Connection> ();
	public static List<string> levels = new List<string> ();

	void Start()
	{
		Debug.Log ("NetMangager:Start()");
		StartServer ();

		//Register all handlers here when we get the message types defined.
		NetworkServer.RegisterHandler(MessageType.LOGIN_MSG, OnServerReceiveMessage<Login>);
		//NetworkServer.RegisterHandler(MessageType.LEVEL_REQ_SINGLE, OnServerReceiveMessage<Level>);
		NetworkServer.RegisterHandler(MessageType.REQUEST_LIST, OnServerReceiveMessage<LevelArrayReq>);
		NetworkServer.RegisterHandler(MessageType.LEVEL_MSG, OnServerReceiveMessage<Level>);

		//Load the levels
		DirectoryInfo dir = new DirectoryInfo (Application.dataPath + "/Levels/");
		FileInfo[] info = dir.GetFiles ("*.lvl");
		foreach (FileInfo f in info) {
			levels.Add (SaveLoad.Load(f.Name));
		}
//		foreach (string str in levels) {
//			Debug.Log (str);
//		}
	}


	public void OnServerSendLevel(int connectionId)
	{
		Debug.Log("Sending Level " + connectionId);
		var msg = new JsonMessage<Level>();
		
		// Take the level, serialize it and store in the message
		Level level = LgJsonNode.Create<Level>();
		LevelObject levelObject = level.LevelObjectArray.AddNew();
		levelObject.id = "ExamplePrefab";
		levelObject.row = 5;
		levelObject.column = 3 * 2;
		levelObject.rotation = 90;
		levelObject.status = 0;
		
		msg.message = level.Serialize();
		//NetworkServer.SendToClient(connectionId, MessageType.LEVEL_MSG, msg);
	}

	public static void OnServerSendLoginSuccess(int connectionId)
	{
		Debug.Log("Sending Login Success to connectionId: " + connectionId);
		var msg = new JsonMessage<Acknowledgement>();
		
		Acknowledgement acknowledgement = LgJsonNode.Create<Acknowledgement>();
		acknowledgement.ack = MessageValue.LOGIN_SUCCESS;
		
		msg.message = acknowledgement.Serialize();
		NetworkServer.SendToClient(connectionId, MessageType.ACKNOWLEDGE, msg);
	}
	
	public static void OnServerSendLoginFailure(int connectionId)
	{
		Debug.Log("Sending Login Failure to connectionId: " + connectionId);
		var msg = new JsonMessage<Acknowledgement>();
		
		Acknowledgement acknowledgement = LgJsonNode.Create<Acknowledgement>();
		acknowledgement.ack = MessageValue.LOGIN_FAILURE;
		
		msg.message = acknowledgement.Serialize();
		NetworkServer.SendToClient(connectionId, MessageType.ACKNOWLEDGE, msg);
	}

	public static void OnServerSendLevelArray(int connectionId)
	{
		Debug.Log ("Sending level array to connectionId: " + connectionId);
	}
	
	void OnServerReceiveMessage<T>(NetworkMessage netMsg) where T : LgJsonDictionary, IJsonable, new()
	{
		JsonMessage<T> jsonMessage = netMsg.ReadMessage<JsonMessage<T>>();
		Debug.Log("OnServerReceiveMessage: Received Message " + jsonMessage.message);
		T obj = LgJsonNode.CreateFromJsonString<T>(jsonMessage.message);
		obj.HandleNewObject(netMsg.conn.connectionId);
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect(conn);
		if (networkConnections.Count < conn.connectionId) {
			networkConnections.Add (new Connection (conn.address, conn.connectionId, "null"));
		} else {
			networkConnections[conn.connectionId - 1] = new Connection (conn.address, conn.connectionId, "null");
		}

		Debug.Log("Client Connected: " + conn.connectionId);
	}
	
}