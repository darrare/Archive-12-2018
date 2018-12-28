using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilderCameraScript : MonoBehaviour
{
    /// <summary>
    /// The camera attached to this script
    /// </summary>
    public Camera Camera
    { get; private set; }

    /// <summary>
    /// Cameras rigid body
    /// </summary>
    public Rigidbody rBody
    { get; private set; }

	// Use this for initialization
	void Awake ()
    {
        Camera = GetComponent<Camera>();
        rBody = GetComponent<Rigidbody>();
        LevelBuilderManager.Instance.MainCam = this;
	}
	
	/// <summary>
    /// Updates the level builder camera
    /// </summary>
	void Update ()
    {
        if (rBody.velocity.magnitude <= .03f)
        {
            rBody.velocity = Vector3.zero;
        }
        else
        {
            rBody.velocity *= .95f;
        }
	}
}
