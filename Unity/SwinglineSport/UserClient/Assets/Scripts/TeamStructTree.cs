using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamStructTree
{
    #region accessors and mutators

    /// <summary>
    /// The dictionary of teams
    /// </summary>
    public Dictionary<int, List<TeamStruct>> Teams
    { get; set; }

    #endregion

    public int leftOvers;

    #region constructors

    /// <summary>
    /// Generates the tree based on the initial teams signed up for the event
    /// </summary>
    /// <param name="teams">list of teams signed up for the event</param>
    public TeamStructTree(List<TeamStruct> teams)
    {
        Teams = new Dictionary<int, List<TeamStruct>>();

        //calculate the amount of levels and leftover teams when /8.
        int numTeams = teams.Count;
        int firstRowCount = 1;
        int tiers = 1; //always account for the leftover tier

        while ((firstRowCount * 2) <= numTeams)
        {
            firstRowCount *= 2;
        }

        leftOvers = numTeams - firstRowCount;

        int temp = firstRowCount;
        while (temp >= 1)
        {
            temp /= 2;
            tiers++;
        }

        //Generate the overflow
        List<TeamStruct> toAdd = new List<TeamStruct>();
        for (int i = 0; i < leftOvers; i++)
        {
            toAdd.Add(teams[0]);
            teams.RemoveAt(0);
        }
        Teams.Add(0, new List<TeamStruct>(toAdd));

        //Generate the first row of the dictionary
        toAdd.Clear();
        for (int i = 0; i < firstRowCount; i++)
        {
            toAdd.Add(teams[0]);
            teams.RemoveAt(0);
        }
        Teams.Add(1, new List<TeamStruct>(toAdd));
        numTeams /= 2;

        //Generate the empty part of the tree
        for(int j = 2; j < tiers; j++)
        {
            toAdd.Clear();
            for (int i = 0; i < numTeams; i++)
            {
                toAdd.Add(new TeamStruct(" ", " ", " ", " "));
            }
            numTeams /= 2;
            Teams.Add(j, new List<TeamStruct>(toAdd));
        }
    }


    /// <summary>
    /// Default constructor for the tree.
    /// </summary>
    /// <example> TeamStructTree myTree = new TeamStructTree(); myTree = copyTree;</example>
    public TeamStructTree(Dictionary<int, List<TeamStruct>> teams)
    {
        Teams = teams;
    }

    #endregion

    #region public methods

    #endregion 
}


