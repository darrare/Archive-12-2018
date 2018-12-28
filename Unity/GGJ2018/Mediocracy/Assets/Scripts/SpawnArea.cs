using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnArea : MonoBehaviour
{
  [SerializeField] bool visualizeBounds;
  [SerializeField] Color visCol = Color.red;
  public Vector3 bounds;
  private void OnDrawGizmos()
  {
    if (visualizeBounds)
    {
      Gizmos.color = visCol;
      Gizmos.DrawCube(transform.position, bounds);
    }
  }
}
