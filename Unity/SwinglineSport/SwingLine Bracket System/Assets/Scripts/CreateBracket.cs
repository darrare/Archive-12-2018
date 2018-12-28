using UnityEngine;
using System.Collections;

public class CreateBracket : MonoBehaviour {

	enum Description {Top, Middle, Bottom};
	Description description = Description.Bottom;
	int numberOfTeams;
	int firstRowCount = 1;
	int leftOvers = 0;
	int tiers = 0;
	int nameIndex = 1;
	float height;
	float width;
	float componentHeight;
	float componentWidth;
	int remainingTeams;
	RectTransform myRect;
	GameObject canvas;

	// Use this for initialization
	void Start () {

		canvas = GameObject.Find ("Canvas");
		numberOfTeams = InputScreenControl.teams.Count;
		height = GameObject.Find ("Canvas").GetComponent<RectTransform> ().rect.height;
		width = GameObject.Find ("Canvas").GetComponent<RectTransform> ().rect.width;

		CalculateBracket ();
	}

	void CalculateBracket()
	{
		//numberOfTeams = 28; //USED TO DEBUG
		while ((firstRowCount * 2) <= numberOfTeams) {
			firstRowCount *= 2;
		}
		leftOvers = numberOfTeams - firstRowCount;
		int temp = firstRowCount;
		while (temp > 1) {
			temp /= 2;
			tiers++;
		}
		Debug.Log ("NumTeams: " + numberOfTeams);
		Debug.Log ("FirstRowCount: " + firstRowCount);
		Debug.Log ("Left overs: " + leftOvers);
		Debug.Log ("tiers: " + tiers);
		BRACKET.numberOfTeams = numberOfTeams;
		BRACKET.firstRowCount = firstRowCount;
		BRACKET.leftOvers = leftOvers;
		BRACKET.tiers = tiers;
		componentHeight = height / (firstRowCount * 2);
		componentWidth = componentHeight * 2;
		DrawBracket ();
	}

	void DrawBracket()
	{
		GameObject.Find ("NetManager").GetComponent<NetManager>().ReturnBracketInfo ();
		int amountDrawn = 0;
		int scalar = 1;
		bool topBracket = true;
		int distance = 1;

		for (int i = 0; i < leftOvers * 2; i++) 
		{
			if (topBracket)
			{
				GameObject topObject = Instantiate (Resources.Load("Top")) as GameObject;
				topObject.name = nameIndex.ToString ();
				topObject.transform.SetParent (canvas.transform);
				myRect = topObject.GetComponent<RectTransform>();
				myRect.sizeDelta = new Vector2(componentWidth, componentHeight);
				myRect.anchoredPosition = new Vector2((-width / 2) + (componentWidth * .5f), (height/2) - (componentHeight * amountDrawn) - (componentHeight / 2));
			}
			else
			{
				GameObject bottomObject = Instantiate (Resources.Load ("Bottom")) as GameObject;
				bottomObject.name = nameIndex.ToString ();
				bottomObject.transform.SetParent (canvas.transform);
				myRect = bottomObject.GetComponent<RectTransform>();
				myRect.sizeDelta = new Vector2(componentWidth, componentHeight);
				myRect.anchoredPosition = new Vector2((-width / 2) + (componentWidth * .5f), (height/2) - (componentHeight * amountDrawn) - (componentHeight / 2));
			}
			topBracket = !topBracket;
			amountDrawn++;
			nameIndex++;
		}
		topBracket = false;
		amountDrawn = 1;
		for (int i = 0; i < tiers; i++) 
		{
			for (int j = 0; j < firstRowCount / scalar; j++)
			{
				if (topBracket)
				{
					GameObject topObject = Instantiate (Resources.Load("Top")) as GameObject;
					topObject.name = nameIndex.ToString ();
					topObject.transform.SetParent (canvas.transform);
					myRect = topObject.GetComponent<RectTransform>();
					myRect.sizeDelta = new Vector2(componentWidth, componentHeight);
					if (distance % 2 == 0)
					{
						myRect.anchoredPosition = new Vector2((-width / 2) + (componentWidth * 1.5f) + (i * componentWidth), (-height/2) + (componentHeight * amountDrawn) - (componentHeight / 2));
					}
					else
					{
						myRect.anchoredPosition = new Vector2((-width / 2) + (componentWidth * 1.5f) + (i * componentWidth), (-height/2) + (componentHeight * amountDrawn));
					}
					//draw empty
					for (int k = 0; k < distance; k++)
					{
						amountDrawn++;
					}
				}
				else if (!topBracket)
				{
					GameObject bottomObject = Instantiate (Resources.Load ("Bottom")) as GameObject;
					bottomObject.name = nameIndex.ToString ();
					bottomObject.transform.SetParent (canvas.transform);
					myRect = bottomObject.GetComponent<RectTransform>();
					myRect.sizeDelta = new Vector2(componentWidth, componentHeight);
					if (distance % 2 == 0)
					{
						myRect.anchoredPosition = new Vector2((-width / 2) + (componentWidth * 1.5f) + (i * componentWidth), (-height/2) + (componentHeight * amountDrawn) - (componentHeight / 2));
					}
					else
					{
						myRect.anchoredPosition = new Vector2((-width / 2) + (componentWidth * 1.5f) + (i * componentWidth), (-height/2) + (componentHeight * amountDrawn));
					}
					//draw middle
					for (int k = 0; k < distance; k++)
					{
						amountDrawn++;
						GameObject middleObject = Instantiate (Resources.Load ("Middle")) as GameObject;
						middleObject.name = "MiddlePiece";
						middleObject.transform.SetParent (canvas.transform);
						myRect = middleObject.GetComponent<RectTransform>();
						myRect.sizeDelta = new Vector2(componentWidth, componentHeight);
						if (distance % 2 == 0)
						{
							myRect.anchoredPosition = new Vector2((-width / 2) + (componentWidth * 1.5f) + (i * componentWidth), (-height/2) + (componentHeight * amountDrawn) - (componentHeight / 2));
						}
						else
						{
							myRect.anchoredPosition = new Vector2((-width / 2) + (componentWidth * 1.5f) + (i * componentWidth), (-height/2) + (componentHeight * amountDrawn));
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
		}
		//draws the winner line
		GameObject winner = Instantiate (Resources.Load("Winner")) as GameObject;
		winner.name = nameIndex.ToString ();
		winner.transform.SetParent (canvas.transform);
		myRect = winner.GetComponent<RectTransform>();
		myRect.sizeDelta = new Vector2(componentWidth, componentHeight);
		myRect.anchoredPosition = new Vector2((-width / 2) + (componentWidth * 1.5f) + (tiers * componentWidth), (-height/2) + (componentHeight * amountDrawn));
	}
}
