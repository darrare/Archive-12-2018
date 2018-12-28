using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovingPlatform : MonoBehaviour {
	
	public float travelDistance = 2f;
	float velocityMultiplier = 2f;
	public bool isVertical = true;
	public bool startNormalDirection = true;
	float transportTime = 0f;
	Vector2 origin;
	enum PlatformState { GOINGNEG, GOINGPOS };
	PlatformState platformState = PlatformState.GOINGNEG;

	// Use this for initialization
	void Awake () {
		origin = transform.position;
		if (!startNormalDirection) {
			platformState = PlatformState.GOINGPOS;
		}
	}

	public bool IsVertical
	{
		get { return isVertical; }
		set { isVertical = value; Reset (); }
	}

	public bool NormDirection
	{
		get { return startNormalDirection; }
		set { startNormalDirection = value; Reset (); }
	}

	public float TravelDistance
	{
		get { return travelDistance; }
		set { travelDistance = value; Reset (); }
	}

	void Reset()
	{
		transform.position = origin;
		if (!startNormalDirection) {
			platformState = PlatformState.GOINGPOS;
		} else {
			platformState = PlatformState.GOINGNEG;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale == 0) {
			return;
		}
		if (!isVertical) {
			if (platformState == PlatformState.GOINGNEG) {
				transportTime += Time.deltaTime / travelDistance;
				transform.position = new Vector2 (Mathf.Lerp (origin.x + travelDistance, origin.x - travelDistance, transportTime * velocityMultiplier), transform.position.y);
				if (Mathf.Abs (transform.position.x - (origin.x - travelDistance)) < .03f) {
					platformState = PlatformState.GOINGPOS;
					transportTime = 0;
				}
			} else if (platformState == PlatformState.GOINGPOS) {
				transportTime += Time.deltaTime / travelDistance;
				transform.position = new Vector2 (Mathf.Lerp (origin.x - travelDistance, origin.x + travelDistance, transportTime * velocityMultiplier), transform.position.y);
				if (Mathf.Abs (transform.position.x - (origin.x + travelDistance)) < .03f) {
					platformState = PlatformState.GOINGNEG;
					transportTime = 0;
				}
			}
		} else {
			if (platformState == PlatformState.GOINGNEG) {
				transportTime += Time.deltaTime / travelDistance;
				transform.position = new Vector2 (transform.position.x, Mathf.Lerp(origin.y + travelDistance, origin.y - travelDistance, transportTime * velocityMultiplier));
				if (Mathf.Abs (transform.position.y - (origin.y - travelDistance)) < .03f) {
					platformState = PlatformState.GOINGPOS;
					transportTime = 0;
				}
			} else if (platformState == PlatformState.GOINGPOS) {
				transportTime += Time.deltaTime / travelDistance;
				transform.position = new Vector2 (transform.position.x, Mathf.Lerp(origin.y - travelDistance, origin.y + travelDistance, transportTime * velocityMultiplier));
				if (Mathf.Abs (transform.position.y - (origin.y + travelDistance)) < .03f) {
					platformState = PlatformState.GOINGNEG;
					transportTime = 0;
				}
			}
		}
	}
}
