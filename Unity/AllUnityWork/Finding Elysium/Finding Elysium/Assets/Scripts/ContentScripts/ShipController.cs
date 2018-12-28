using UnityEngine;
using System.Collections;

public class ShipController : PauseableObjectScript
{

	// Use this for initialization
	void Start () {
        base.Initialize();
        StartCoroutine("CheckForOutOfRangePlanets");
	}

    protected override void NotPausedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rbody.velocity = transform.forward * Constants.SHIP_SPEED;
        }
        else
        {
            rbody.velocity = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -3, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 3, 0));
        }
    }


    IEnumerator CheckForOutOfRangePlanets()
    {
        while(true)
        {
            float distance = Constants.DISTANCE_TO_DESPAWN_PLANETS + Vector3.Distance(transform.position, Vector3.zero);
            foreach (Planet p in GameManager.Instance.Planets)
            {
                if (Vector3.Distance(transform.position, p.transform.position) > distance)
                {
                    p.IsEnabled = false;
                }
                else
                {
                    p.IsEnabled = true;
                }
            }
            yield return new WaitForSeconds(.3f);
        }
    }
}
