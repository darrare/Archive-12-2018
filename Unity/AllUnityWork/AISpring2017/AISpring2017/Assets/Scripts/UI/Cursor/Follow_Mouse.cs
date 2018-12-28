using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Mouse : MonoBehaviour
{
    public GameObject followJoystick;
    public int moveSpeed = 5;
    Vector3 updatePosition;
    public float gridSideLength = .64f;

    private void Start()
    {
        
    }

    void Update()
    {
        if ((Input.GetJoystickNames().Length > 0) && (Input.GetJoystickNames()[0] != ""))
        {
            transform.position = followJoystick.transform.position;

            transform.localPosition = GetSnappedPosition(transform.localPosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        }
        else
        {
            updatePosition = Input.mousePosition;
            updatePosition = Camera.main.ScreenToWorldPoint(updatePosition);
            updatePosition = new Vector3(updatePosition.x, updatePosition.y, -3);
            transform.position = Vector2.Lerp(transform.position, updatePosition, moveSpeed);

            transform.localPosition = GetSnappedPosition(transform.localPosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        }
    }

    public Vector3 GetGridPosition()
    {
        return GetSnappedPosition(transform.position);
    }

    public Vector3 GetSnappedPosition(Vector3 position)
    {

        // not fatal in the Editor, but just better not to divide by 0 if we can avoid it
        if (gridSideLength == 0) { return position; }

        Vector3 gridPosition = new Vector3(
            gridSideLength * Mathf.Round(position.x / gridSideLength),
            gridSideLength * Mathf.Round(position.y / gridSideLength),
            -3//gridSideLength * Mathf.Round(position.z / gridSideLength)
        );
        return gridPosition;
    }
}
