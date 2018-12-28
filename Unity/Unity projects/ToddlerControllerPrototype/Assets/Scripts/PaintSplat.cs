using UnityEngine;
using System.Collections;

public class PaintSplat : MonoBehaviour {
	

		
	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
		GetComponent<SpriteRenderer> ().sortingOrder = CONSTANTS.layerCounter;
		transform.SetParent (CONSTANTS.splatParent);
		CONSTANTS.layerCounter++;
	}

	public void SetColor(Color color)
	{
		GetComponent<SpriteRenderer> ().color = color;
	}

}
