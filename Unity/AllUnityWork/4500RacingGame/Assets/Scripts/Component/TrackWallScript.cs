using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackWallScript : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HealthScript>())
        {
            collision.gameObject.GetComponent<HealthScript>().TakeDamage(collision.relativeVelocity.magnitude * .05f);
        }
    }

}
