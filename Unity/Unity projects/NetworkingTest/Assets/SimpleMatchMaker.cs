using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public static class CONSTANTS
{
    public static bool IS_HOST = false;
}

public class SimpleMatchMaker : MonoBehaviour
{
    public Text text;
    public Canvas mainCanvas;
    List<GameObject> panels = new List<GameObject>();


    void Start()
    {
        NetworkManager.singleton.StartMatchMaker();
        //CreateInternetMatch("TestMatch");
    }

    //call this method to request a match to be created on the server
    public void CreateInternetMatch(string matchName)
    {
        CreateMatchRequest create = new CreateMatchRequest();
        create.name = matchName;
        create.size = 4;
        create.advertise = true;
        create.password = "";

        NetworkManager.singleton.matchMaker.CreateMatch(create, OnInternetMatchCreate);
        NetworkServer.RegisterHandler(1000, CallThisWhenReceiveMessage);
        CONSTANTS.IS_HOST = true;
    }

    void CallThisWhenReceiveMessage(NetworkMessage netMsg)
    {
        text.text += "\n Received message";
        foreach (GameObject panel in panels)
        {
            if (panel.GetComponent<PanelScript>().name.text == netMsg.conn.connectionId.ToString())
            {
                panel.GetComponent<PanelScript>().chat.text += "\n" + netMsg.ReadMessage<Msg>().message;
                return;
            }
        }
        text.text += "\n Creating new panel";
        GameObject newPanel = Instantiate(Resources.Load("ChatPanel")) as GameObject;
        newPanel.transform.SetParent(mainCanvas.transform, false);
        newPanel.GetComponent<PanelScript>().name.text = netMsg.conn.connectionId.ToString();
        newPanel.GetComponent<PanelScript>().connId = netMsg.conn.connectionId;

        panels.Add(newPanel);
    }

    //this method is called when your request for creating a match is returned
    private void OnInternetMatchCreate(CreateMatchResponse matchResponse)
    {
        if (matchResponse != null && matchResponse.success)
        {
            text.text += "\n Created match";

            MatchInfo hostInfo = new MatchInfo(matchResponse);
            NetworkServer.Listen(hostInfo, 9000);

            NetworkManager.singleton.StartHost(hostInfo);

            text.text += "\n Joined host: " + hostInfo.address + ":" + hostInfo.port;
        }
        else
        {
            text.text += "\n Match creation failed";
        }
    }

    //call this method to find a match through the matchmaker
    public void FindInternetMatch(string matchName)
    {
        NetworkManager.singleton.matchMaker.ListMatches(0, 20, matchName, OnInternetMatchList);
    }

    //this method is called when a list of matches is returned
    private void OnInternetMatchList(ListMatchResponse matchListResponse)
    {
        if (matchListResponse.success)
        {
            if (matchListResponse.matches.Count != 0)
            {
                text.text += "\n A list of matches was returned";

                //join the last server (just in case there are two...)
                NetworkManager.singleton.matchMaker.JoinMatch(matchListResponse.matches[matchListResponse.matches.Count - 1].networkId, "", OnJoinInternetMatch);
            }
            else
            {
                text.text += "\n No matches in requested room.";
            }
        }
        else
        {
            text.text += "\n Couldn't connect to the match maker.";
        }
    }

    //this method is called when your request to join a match is returned
    private void OnJoinInternetMatch(JoinMatchResponse matchJoin)
    {
        if (matchJoin.success)
        {
            text.text += "\n Joining the match";


            MatchInfo hostInfo = new MatchInfo(matchJoin);
            NetworkManager.singleton.StartClient(hostInfo);
           
            text.text += "\n Joined match: " + hostInfo.address + ":" + hostInfo.port;
            if (!CONSTANTS.IS_HOST)
            {
                text.text += "\n Ran as false.";
                NetworkManager.singleton.client.RegisterHandler(1000, CallThisWhenReceiveMessage);
                //NetworkServer.RegisterHandler(1000, CallThisWhenReceiveMessage);
                GameObject newPanel = Instantiate(Resources.Load("ChatPanel")) as GameObject;
                newPanel.transform.SetParent(mainCanvas.transform, false);
                newPanel.GetComponent<PanelScript>().name.text = "Host chat";
                newPanel.GetComponent<PanelScript>().connId = 0;
            }
        }
        else
        {
            text.text += "\n Failed Joining the match";
        }
    }

    public void SendMessage()
    {
        text.text += "\n Sending message.";
        short DEFAULT_MESSAGE = 1000;
        Msg message = new Msg();
        message.message = "This is a test";
        NetworkManager.singleton.client.Send(DEFAULT_MESSAGE, message);
    }
}