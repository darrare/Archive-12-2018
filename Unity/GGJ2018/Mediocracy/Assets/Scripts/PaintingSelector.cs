using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingSelector : MonoBehaviour
{
  [SerializeField] Sprite[] sprs;
  public int spriteValue = 0;
  public int SpriteValue
  {
    get
    {
      return spriteValue;
    }

    set
    {
      spriteValue = value;
    }
  }

  void Awake()
  {
    float rVal = Random.value;
    float techThresh = .15f + (.0025f * ((transform.position.y) * (transform.position.y) * .01f));
    if (rVal < techThresh){
      spriteValue = 2;
    } else {
      spriteValue = Random.Range(0, sprs.Length);
    }
    GetComponent<SpriteRenderer>().sprite = sprs[spriteValue];
  }

  public void ChangeSprite(int v)
  {
    SpriteValue = v;
    GetComponent<SpriteRenderer>().sprite = sprs[v];
    CheckIfRouter();
  }

  public bool CheckIfRouter()
  {
    return spriteValue == 2;
  }
}
