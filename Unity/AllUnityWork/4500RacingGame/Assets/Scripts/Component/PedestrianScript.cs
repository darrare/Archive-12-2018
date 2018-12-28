using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PedestrianScript : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Inventory>() != null)
        {
            collision.gameObject.GetComponent<Inventory>().PickupItem((ItemType)Random.Range(1, 5));
            Destroy(gameObject);
        }
    }
}
