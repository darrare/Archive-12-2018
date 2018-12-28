using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour {
    static ObjectiveScript instance;
    Rigidbody rBody;
    AudioSource audioSource;
    float volumeMin = 0, volumeMax = 1;
    float pitchMin = 1, pitchMax = 2;

    [SerializeField]
    AudioClip clank;

    public static ObjectiveScript Instance
    {
        get { return instance; }
    }

    public List<GravitationalBodyScript> GravitationalBodies
    { get; set; }

	// Use this for initialization
	void Awake ()
    {
        instance = this;
        rBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        GravitationalBodies = new List<GravitationalBodyScript>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        audioSource.volume = Mathf.Lerp(audioSource.volume, Mathf.Clamp(rBody.velocity.magnitude / 15, volumeMin, volumeMax), Time.deltaTime * 5);
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, Mathf.Clamp(1 + (rBody.velocity.magnitude / 15), pitchMin, pitchMax), Time.deltaTime * 5);
		foreach (GravitationalBodyScript g in GravitationalBodies)
        {
            rBody.velocity += g.GetPullEffector(transform.position) * Time.deltaTime;
        }
	}

    /// <summary>
    /// Called whenever this object collides with another object
    /// </summary>
    /// <param name="col">The collision that occurs</param>
    void OnCollisionEnter(Collision col)
    {
        //Play clank sound effect here
        audioSource.PlayOneShot(clank);
    }
}
