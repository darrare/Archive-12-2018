using UnityEngine;
using System.Collections;

public class _Sprite
{
    Sprite sprite;
    Color avgColor;

    public _Sprite(Sprite sprite, Color avgColor)
    {
        this.sprite = sprite;
        this.avgColor = avgColor;
    }

    public Color AverageColor
    {
        get { return avgColor; }
    }

    public Sprite TheSprite
    {
        get { return sprite; }
    }
    
}
