using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyScript : MonoBehaviour
{
    public float distanceToAggro;
    public float priorityForCam;

    SpriteRenderer sRenderer;

	// Use this for initialization
	void Start ()
    {
        sRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position));
		if (Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position) < distanceToAggro)
        {
            sRenderer.color = Color.red;
            GameManager.Instance.Camera.AddTransformToTargets(transform, priorityForCam);
        }
        else
        {
            sRenderer.color = Color.white;
        }
	}
}
