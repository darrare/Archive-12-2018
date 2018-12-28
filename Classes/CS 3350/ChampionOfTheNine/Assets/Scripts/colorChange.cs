using UnityEngine;
using System.Collections;

public class colorChange : MonoBehaviour {
	float red = 0;
	float green = 0;
	float blue = 0;
	Renderer rend;

	void Start() {
		rend = GetComponent<Renderer>();
	}

	public void Red(float newRed)
	{
		red = newRed;
		rend.material.color = new Color (red, green, blue);
	}

	public void Green(float newGreen)
	{
		green = newGreen;
		rend.material.color = new Color (red, green, blue);
	}

	public void Blue(float newBlue)
	{
		blue = newBlue;
		rend.material.color = new Color (red, green, blue);
	}
}
