using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSpecifier : MonoBehaviour
{
  [SerializeField] Sprite[] spriteSet;
  [SerializeField] bool AdjustSortingNumber;
  [SerializeField] int[] layerOrder;
  [SerializeField] List<int> validValuesForTransmitter;
  public int curVal = 0;

  public void SetSprite(int ind)
  {
    transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = spriteSet[ind];
  }

  public void SetSpriteRandomly(List<int> exVals)
  {
    int value = Random.Range(0, spriteSet.Length);
    while (exVals.Contains(value))
    {
      value = Random.Range(0, spriteSet.Length);
    }

    float rVal = Random.value;
    float techThresh = .15f + (.0025f * (transform.position.y * transform.position.y * .01f));
    if (rVal < techThresh)
    {
      value = Mathf.RoundToInt(Random.value);
    }
    else
    {
      while (exVals.Contains(value))
      {
        value = Random.Range(0, spriteSet.Length);
      }
    }

    transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = spriteSet[value];
    curVal = value;

    if (AdjustSortingNumber)
    {
      transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = layerOrder[curVal];
    }
    
  }

  public bool CheckIfTransposer()
  {
    if (validValuesForTransmitter.Contains(curVal) && curVal == 1)
    {
      GetComponent<ColliderUpdater>().useSecondVectors = true;
    }
    return validValuesForTransmitter.Contains(curVal);
  }
}
