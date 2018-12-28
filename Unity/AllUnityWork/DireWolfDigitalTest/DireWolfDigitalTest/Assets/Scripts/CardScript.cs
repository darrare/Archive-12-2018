using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    Vector2 startPos, endPos;
    float lerpTimer = 0;
    float timeToLerp = 1.5f;

    /// <summary>
    /// The card data associated with this prefab
    /// </summary>
    public Card MyCard
    { get; private set; }

    /// <summary>
    /// The hand this card is currently a part of
    /// </summary>
    public Hand MyHand
    { get; private set; }

    /// <summary>
    /// The sprite that this object needs to use
    /// </summary>
    public Sprite MySprite
    { get; private set; }

	// Use this for initialization
	public void Instantiate (Card card, Hand hand, Vector2 startPos, Vector2 endPos)
    {
        MyCard = card;
        MyHand = hand;
        MySprite = GameManager.Instance.CardSprites[card.Suit][card.Value];
        GetComponent<SpriteRenderer>().sprite = MySprite;
        transform.position = startPos;
        this.startPos = startPos;
        this.endPos = endPos;
        StartCoroutine(LerpToPosition());
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    /// <summary>
    /// Causes the card to slowly lerp to its desired location from its starting location
    /// </summary>
    /// <returns></returns>
    IEnumerator LerpToPosition()
    {
        for (;;)
        {
            lerpTimer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, endPos, lerpTimer / timeToLerp);
            if (lerpTimer >= timeToLerp)
            {
                MyHand.CardAddedToHand(this);
                yield break;
            }
            yield return null;
        }
    }
}
