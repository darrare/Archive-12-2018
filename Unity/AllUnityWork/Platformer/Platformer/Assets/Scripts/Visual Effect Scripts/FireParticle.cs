using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticle : MonoBehaviour
{
    Vector3 velocity;
    float lifeSpan;
    Color startingColor;
    Color endingColor;
    SpriteRenderer spriteRenderer;

    public FireParticleEffect effector;

	// Use this for initialization
	void Start ()
    {
        lifeSpan = Random.Range(.3f, .7f);
        velocity = new Vector3(Random.Range(-.1f, .1f), Random.Range(.2f, .4f));
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingColor = spriteRenderer.color;
        endingColor = new Color(startingColor.r, startingColor.g, startingColor.b, 0);
    }

	// Update is called once per frame
	void Update ()
    {
        spriteRenderer.color = Color.Lerp(endingColor, startingColor, 2 / (1 / lifeSpan));
        lifeSpan -= Time.deltaTime;
        
        if (lifeSpan <= 0)
        {
            lifeSpan = Random.Range(.3f, .7f);
            effector.ReuseOldFireParticle(this);
            return;
        }
        transform.position += velocity * Time.deltaTime;
        //transform.Translate(velocity * Time.deltaTime); //this is worse on the profiler
	}
}
