using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{
    Transform target;
    Vector3 currentVelocity;

    float targetVelocityMod = .5f;

    void FixedUpdate()
    {
        if (!target && GameManager.Instance.Player)
        {
            target = GameManager.Instance.Player.transform;
        }
        else if (target)
        {
            Vector3 newPos = Vector3.SmoothDamp(GetComponent<Rigidbody2D>().position, target.position + (Vector3)target.GetComponent<Rigidbody2D>().velocity * targetVelocityMod, ref currentVelocity, .25f);
            newPos.z = -10;
            GetComponent<Rigidbody2D>().MovePosition(newPos);
        }
    }
}
