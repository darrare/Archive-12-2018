using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

#region Team Struct
/// <summary>
/// A struct for housing any teams info ncluding team name,
/// player names and what color the team will be displayed as
/// </summary>
public class TeamStruct
{
    // team Info
    public string teamName;
    public string player1;
    public string player2;
    public string player3;

    // TeamStruct constructor
    /// <summary>
    /// Creates a team for tournament display
    /// </summary>
    /// <param name="teamname"></param>
    /// <param name="player1"></param>
    /// <param name="player2"></param>
    /// <param name="player3"></param>
    /// <param name="color"></param>
    public TeamStruct(string teamname, string player1, string player2, string player3)
    {
        teamName = teamname;
        this.player1 = player1;
        this.player2 = player2;
        this.player3 = player3;
    }
}

#endregion

#region Bracket Class
/// <summary>
/// Bracket class used for creating tournament brackets
/// </summary>
public static class BRACKET
{ 
    public static int numberOfTeams;
    public static int firstRowCount;
    public static int leftOvers;
    public static int tiers;
    public static List<TeamStruct> bracketInfo = new List<TeamStruct>();
}
#endregion

#region Bracket Script

public class BracketScript : MonoBehaviour
{

    #region Ryan's Bracket System "Variables"
    // Ryan's work for creating brackets
    private int leftOvers = 0;
    private int tiers = 0;
    private int nameIndex = 1;
    private float cameraHeight;
    private float cameraWidth;
    private RectTransform myRect;
    private GameObject canvas;
    private Vector3 namePosition = new Vector3(10, -35, 0);

    [SerializeField]
    Transform contentHolder;

    private Dictionary<int, List<TeamStruct>> teamDictionary = new Dictionary<int, List<TeamStruct>>();
    // public variables for debugging

    //TeamStructTree teamDictionary;
    List<TeamStruct> myTeams = new List<TeamStruct>();
    List<TeamStruct> leftoverTeam = new List<TeamStruct>();
    List<TeamStruct> tier3 = new List<TeamStruct>();
    List<TeamStruct> tier4 = new List<TeamStruct>();
    List<TeamStruct> tier5 = new List<TeamStruct>();
    List<TeamStruct> tier6 = new List<TeamStruct>();
    #endregion

    #region Start Method
    // Use this for initialization
    void Start()
    {
        //For debugging
        for (int i = 0; i < 14; i++)
        {
            leftoverTeam.Add(new TeamStruct("Team" + i, " ", " ", " "));
        }
        for (int i = 0; i < 8; i++)
        {
            if (i < 7)
            {
                myTeams.Add(new TeamStruct(" ", " ", " ", " "));
            }
            else
            {
                myTeams.Add(new TeamStruct("Team" + 8 + i, " ", " ", " "));
            }
            
        }
        //for (int i = 0; i < 8; i++)
        //{
        //    tier3.Add(new TeamStruct(" ", " ", " ", " "));
        //}
        for (int i = 0; i < 4; i++)
        {
            tier4.Add(new TeamStruct(" ", " ", " ", " "));
        }
        for (int i = 0; i < 2; i++)
        {
            tier5.Add(new TeamStruct(" ", " ", " ", " "));
        }
        for (int i = 0; i < 1; i++)
        {
            tier6.Add(new TeamStruct(" ", " ", " ", " "));
        }
        teamDictionary.Add(0, leftoverTeam);
        teamDictionary.Add(1, myTeams);
        //teamDictionary.Add(2, tier3);
        teamDictionary.Add(2, tier4);
        teamDictionary.Add(3, tier5);
        teamDictionary.Add(4, tier6);

        CONSTANTS.netManager.SendRequestForBracketInfo();

        //end for debugging

        #region Ryan's Bracket System "Start Items"
        // Ryan's work for creating brackets
        canvas = GameObject.Find("Canvas");
        cameraHeight = canvas.GetComponent<Canvas>().GetComponent<RectTransform>().rect.height;
        cameraWidth = canvas.GetComponent<Canvas>().GetComponent<RectTransform>().rect.width;
        cameraHeight = cameraHeight / 25;
        cameraWidth = cameraWidth / 8;

        #endregion
        tiers = teamDictionary.Count - 1;
        leftOvers = teamDictionary[0].Count;
        DrawBracket();
    }

