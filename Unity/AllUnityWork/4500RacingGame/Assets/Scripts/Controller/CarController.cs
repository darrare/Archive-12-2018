using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CarController : NetworkBehaviour
{
    float maxSpeedMagnitude = 3;
    float curSpeedMod = 0;
    float turnMod = 150;
    float accelMod = .15f;

    Rigidbody2D rbody;

    private void Start()
    {
        if (isLocalPlayer)
        {
            GameManager.Instance.Player = this;
        }
        rbody = GetComponent<Rigidbody2D>();
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public HealthScript Health
    { get { return GetComponent<HealthScript>(); } }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (!isLocalPlayer)
            return;

        Vector2 newSpeed = Vector2.zero;

        newSpeed = new Vector2(Input.GetAxisRaw("Vertical"), 0);

        if (newSpeed.x == 0)
        {
            rbody.velocity *= .95f;
        }


        //transform.Rotate(Vector3.forward, ;
        //transform.Rotate(0, 0, Input.GetAxisRaw("Horizontal") * -turnMod * Time.deltaTime);
        rbody.rotation = rbody.rotation + Input.GetAxisRaw("Horizontal") * -turnMod * Time.deltaTime;

        newSpeed = Utilities.RotateDegrees(newSpeed, transform.eulerAngles.z);
        rbody.velocity = Vector2.ClampMagnitude(rbody.velocity + newSpeed * accelMod, maxSpeedMagnitude);
    }
}
