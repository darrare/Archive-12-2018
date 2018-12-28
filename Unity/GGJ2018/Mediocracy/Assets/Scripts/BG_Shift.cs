using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Shift : MonoBehaviour
{
  Vector3 pos;
  int dir = 1;
  float val = 0;
	// Use this for initialization
	void Start () {
    pos = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate ()
  {
    val += Time.deltaTime * dir;
    Vector3 move = Vector3.right * dir;
    move.x = Mathf.Sin(val * .5f * .3f) * 30f;
    transform.position = pos + move;
  }

  
}
