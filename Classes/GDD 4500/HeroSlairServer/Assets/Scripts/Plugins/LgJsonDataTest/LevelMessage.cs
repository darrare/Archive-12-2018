using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LevelMessage : MessageBase {
	public string message;

	public string Message
	{
		get { return message;}
		set { message = value;}
	}

	public override void Serialize(NetworkWriter writer)
	{
		//writer.StartMessage (1003);
		writer.Write (message);
		//writer.FinishMessage ();
	}
}
