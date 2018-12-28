using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PanelScript : MonoBehaviour {

    Vector2 offset;

    public int connId;

    public Text name;

    public Text chat;

    public InputField chatBox;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MovePanel()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

    }

    public void ResetMovePanel()
    {
        
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SendText()
    {
        short DEFAULT_MESSAGE = 1000;
        Msg message = new Msg();
        message.message = chatBox.text;

        if (!CONSTANTS.IS_HOST)
        {
            NetworkManager.singleton.client.Send(DEFAULT_MESSAGE, message);
        }
        else
        {
            NetworkServer.SendToClient(connId, DEFAULT_MESSAGE, message);
        }

        chatBox.text = "";
    }
}
