using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    Sprite[] sprites;
    List<_Sprite> mySprites;
    Texture2D source;

    [SerializeField]
    Button loadImages, generate;

    [SerializeField]
    RawImage img;

    GameObject prefab;

    void Start()
    {
        Application.runInBackground = true;
        generate.gameObject.SetActive(false);
        loadImages.gameObject.SetActive(true);
        prefab = Resources.Load<GameObject>("SpritePrefab");
    }

    public void LoadImages()
    {
        sprites = Resources.LoadAll<Sprite>("Library");
        Debug.Log(sprites.Length + " Images Loaded");

        mySprites = new List<_Sprite>();
        foreach (Sprite sprite in sprites)
        {
            mySprites.Add(new _Sprite(sprite, GetAverageColor(sprite)));
        }

        source = Resources.LoadAll<Texture2D>("Source")[0];
        Debug.Log("Source Loaded");

        img.texture = source;
        img.SetNativeSize();

        generate.gameObject.SetActive(true);
        loadImages.gameObject.SetActive(false);
    }

    public void GenerateImage()
    {
        //img.gameObject.SetActive(false);

        for (int x = Constants.SIZE_OF_PIXEL_CLUSTER; x < source.width; x += Constants.SIZE_OF_PIXEL_CLUSTER)
        {
            for (int y = Constants.SIZE_OF_PIXEL_CLUSTER; y < source.height; y += Constants.SIZE_OF_PIXEL_CLUSTER)
            {
                Sprite mostCommon = FindMostCommonImage(GetAverageColorInCluster(x, y));
                GameObject newObj = Instantiate(prefab, new Vector3(x * Constants.SPRITE_SPACER_MODIFIER, y * Constants.SPRITE_SPACER_MODIFIER, 0), Quaternion.identity) as GameObject;
                newObj.GetComponent<SpriteRenderer>().sprite = mostCommon;
            }
        }
        generate.gameObject.SetActive(false);
        loadImages.gameObject.SetActive(false);
    }

    /// <summary>
    /// Gets the average color of the cluster
    /// </summary>
    /// <param name="x">the x center of the cluster</param>
    /// <param name="y">the y center of the cluster</param>
    /// <returns></returns>
    Color GetAverageColorInCluster(int x, int y)
    {
        Color sum = Color.black;
        int count = 0;
        int xMin = x - Constants.SIZE_OF_PIXEL_CLUSTER;
        int xMax = x + Constants.SIZE_OF_PIXEL_CLUSTER;
        int yMin = y - Constants.SIZE_OF_PIXEL_CLUSTER;
        int yMax = y + Constants.SIZE_OF_PIXEL_CLUSTER;

        //Bounds check
        if (xMin < 0)
        { xMin = 0; }
        if (xMax > source.width - 1)
        { xMax = source.width - 1; }
        if (yMin < 0)
        { yMin = 0; }
        if (yMax > source.height - 1)
        { yMax = source.height - 1; }


        for (int i = xMin; i < xMax; i++)
        {
            for (int j = yMin; j < yMax; j++)
            {
                sum += source.GetPixel(i, j);
                count++;
            }
        }
        sum /= count;
        return sum;
    }

    /// <summary>
    /// Calculates the image that is most common to the color specified as a parameter
    /// </summary>
    /// <param name="colorToMatch">The color we are trying to match</param>
    /// <returns>The sprite that most resembles the color</returns>
    Sprite FindMostCommonImage(Color colorToMatch)
    {
        float smallestDifference = Mathf.Infinity;
        Sprite smallestDifferenceSprite = sprites[0];

        float tempDiff;

        foreach (_Sprite sprite in mySprites)
        {
            tempDiff = DifferenceOfTwoColors(colorToMatch, sprite.AverageColor);
            if (tempDiff < smallestDifference)
            {
                smallestDifference = tempDiff;
                smallestDifferenceSprite = sprite.TheSprite;
            }
        }
        return smallestDifferenceSprite;
    }

    /// <summary>
    /// Finds the difference of two colors
    /// </summary>
    /// <param name="first">The first color</param>
    /// <param name="second">The second color</param>
    /// <returns>The value of difference</returns>
    float DifferenceOfTwoColors(Color first, Color second)
    {
        return Mathf.Sqrt((Mathf.Pow(second.r - first.r, 2) + Mathf.Pow(second.g - first.g, 2) + Mathf.Pow(second.b - first.b, 2)));
        //return Vector2.Distance(ColorToVector3(first), ColorToVector3(second));
    }

    /// <summary>
    /// Gets the average color of a sprite
    /// </summary>
    /// <param name="sprite">Sprite to get the average color of</param>
    /// <returns>The average color of the sprite</returns>
    Color GetAverageColor(Sprite sprite)
    {
        Color sum = Color.black;
        int count = 0;
        int _x = sprite.texture.width;
        int _y = sprite.texture.height;
        for (int x = 0; x < _x; x++)
        {
            for (int y = 0; y < _y; y++)
            {
                //If the pixel is not transparent
                if (sprite.texture.GetPixel(x, y).a > 0)
                {
                    sum += sprite.texture.GetPixel(x, y);
                    count++;
                }
                else
                {
                    //sum += Camera.main.backgroundColor;
                }
                //count++;
            }
        }
        sum /= count;
        return sum;
    }

    /// <summary>
    /// Converts a color to a vector3
    /// </summary>
    /// <param name="col">Color to convert</param>
    /// <returns>Vector that represents the color</returns>
    Vector3 ColorToVector3(Color col)
    {
        return new Vector3(col.r, col.g, col.b);
    }

    /// <summary>
    /// Converts a vector3 to a color
    /// </summary>
    /// <param name="vec">Vector3 to convert</param>
    /// <returns>Color represented by the vector3</returns>
    Color Vector3ToColor(Vector3 vec)
    {
        return new Color(vec.x, vec.y, vec.z);
    }

    float GetColorAdditive(Color col)
    {
        return col.r + col.b + col.g;
    }
}
