using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    float maxVelocity = 3f;
    float sliderValue = .5f;
    [SerializeField]
    Slider slider;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.RotateAround(Vector3.zero, Vector3.down, (sliderValue - .5f) * maxVelocity);
	}

    public void ModifyValues()
    {
        sliderValue = slider.value;
    }
}
