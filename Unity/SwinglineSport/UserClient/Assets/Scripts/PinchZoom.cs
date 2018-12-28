using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour {

    // vector for panning camera
    Vector3 newPos = new Vector3();

    // touch input variables
    Touch touchZero;
    Touch touchOne;

    // touch input vectors for movement direction
    Vector2 touchZeroPrevPos;
    Vector2 touchOnePrevPos;

    // speed of movement
    float prevTouchDeltaMag;
    float touchDeltaMag;

    // speeds for zoom in/out
    private float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    private float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

    void Update()
    {


        //if ( Input.GetKeyDown(KeyCode.LeftArrow) &&
        //    Camera.main.transform.position.x > -3)
        //    {
        //    newPos = new Vector3(/*touchZero.deltaPosition.x*/1, 0, 0);
        //    Camera.main.transform.position -= newPos;
        //    Debug.Log("Left Arrow Pressed");
        //}

        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            touchZero = Input.GetTouch(0);
            touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            //if (deltaMagnitudeDiff < .5f)
            //{
            if (touchZero.position.x < touchZeroPrevPos.x &&
                touchOne.position.x < touchOnePrevPos.x &&
                Camera.main.transform.position.x > -3)
            {
                newPos = new Vector3(touchZero.deltaPosition.x, 0, 0);
                Camera.main.transform.position -= newPos;
                Debug.Log("Left Arrow Pressed");
            }
            if (touchZero.position.x > touchZeroPrevPos.x &&
                touchOne.position.x > touchOnePrevPos.x &&
                Camera.main.transform.position.x > 3)
            {
                newPos = new Vector3(touchZero.deltaPosition.x, 0, 0);
                Camera.main.transform.position += newPos;
                Debug.Log("Right Arrow Pressed");
            }
            if (touchZero.position.y < touchZeroPrevPos.y &&
                touchOne.position.y < touchOnePrevPos.y &&
                Camera.main.transform.position.y > -3)
            {
                newPos = new Vector3(0, touchZero.deltaPosition.y, 0);
                Camera.main.transform.position -= newPos;
                Debug.Log("Down Arrow Pressed");
            }
            if (touchZero.position.y > touchZeroPrevPos.y &&
                touchOne.position.y > touchOnePrevPos.y &&
                Camera.main.transform.position.y < 3)
            {
                newPos = new Vector3(0, touchZero.deltaPosition.y, 0);
                Camera.main.transform.position += newPos;
                Debug.Log("Up Arrow Pressed");
            }
            //}

            // If the camera is orthographic...
            if (Camera.main.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 179.9f);
            }
        }
    }
}
