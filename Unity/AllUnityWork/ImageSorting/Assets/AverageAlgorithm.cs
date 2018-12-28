using UnityEngine;
using System.Collections;

public static class AverageAlgorithm {
	
	public static float GetAverageR(Sprite sprite)
	{
		float sum = 0;
		int count = 0;
		for (int x = 0; x < 64; x++) {
			for (int y = 0; y < 64; y++) {
				if (sprite.texture.GetPixel (x, y).a > 0)
				{
					sum += sprite.texture.GetPixel (x, y).r;
					count++;
				}
			}
		}
		return sum / count;
	}
	public static float GetAverageG(Sprite sprite)
	{
		float sum = 0;
		int count = 0;
		for (int x = 0; x < 64; x++) {
			for (int y = 0; y < 64; y++) {
				if (sprite.texture.GetPixel (x, y).a > 0)
				{
					sum += sprite.texture.GetPixel (x, y).g;
					count++;
				}
			}
		}
		return sum / count;
	}	
	public static float GetAverageB(Sprite sprite)
	{
		float sum = 0;
		int count = 0;
		for (int x = 0; x < 64; x++) {
			for (int y = 0; y < 64; y++) {
				if (sprite.texture.GetPixel (x, y).a > 0)
				{
					sum += sprite.texture.GetPixel (x, y).b;
					count++;
				}
			}
		}
		return sum / count;
	}

}
