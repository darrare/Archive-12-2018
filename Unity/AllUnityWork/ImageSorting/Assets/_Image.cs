using UnityEngine;
using System.Collections;

public class _Image {//: MonoBehaviour {
	Sprite imageSprite;
	float _r = -1;
	float _g = -1;
	float _b = -1;

	public _Image(Sprite sprite)
	{
		imageSprite = sprite;

	}

	public Sprite sprite()
	{
		return imageSprite;
	}

	public float r()
	{
		if (_r == -1) {
			_r = AverageAlgorithm.GetAverageR (imageSprite);
		}
		return _r;
	}
	public float g()
	{
		if (_g == -1) {
			_g = AverageAlgorithm.GetAverageG (imageSprite);
		}
		return _g;
	}
	public float b()
	{
		if (_b == -1) {
			_b = AverageAlgorithm.GetAverageB (imageSprite);
		}
		return _b;
	}

}
