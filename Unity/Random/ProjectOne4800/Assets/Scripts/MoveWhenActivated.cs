using UnityEngine;
using System.Collections;

public class MoveWhenActivated : MonoBehaviour
{
    Vector3 startposition;
    [SerializeField] Vector3 endPosition;
    Vector3 startRotation;
    [SerializeField] Vector3 endRotation;
    [SerializeField] float timeToFinish;
    float timer = 0;
    [SerializeField] BodyPart bodyPart;

    public bool IsStarted
    { get; set; }

    public bool IsReturning
    { get; set; }

    public float Timer
    { get { return timer; } }

	// Use this for initialization
	void Start ()
    {
        startposition = transform.localPosition;
        startRotation = transform.localEulerAngles;
        Manager.Instance.ThingsToMove.Add(bodyPart, this);
        IsReturning = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (IsStarted)
        {
            transform.localPosition = Vector3.Lerp(startposition, endPosition, timer);
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, timer);
            timer += Time.deltaTime / timeToFinish;
            if (timer >= timeToFinish)
            {
                IsStarted = false;
                IsReturning = false;
                timer = 0;
            }
        }
        else if (IsReturning)
        {
            transform.localPosition = Vector3.Lerp(endPosition, startposition, timer);
            transform.localEulerAngles = Vector3.Lerp(endRotation, startRotation, timer);
            timer += Time.deltaTime / timeToFinish;
            if (timer >= timeToFinish)
            {
                IsStarted = false;
                IsReturning = false;
                timer = 0;
            }
        }
	}
}
