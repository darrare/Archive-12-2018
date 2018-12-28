using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Vector3 playerPositionOffset = new Vector3(0, 4.6f, 0);

    //Tuning stuff
    float maxDistanceFromPlayer = 50f;
    float curDistanceFromPlayer = 25f;
    float wantedDistanceFromPlayer = 0f;
    float distanceToStopRenderingPlayer = 2f;
    float zoomRate = .2f;

    Vector3 directionFromPlayer;

    Vector2 curMousePosition;
    Vector2 startMousePosition = Vector2.zero;
    float mousePositionMultiplier = 3f;
    Quaternion rotation;
    float cameraOffsetFromWalls = 1f;
    float distanceOffsetToNotCorrectCamera = 3f;

    bool invertYAxis = true;

    // Use this for initialization
    void Start ()
    {
        directionFromPlayer = new Vector3(0, 1, -1);
        directionFromPlayer.Normalize();
        startMousePosition = Input.mousePosition;
        curMousePosition = Input.mousePosition;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Handle mouse controls
        curMousePosition = Input.mousePosition;
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            if (invertYAxis)
            {
                startMousePosition += new Vector2(Input.GetAxis("Mouse X") * mousePositionMultiplier, -Input.GetAxis("Mouse Y") * mousePositionMultiplier);
            }
            else
            {
                startMousePosition += new Vector2(Input.GetAxis("Mouse X") * mousePositionMultiplier, Input.GetAxis("Mouse Y") * mousePositionMultiplier);
            }
            
            rotation = Quaternion.Euler(startMousePosition.y, startMousePosition.x, 0);
            transform.position = (player.transform.position + playerPositionOffset) + rotation * new Vector3(0, 0, -curDistanceFromPlayer);
            transform.LookAt(player.transform.position + playerPositionOffset);
        }
        else if (Input.GetMouseButton(1))
        {
            if (invertYAxis)
            {
                startMousePosition += new Vector2(Input.GetAxis("Mouse X") * mousePositionMultiplier, -Input.GetAxis("Mouse Y") * mousePositionMultiplier);
            }
            else
            {
                startMousePosition += new Vector2(Input.GetAxis("Mouse X") * mousePositionMultiplier, Input.GetAxis("Mouse Y") * mousePositionMultiplier);
            }

            rotation = Quaternion.Euler(startMousePosition.y, startMousePosition.x, 0);
            transform.position = (player.transform.position + playerPositionOffset) + rotation * new Vector3(0, 0, -curDistanceFromPlayer);
            transform.LookAt(player.transform.position + playerPositionOffset);

            player.transform.LookAt(new Vector3(player.transform.position.x + player.transform.position.x - transform.position.x, player.transform.position.y, player.transform.position.z + player.transform.position.z - transform.position.z));
        }
        else
        {
            transform.position = (player.transform.position + playerPositionOffset) + rotation * new Vector3(0, 0, -curDistanceFromPlayer);
            transform.LookAt(player.transform.position + playerPositionOffset);
        }


        





        //Handle raycasting and zooming camera
        Ray camRay = new Ray(player.transform.position + playerPositionOffset, transform.position - (player.transform.position + playerPositionOffset) * (curDistanceFromPlayer / Vector3.Distance(transform.position, (player.transform.position + playerPositionOffset))));
        RaycastHit info;
        if (Physics.Raycast(camRay, out info, curDistanceFromPlayer))
        {
            //If the ray hit something, check to see if it is skinny enough to ignore.
            float t = (info.point.x - player.transform.position.x) / (transform.position - (player.transform.position + playerPositionOffset)).x;
            RaycastHit left, right, up, down, front, back;
            Physics.Raycast(player.transform.position + playerPositionOffset, transform.position - (player.transform.position + playerPositionOffset - Vector3.right * distanceOffsetToNotCorrectCamera * t), out left, curDistanceFromPlayer);
            Physics.Raycast(player.transform.position + playerPositionOffset, transform.position - (player.transform.position + playerPositionOffset + Vector3.right * distanceOffsetToNotCorrectCamera * t), out right, curDistanceFromPlayer);
            Physics.Raycast(player.transform.position + playerPositionOffset, transform.position - (player.transform.position + playerPositionOffset + Vector3.up * distanceOffsetToNotCorrectCamera * t), out up, curDistanceFromPlayer);
            Physics.Raycast(player.transform.position + playerPositionOffset, transform.position - (player.transform.position + playerPositionOffset - Vector3.up * distanceOffsetToNotCorrectCamera * t), out down, curDistanceFromPlayer);
            Physics.Raycast(player.transform.position + playerPositionOffset, transform.position - (player.transform.position + playerPositionOffset + Vector3.forward * distanceOffsetToNotCorrectCamera) * 1.1f, out front, curDistanceFromPlayer);
            Physics.Raycast(player.transform.position + playerPositionOffset, transform.position - (player.transform.position + playerPositionOffset - Vector3.forward * distanceOffsetToNotCorrectCamera) * 1.1f, out back, curDistanceFromPlayer);

            if (left.collider != info.collider || right.collider != info.collider || up.collider != info.collider || down.collider != info.collider || front.collider != info.collider || back.collider != info.collider)
            {

            }
            if (!left.collider || !right.collider || !up.collider || !down.collider || !front.collider || !back.collider)
            {
                //Handle mouse wheel zoom stuff
                wantedDistanceFromPlayer -= Input.GetAxis("Mouse ScrollWheel") * 20f;
                wantedDistanceFromPlayer = Mathf.Clamp(wantedDistanceFromPlayer, .1f, maxDistanceFromPlayer);
                curDistanceFromPlayer = Mathf.Lerp(curDistanceFromPlayer, wantedDistanceFromPlayer, .07f);
            }
            else
            {
                transform.position = (player.transform.position + playerPositionOffset) + rotation * new Vector3(0, 0, -curDistanceFromPlayer * t + cameraOffsetFromWalls);
                curDistanceFromPlayer = curDistanceFromPlayer * t + cameraOffsetFromWalls;
                transform.LookAt(player.transform.position + playerPositionOffset);
            }




            Debug.DrawLine(player.transform.position + playerPositionOffset, player.transform.position + playerPositionOffset + transform.position - (player.transform.position + playerPositionOffset), Color.green);
            Debug.DrawRay(player.transform.position + playerPositionOffset, (transform.position - (player.transform.position + playerPositionOffset - Vector3.right * distanceOffsetToNotCorrectCamera * t)) * 1.1f, Color.red);
            Debug.DrawRay(player.transform.position + playerPositionOffset, (transform.position - (player.transform.position + playerPositionOffset + Vector3.right * distanceOffsetToNotCorrectCamera * t)) * 1.1f, Color.blue);
            Debug.DrawRay(player.transform.position + playerPositionOffset, (transform.position - (player.transform.position + playerPositionOffset + Vector3.up * distanceOffsetToNotCorrectCamera * t)) * 1.1f, Color.cyan);
            Debug.DrawRay(player.transform.position + playerPositionOffset, (transform.position - (player.transform.position + playerPositionOffset - Vector3.up * distanceOffsetToNotCorrectCamera * t)) * 1.1f, Color.green);
            Debug.DrawRay(player.transform.position + playerPositionOffset, (transform.position - (player.transform.position + playerPositionOffset + Vector3.forward * distanceOffsetToNotCorrectCamera * t)) * 1.1f, Color.white);
            Debug.DrawRay(player.transform.position + playerPositionOffset, (transform.position - (player.transform.position + playerPositionOffset - Vector3.forward * distanceOffsetToNotCorrectCamera * t)) * 1.1f, Color.black);
        }
        else
        {
            //Handle mouse wheel zoom stuff
            wantedDistanceFromPlayer -= Input.GetAxis("Mouse ScrollWheel") * 20f;
            wantedDistanceFromPlayer = Mathf.Clamp(wantedDistanceFromPlayer, .1f, maxDistanceFromPlayer);
            curDistanceFromPlayer = Mathf.Lerp(curDistanceFromPlayer, wantedDistanceFromPlayer, .03f);
        }

        //handle rendering when close
        if (Vector3.Distance(transform.position, player.transform.position + playerPositionOffset) < distanceToStopRenderingPlayer)
        {
            GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
        }
        else
        {
            GetComponent<Camera>().cullingMask |= (1 << LayerMask.NameToLayer("Player"));
        }
    }
}
