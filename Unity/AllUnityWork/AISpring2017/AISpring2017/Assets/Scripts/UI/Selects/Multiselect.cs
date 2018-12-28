using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiselect : MonoBehaviour
{
    public int moveSpeed = 5;
    Vector3 mousePosition;
    public float gridSideLength = .64f;

    void Start()
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, -3);
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);

        transform.localPosition = GetSnappedPosition(transform.localPosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
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
