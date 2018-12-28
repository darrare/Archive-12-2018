using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HarpoonScript : AttackItemScript
{
    const float speedConst = 1f;

    public override void Initialize(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        GetComponent<Rigidbody2D>().velocity = direction * speedConst;
        transform.eulerAngles = Vector3.forward * Utilities.EulerAnglesLookInDirection(direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("We should do something here.");
    }
}
