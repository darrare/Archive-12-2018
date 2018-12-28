using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardSuit { Hearts, Diamonds, Clubs, Spades };
public enum CardValue { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };

public class Card
{
    /// <summary>
    /// The suit of this card
    /// </summary>
    public CardSuit Suit
    { get; private set; }

    /// <summary>
    /// The value of this card
    /// </summary>
    public CardValue Value
    { get; private set; }

    /// <summary>
    /// Constructor for a card
    /// </summary>
    /// <param name="suit">The card's suit</param>
    /// <param name="value">The card's value</param>
    public Card(CardSuit suit, CardValue value)
    {
        Suit = suit;
        Value = value;
    }

    /// <summary>
    /// Overrided > operator
    /// </summary>
    /// <param name="a">First card</param>
    /// <param name="b">Second Card</param>
    /// <returns>If card A is > card B</returns>
    public static bool operator >(Card a, Card b)
    {
        if ((int)a.Suit > (int)b.Suit)
        {
            return true;
        }
        else if ((int)a.Suit < (int)b.Suit)
        {
            return false;
        }

        //This is assuming they have the same suit
        if ((int)a.Value > (int)b.Value)
        {
            return true;
        }
        else if ((int)a.Value < (int)b.Value)
        {
            return false;
        }
        return false; //they are equal
    }

    /// <summary>
    /// Overrided > operator
    /// </summary>
    /// <param name="a">First card</param>
    /// <param name="b">Second Card</param>
    /// <returns>If card A is < card B</returns>
    public static bool operator <(Card a, Card b)
    {
        if ((int)a.Suit < (int)b.Suit)
        {
            return true;
        }
        else if ((int)a.Suit > (int)b.Suit)
        {
            return false;
        }

        //This is assuming they have the same suit
        if ((int)a.Value < (int)b.Value)
        {
            return true;
        }
        else if ((int)a.Value > (int)b.Value)
        {
            return false;
        }
        return false; //they are equal
    }
}
