using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraControllerScript : MonoBehaviour
{
    #region fields

    Vector3 positionSmoothRef;
    float sizeSmoothRef;
    Vector3 targetPos;
    float targetSize;
    Vector3 oldPosition;

    //camera shake stuff
    bool isShaking;
    float shakeTimer = 0;
    float shakeTimerGoal;
    float shakeIntensity;

    #endregion

    #region properties

    /// <summary>
    /// A list of CameraTargets used to choose where the camera should navigate to
    /// </summary>
    private List<CameraTarget> Targets
    { get; set; }

    /// <summary>
    /// The camera component attached to this gameobject
    /// </summary>
    public Camera Cam
    { get; set; }

    #endregion

    #region public methods

    /// <summary>
    /// Adds a new CameraTarget to the list of targets so the camera can choose where it needs to go
    /// </summary>
    /// <param name="t">The transform we want to focus in on</param>
    /// <param name="priority">The higher, the more pull the object has on the camera. Default = 1</param>
    public void AddTransformToTargets(Transform t, float priority = 1)
    {
        if (!Targets.Any(y => y.target == t))
        {
            Targets.Add(new CameraTarget(t, priority));
        }
    }

    /// <summary>
    /// Either the transform has been destroyed or it has moved too far away
    /// to where the camera shouldn't care about it any more.
    /// </summary>
    /// <param name="t">The transform to remove</param>
    public void RemoveTransformFromTargets(Transform t)
    {
        Targets.RemoveAll(y => y.target == t);
    }

    /// <summary>
    /// Applies a camera shake for effect
    /// </summary>
    /// <param name="intensity">The intensity of the camera shake (base radius is 1, so keep lower values)</param>
    /// <param name="duration">The duration of the camera shake (0 for 1 frame)</param>
    public void ApplyCameraShake(float intensity, float duration = 0)
    {
        //We ignore applying a camera shake if the current intensity is higher that what is input. (so explosion shakes don't get cancled out by gunshot shakes)
        if (shakeIntensity < intensity * Constants.CAMERA_SHAKE_MULTIPLIER)
        {
            isShaking = true;
            shakeTimer = 0;
            shakeTimerGoal = duration;
            shakeIntensity = intensity * Constants.CAMERA_SHAKE_MULTIPLIER;
        }
    }

    /// <summary>
    /// If for whatever reason we need to stop the camera shake before it finishes.
    /// </summary>
    public void StopCameraShake()
    {
        isShaking = false;
    }

    #endregion

    #region private methods

    // Use this for initialization
    void Awake()
    {
        //Initialize the cameras position to be directly on the player
        GameManager.Instance.Camera = this;
        Targets = new List<CameraTarget>();
        Cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Handle positioning the camera
        targetPos = GameManager.Instance.Player.transform.position + (GameManager.Instance.Player.Velocity * Constants.CAMERA_VELOCITY_WEIGHT_MULTIPLIER); //maybe add a modifier depending on which way the player is facing
        foreach (CameraTarget c in Targets)
        {
            targetPos += Vector3.LerpUnclamped(GameManager.Instance.Player.transform.position, c.target.position, c.priority);
        }
        targetPos /= Targets.Count + 1;
        targetPos -= Vector3.forward * 10;
        oldPosition = transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref positionSmoothRef, Constants.CAMERA_POSITION_SMOOTH_TIME);

        //Clean out the list of anything that shouldn't be in there anymore
        //for (int i = Targets.Count - 1; i >= 0; i--)
        //{
        //    if (Targets[i].target == null)
        //    {
        //        Targets.RemoveAt(i);
        //        continue;
        //    }
        //    if (Vector3.Distance(GameManager.Instance.Player.transform.position, Targets[i].target.position) > Constants.CAMERA_DISTANCE_TO_BE_REMOVED_FROM_TARGETS_LIST)
        //    {
        //        Targets.RemoveAt(i);
        //    }
        //}
        
        Cam.orthographicSize = Mathf.Clamp(Mathf.SmoothDamp(Cam.orthographicSize, Mathf.Lerp(Constants.CAMERA_ORTHO_SIZE_MIN, Constants.CAMERA_ORTHO_SIZE_MAX, ((Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position) / Constants.CAMERA_DISTANCE_MAX_FOR_ORTHO_CAM) + ((transform.position - oldPosition).magnitude / Constants.CAMERA_MAGNITUDE_MAX_FOR_ORTHO_CAM)) / 2), ref sizeSmoothRef, Constants.CAMERA_SIZE_SMOOTH_TIME), Constants.CAMERA_ORTHO_SIZE_MIN, Constants.CAMERA_ORTHO_SIZE_MAX);

        //Shake if needed
        if (isShaking)
        {
            shakeTimer += Time.deltaTime;
            transform.position += (Vector3)Random.insideUnitCircle * shakeIntensity;
            shakeIntensity *= Constants.CAMERA_SHAKE_DAMPENER_MULTIPLIER;
            if (shakeTimer >= shakeTimerGoal)
            {
                shakeIntensity = 0;
                isShaking = false;
            }
        }
    }

    #endregion
    
    /// <summary>
    /// Used to dictate where the camera wants to move
    /// </summary>
    struct CameraTarget
    {
        public Transform target;
        public float priority;

        /// <summary>
        /// Constructor for the CameraTarget
        /// </summary>
        /// <param name="t">The transform of the object</param>
        /// <param name="p">The priority of the object. MUST BE BETWEEN 0 AND 1</param>
        public CameraTarget(Transform t, float p)
        {
            target = t;
            priority = p;
        }
    }
}


