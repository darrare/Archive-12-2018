using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClientManager : NetworkBehaviour {

	const short clientMsgType = 1002;
	const short serverMsgType = 1003;
	
	public NetworkManager myManager;
	public NetworkClient myClient;

	public void Start()
	{
		Init(myManager.client);
	}
	
	public void Init(NetworkClient client)
	{
		myClient = client;
		NetworkServer.RegisterHandler(serverMsgType, OnServerReadyToBeginMessage);
		NetworkServer.RegisterHandler(clientMsgType, OnServerReadyToBeginMessage);
		if (myClient != null) {
			Debug.Log ("Getting registered");
			myClient.RegisterHandler (serverMsgType, OnServerReadyToBeginMessage);
			myClient.RegisterHandler (clientMsgType, OnServerReadyToBeginMessage);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.V)) {
			SendReadyToBeginMessage(0);
		}
	}
	
	public void SendReadyToBeginMessage(int myId)
	{
		LevelMessage msg = new LevelMessage();
		msg.resourceName = "asldkjnaklsjdna";
		myClient.Send(clientMsgType, msg);
	}
	
	void OnServerReadyToBeginMessage(NetworkMessage netMsg)
	{
		LevelMessage beginMsg = netMsg.ReadMessage<LevelMessage>();
		Debug.Log("received OnServerReadyToBeginMessage: " + beginMsg.resourceName);
	}
}
