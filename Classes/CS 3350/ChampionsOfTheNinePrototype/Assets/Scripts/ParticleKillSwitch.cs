using UnityEngine;
using System.Collections;

public class ParticleKillSwitch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("KillSwitch", 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void KillSwitch()
	{
		Destroy (this.gameObject);
	}
}
