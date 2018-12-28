using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{ 
    /// <summary>
    /// The available cards from this deck.
    /// </summary>
    public List<Card> AvailableCards
    { get; private set; }

    /// <summary>
    /// Constructor for a deck
    /// </summary>
    public Deck()
    {
        AvailableCards = new List<Card>();
        if (Modifiers.IS_STATIC_MODE)
        {
            AvailableCards.Add(new Card(CardSuit.Diamonds, CardValue.Queen));
            AvailableCards.Add(new Card(CardSuit.Diamonds, CardValue.Nine));
            AvailableCards.Add(new Card(CardSuit.Spades, CardValue.Ace));
            AvailableCards.Add(new Card(CardSuit.Spades, CardValue.Jack));
            AvailableCards.Add(new Card(CardSuit.Hearts, CardValue.King));
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    AvailableCards.Add(new Card((CardSuit)i, (CardValue)j));
                }
            }
        }
    }

    /// <summary>
    /// Takes the top card in the list.
    /// </summary>
    /// <returns>The top card in the list.</returns>
    public Card DrawACard()
    {
        Card returnVal = AvailableCards[AvailableCards.Count - 1];
        AvailableCards.Remove(returnVal);
        return returnVal;
    }

    /// <summary>
    /// Shuffles the deck so all of the cards arent in order
    /// </summary>
    public void ShuffleDeck()
    {
        if (!Modifiers.IS_STATIC_MODE)
        {
            int n = AvailableCards.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                Card value = AvailableCards[k];
                AvailableCards[k] = AvailableCards[n];
                AvailableCards[n] = value;
            }
        }
    }
}
