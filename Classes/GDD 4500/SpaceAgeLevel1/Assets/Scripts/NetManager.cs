using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetManager : NetworkManager {

	int connectionId = -1;
	const short clientMsgType = 1002;
	const short serverMsgType = 1003;

	// Use this for initialization
	public void Start () {
		NetworkServer.RegisterHandler(serverMsgType, OnClientReadyToBeginMessage);
		NetworkServer.RegisterHandler(clientMsgType, OnClientReadyToBeginMessage);
	}

	public void SendReadyToBeginMessage(int myId)
	{
		if (connectionId != -1)
		{
			Debug.Log("Attempting to send to " + connectionId);
			for (int i = 0; i < GameObject.Find ("SendableObjects").transform.childCount; i++)
			{
				for (int j = 0; j < GameObject.Find ("SendableObjects").transform.GetChild (i).transform.childCount; j++)
				{
					LevelMessage newMessage = new LevelMessage();
					newMessage.x = GameObject.Find ("SendableObjects").transform.GetChild (i).transform.GetChild (j).transform.position.x;
					newMessage.y = GameObject.Find ("SendableObjects").transform.GetChild (i).transform.GetChild (j).transform.position.y;
					newMessage.zRot = GameObject.Find ("SendableObjects").transform.GetChild (i).transform.GetChild (j).transform.rotation.z;
					newMessage.resourceName = GameObject.Find ("SendableObjects").transform.GetChild (i).transform.GetChild (j).transform.name;
					NetworkServer.SendToClient(connectionId, serverMsgType, newMessage);
				}
			}
		}
		else
		{
			Debug.Log("ERROR: Not connected to client");
		}
	}

	void OnClientReadyToBeginMessage(NetworkMessage netMsg)
	{
		Debug.Log("received OnClientReadyToBeginMessage");
		LevelMessage msg = netMsg.ReadMessage<LevelMessage>();
		SendReadyToBeginMessage(0);
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect(conn);
		connectionId = conn.connectionId;
		Debug.Log("Client Connected: " + connectionId);
	}
}
