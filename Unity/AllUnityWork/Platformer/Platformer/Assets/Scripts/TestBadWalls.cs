using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBadWalls : MonoBehaviour
{
    public bool isBadWall = true;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isBadWall && col.collider.tag == "Player")
        {
            GameManager.Instance.Player.DamageTaken(0, col);
        }
    }
}
