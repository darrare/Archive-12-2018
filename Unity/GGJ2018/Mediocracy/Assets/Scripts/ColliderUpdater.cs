using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderUpdater : MonoBehaviour
{
  public Vector2 scale, offset;
  public Vector2 otherScale, otherOffset;
  public bool useSecondVectors = false;

  BoxCollider2D col;
  public void UpdateCollider()
  {
    if (col == null)
    {
      col = gameObject.GetComponent<BoxCollider2D>();
    }

    if (useSecondVectors)
    {
      col.offset = otherOffset;
      col.size = otherScale;
    }
    else
    {
      col.offset = offset;
      col.size = scale;
    }
  }
}
