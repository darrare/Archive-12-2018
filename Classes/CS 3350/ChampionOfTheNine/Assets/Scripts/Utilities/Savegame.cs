using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class that holds game save information
/// </summary>
[Serializable]
public class Savegame
{
    #region Fields

    List<KingdomName> kingdoms;
    Dictionary<InputType, InputButton> inputs;
    CharacterType playerType;
    int currentKingdom;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public Savegame(CharacterType playerType)
    {
        // Make kingdom locations
        kingdoms = new List<KingdomName>();
        HashSet<KingdomName> kingdomsAdded = new HashSet<KingdomName>();
        for (int i = 0; i < Constants.NUM_KINGDOMS; i++)
        {
            KingdomName nextName;
            do { nextName = (KingdomName)UnityEngine.Random.Range(0, Constants.NUM_KINGDOMS); }
            while (kingdomsAdded.Contains(nextName));
            kingdoms.Add(nextName);
            kingdomsAdded.Add(nextName);
        }
        inputs = new Dictionary<InputType, InputButton>();
        inputs.Add(InputType.Main, new InputButton(0));
        inputs.Add(InputType.Secondary, new InputButton(1));
        inputs.Add(InputType.Power, new InputButton("e"));
        inputs.Add(InputType.Special, new InputButton("r"));
        this.playerType = playerType;
        currentKingdom = 0;
    }

    #endregion

    #region Properties

    public int CurrentKingdom
    {
        get { return currentKingdom; }
        set { currentKingdom = value; }
    }

    /// <summary>
    /// Gets the saved kingdoms
    /// </summary>
    public List<KingdomName> Kingdoms
    {
        get { return kingdoms; }
    }

    /// <summary>
    /// Gets the player type
    /// </summary>
    public CharacterType PlayerType
    {
        get { return playerType; }
        set { playerType = value; }
    }

    /// <summary>
    /// Gets the inputs dictionary
    /// </summary>
    public Dictionary<InputType, InputButton> Inputs
    { get { return inputs; } }

    #endregion
}
