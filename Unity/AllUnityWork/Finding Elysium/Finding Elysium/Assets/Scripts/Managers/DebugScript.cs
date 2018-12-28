using UnityEngine;
using System.Collections;

public class DebugScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        #if !UNITY_EDITOR
            Destroy(this);
        #endif
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.G))
        {
            GameManager.Instance.StartNewGame();
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            Time.timeScale += 1;
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            Time.timeScale -= 1;
        }
    }
}
