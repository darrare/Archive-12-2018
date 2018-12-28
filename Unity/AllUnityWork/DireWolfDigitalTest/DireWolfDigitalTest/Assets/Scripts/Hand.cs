using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    List<CardScript> hand = new List<CardScript>();
    Vector2[] originalPositions, goalPositions;
    float lerpTimer = 0;
    float timeToLerp = .75f;


	/// <summary>
    /// Instantiate the hand
    /// </summary>
	void Start ()
    {
        GameManager.Instance.PlayerHand = this;
        StartCoroutine(DrawStartingHand());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// Called when a card has been added to the hand, and the hand needs to reorganize itself
    /// </summary>
    public void CardAddedToHand(CardScript card)
    {
        hand.Add(card);
        SortHand();

        //Records the cards original positions so that we can lerp them smoothly
        originalPositions = new Vector2[hand.Count];
        for (int i = 0; i < hand.Count; i++)
        {
            originalPositions[i] = hand[i].transform.position;
        }

        //Records the positions that we want the cards to go to
        goalPositions = new Vector2[hand.Count];
        for (int i = 0; i < hand.Count; i++)
        {
            goalPositions[i] = new Vector2( //Although this is incredibly ugly, it gives me the exact position I need
                ((GameManager.Instance.MainCam.transform.position.x + GameManager.Instance.MainCam.orthographicSize * GameManager.Instance.MainCam.aspect) - (hand[i].MySprite.texture.width / 2) / hand[i].MySprite.pixelsPerUnit) - (hand[i].MySprite.texture.width / hand[i].MySprite.pixelsPerUnit) * i,
                (GameManager.Instance.MainCam.transform.position.y + GameManager.Instance.MainCam.orthographicSize) - (hand[i].MySprite.texture.height / 2) / hand[i].MySprite.pixelsPerUnit);
        }
        StartCoroutine(ReorganizeHand());
    }

    /// <summary>
    /// Sorts the hand based on certain rules. 
    /// This is an ugly O(n^2) algorithm, but for a count that is typically going to be around 5 it won't cause any issues.
    /// This algorithm assumes that an Ace of Spades has the highest value (goes as far right as it can, lower index in list) 
    /// and a Two of Hearts has the lowest value (left as far as it can, higher index in list)
    /// </summary>
    public void SortHand()
    {
        List<CardScript> newList = new List<CardScript>();
        int count = hand.Count;
        for (int i = 0; i < count; i++)
        {
            CardScript max = hand[0];
            for (int j = 1; j < hand.Count; j++)
            {
                if (hand[j].MyCard > max.MyCard) //overloaded > operator so I can compare "Cards"
                {
                    max = hand[j];
                }
            }
            newList.Add(max);
            hand.Remove(max);
        }
        hand = newList;
    }

    /// <summary>
    /// Draws the starting hand.
    /// </summary>
    IEnumerator DrawStartingHand()
    {
        for (int i = 0; i < Modifiers.CARDS_TO_DRAW[Modifiers.IS_STATIC_MODE]; i++)
        {
            //instantiate a new card at the lower left side of the screen (outside camera)
            GameObject newCard = Instantiate(Resources.Load<GameObject>("Prefabs/Card"), transform, false);
            newCard.GetComponent<CardScript>().Instantiate(GameManager.Instance.DrawCard(), this, GameManager.Instance.CardSpawningPosition, transform.position);

            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// Organizes your hand so they are in order
    /// </summary>
    /// <returns></returns>
    IEnumerator ReorganizeHand()
    {
        for(;;)
        {
            lerpTimer += Time.deltaTime;
            for(int i = 0; i < hand.Count; i++)
            {
                hand[i].transform.position = Vector2.Lerp(originalPositions[i], goalPositions[i], lerpTimer / timeToLerp);
            }
            if (lerpTimer >= timeToLerp)
            {
                lerpTimer = 0;
                yield break;
            }
            yield return null;
        }
    }
}