    #endregion

    public void ReceiveBracketData(Dictionary<int, List<TeamStruct>> teams)
    {
        Debug.Log("Do something with this dictionary");
        //teamDictionary = teams;
        tiers = teamDictionary.Count - 1;
        leftOvers = teamDictionary[0].Count;
        //DrawBracket();
    }

    #region Ryan's Bracket System "Methods"
    /// <summary>
    /// Uses info from CalculateBracket to draw tournament bracket
    /// correctly.  Adjusts for number of teams and whether they need
    /// seeds or not
    /// </summary>
    void DrawBracket()
    {
        // initialize drawing parameters
        int amountDrawn = 0;
        int scalar = 1;
        bool topBracket = true;
        int distance = 1;
        int bracketPosition = 0;
        List<TeamStruct> tempList;

        //using premade objects, fill in brackets
        for (int i = 0; i < leftOvers; i++)
        {
            tempList = teamDictionary[bracketPosition];
            if (topBracket)
            {
                GameObject topObject = Instantiate(Resources.Load("Top")) as GameObject;
                topObject.name = nameIndex.ToString();
                topObject.transform.SetParent(contentHolder, false);
                myRect = topObject.GetComponent<RectTransform>();
                myRect.sizeDelta = new Vector2(cameraWidth, cameraHeight);
                myRect.anchoredPosition = new Vector2((-cameraWidth * 2) + (cameraWidth * .2f) - (cameraWidth),
                    (-cameraHeight * 8) + (cameraHeight * amountDrawn) + (cameraHeight * 1.5f));
                if (tempList[i].teamName != " ")
                {
                    //topObject.GetComponentInChildren<Text>().color = Color.white;
                    topObject.GetComponentInChildren<Text>().text = "Zero"/*tempList[i].teamName*/;
                }
            }
            else
            {
                GameObject bottomObject = Instantiate(Resources.Load("Bottom")) as GameObject;
                bottomObject.name = nameIndex.ToString();
                bottomObject.transform.SetParent(contentHolder, false);
                myRect = bottomObject.GetComponent<RectTransform>();
                myRect.sizeDelta = new Vector2(cameraWidth, cameraHeight);
                myRect.anchoredPosition = new Vector2((-cameraWidth * 2) + (cameraWidth  * .2f) - (cameraWidth),
                    (-cameraHeight * 8) + (cameraHeight * amountDrawn) - (cameraHeight * .5f));
                if (tempList[i].teamName != " ")
                {
                    //bottomObject.GetComponentInChildren<Text>().color = Color.white;
                    bottomObject.GetComponentInChildren<Text>().text = tempList[i].teamName;
                }
            }
            topBracket = !topBracket;
            amountDrawn++;
            nameIndex++;
        }
        topBracket = false;
        amountDrawn = 1;
        bracketPosition++;
        for (int i = 0; i < tiers - 1; i++)
        {
            for (int j = 0; j < teamDictionary[bracketPosition].Count; j++)
            {
                tempList = teamDictionary[bracketPosition];
                if (topBracket)
                {
                    GameObject topObject = Instantiate(Resources.Load("Top")) as GameObject;
                    topObject.name = nameIndex.ToString();
                    topObject.transform.SetParent(contentHolder, false);
                    myRect = topObject.GetComponent<RectTransform>();
                    myRect.sizeDelta = new Vector2(cameraWidth, cameraHeight);
                    if (distance % 2 == 0)
                    {
                        myRect.anchoredPosition = new Vector2((-cameraWidth * 2) + (cameraWidth  * .2f) + (i * cameraWidth),
                            (-cameraHeight * 8) + (cameraHeight * amountDrawn));
                    }
                    else
                    {
                        myRect.anchoredPosition = new Vector2((-cameraWidth * 2) + (cameraWidth  * .2f) + (i * cameraWidth),
                            (-cameraHeight * 8) + (cameraHeight * amountDrawn));
                    }
                    if (tempList[j].teamName != " ")
                    {
                        //topObject.GetComponentInChildren<Text>().color = Color.white;
                        topObject.GetComponentInChildren<Text>().text = tempList[j].teamName;
                    }
                    //draw empty
                    for (int k = 0; k < distance; k++)
                    {
                        amountDrawn++;
                    }
                }

                else if (!topBracket)
                {
                    GameObject bottomObject = Instantiate(Resources.Load("Bottom")) as GameObject;
                    bottomObject.name = nameIndex.ToString();
                    bottomObject.transform.SetParent(contentHolder, false);
                    myRect = bottomObject.GetComponent<RectTransform>();
                    myRect.sizeDelta = new Vector2(cameraWidth, cameraHeight);
                    if (distance % 2 == 0)
                    {
                        myRect.anchoredPosition = new Vector2((-cameraWidth * 2) + (cameraWidth  * .2f) + (i * cameraWidth),
                            (-cameraHeight * 8) + (cameraHeight * amountDrawn) /*- (cameraHeight / 2)*/);
                    }
                    else
                    {
                        myRect.anchoredPosition = new Vector2((-cameraWidth * 2) + (cameraWidth  * .2f) + (i * cameraWidth),
                            (-cameraHeight * 8) + (cameraHeight * amountDrawn));
                    }
                    if (tempList[j].teamName != " ")
                    {
                        //bottomObject.GetComponentInChildren<Text>().color = Color.white;
                        bottomObject.GetComponentInChildren<Text>().text = tempList[j].teamName;
                    }
                    //draw middle
                    for (int k = 0; k < distance; k++)
                    {
                        amountDrawn++;
                        GameObject middleObject = Instantiate(Resources.Load("Middle")) as GameObject;
                        middleObject.name = "MiddlePiece";
                        middleObject.transform.SetParent(contentHolder, false);
                        myRect = middleObject.GetComponent<RectTransform>();
                        myRect.sizeDelta = new Vector2(cameraWidth, cameraHeight);
                        if (distance % 2 == 0)
                        {
                            myRect.anchoredPosition = new Vector2((-cameraWidth * 2) + (cameraWidth  * .2f) + (i * cameraWidth),
                                (-cameraHeight * 8) + (cameraHeight * amountDrawn) /*- (cameraHeight / 2)*/);
                        }
                        else
                        {
                            myRect.anchoredPosition = new Vector2((-cameraWidth * 2) + (cameraWidth  * .2f) + (i * cameraWidth),
                                (-cameraHeight * 8) + (cameraHeight * amountDrawn));
                        }
                    }
                }
                amountDrawn++;
                nameIndex++;
                topBracket = !topBracket;
            }
            amountDrawn = 1;
            topBracket = false;
            scalar *= 2;
            for (int j = 0; j < distance; j++)
            {
                amountDrawn++;
            }
            distance = distance + distance + 1;
            bracketPosition++;
        }
        tempList = teamDictionary[teamDictionary.Count - 1];
        //draws the winner line
        GameObject winner = Instantiate(Resources.Load("Winner")) as GameObject;
        winner.name = nameIndex.ToString();
        winner.transform.SetParent(contentHolder, false);
        myRect = winner.GetComponent<RectTransform>();
        myRect.sizeDelta = new Vector2(cameraWidth, cameraHeight);
        myRect.anchoredPosition = new Vector2((-cameraWidth * 2) + (cameraWidth  * .2f) + ((tiers - 1) * cameraWidth),
            (-cameraHeight * 8) + (cameraHeight * amountDrawn));
        if (tempList[0].teamName != " ")
        {
            //winner.GetComponentInChildren<Text>().color = Color.white;
            winner.GetComponentInChildren<Text>().text = tempList[0].teamName;
        }
    }
    #endregion
}

#endregion
