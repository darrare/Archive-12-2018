using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using Object = UnityEngine.Object;

public class GameManager
{
    #region fields

    static GameManager instance;
    Deck deck;

    #endregion

    #region Singleton and constructor

    /// <summary>
    /// Singleton for the gamemanager class
    /// </summary>
    public static GameManager Instance
    { get { return instance ?? (instance = new GameManager()); } }

    /// <summary>
    /// Private constructor for the game manager
    /// </summary>
    private GameManager()
    {
        deck = new Deck();
        deck.ShuffleDeck();
        MainCam = Camera.main;
        Sprite s = Resources.Load<Sprite>("Art/c02"); //used to calculate the position cards should spawn in
        CardSpawningPosition = new Vector2((MainCam.transform.position.x - MainCam.orthographicSize * MainCam.aspect) - (s.texture.width / 2) / s.pixelsPerUnit, (MainCam.transform.position.y - MainCam.orthographicSize) - (s.texture.height / 2) / s.pixelsPerUnit);

        InitializeSpriteDictionary();

        Object.DontDestroyOnLoad(new GameObject("GMupdater", typeof(Updater)));
    }

    #endregion

    #region properties

    /// <summary>
    /// The players hand
    /// </summary>
    public Hand PlayerHand
    { get; set; }

    /// <summary>
    /// The position cards should spawn from
    /// </summary>
    public Vector2 CardSpawningPosition
    { get; private set; }

    /// <summary>
    /// Easier to access it this way then Camera.main, plus we can change it if we need to through our system.
    /// </summary>
    public Camera MainCam
    { get; private set; }

    /// <summary>
    /// Dictionary of card sprites
    /// </summary>
    public Dictionary<CardSuit, Dictionary<CardValue, Sprite>> CardSprites
    { get; private set; }

    #endregion

    #region methods

    /// <summary>
    /// The games update loop
    /// </summary>
    public void Update()
    {

    }

    /// <summary>
    /// Draws a card from the deck
    /// </summary>
    /// <returns>The card that was drawn</returns>
    public Card DrawCard()
    {
        return deck.DrawACard();
    }

    /// <summary>
    /// Puts all the card sprites into an easy to use dictionary so we can customly load sprites for cards
    /// </summary>
    private void InitializeSpriteDictionary()
    {
        Dictionary<string, Sprite> tempLinearHolder = Resources.LoadAll<Sprite>("Art").ToDictionary(t => t.name);

        CardSprites = new Dictionary<CardSuit, Dictionary<CardValue, Sprite>>
        {
            { CardSuit.Hearts, new Dictionary<CardValue, Sprite>() },
            { CardSuit.Diamonds, new Dictionary<CardValue, Sprite>() },
            { CardSuit.Clubs, new Dictionary<CardValue, Sprite>() },
            { CardSuit.Spades, new Dictionary<CardValue, Sprite>() },
        };

        foreach (KeyValuePair<string, Sprite> s in tempLinearHolder)
        {
            if (s.Key[0] == 'h')
            {
                CardSprites[CardSuit.Hearts].Add((CardValue)(int.Parse(Regex.Match(s.Key, @"\d+").Value) - 2), s.Value);
            }
            if (s.Key[0] == 'd')
            {
                CardSprites[CardSuit.Diamonds].Add((CardValue)(int.Parse(Regex.Match(s.Key, @"\d+").Value) - 2), s.Value);
            }
            if (s.Key[0] == 'c')
            {
                CardSprites[CardSuit.Clubs].Add((CardValue)(int.Parse(Regex.Match(s.Key, @"\d+").Value) - 2), s.Value);
            }
            if (s.Key[0] == 's')
            {
                CardSprites[CardSuit.Spades].Add((CardValue)(int.Parse(Regex.Match(s.Key, @"\d+").Value) - 2), s.Value);
            }
        }
    }

    #endregion

    #region internal updater class

    /// <summary>
    /// Internal updater class
    /// </summary>
    class Updater : MonoBehaviour
    {
        void Update()
        {
            instance.Update();
        }
    }

    #endregion
}
