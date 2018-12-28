using System.Collections;
using AUTools.Unity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mediocracy
{
    /// <summary>Script that controls the camera.</summary>
    public class CameraScript : PauseObjScript
    {
        #region Fields

        [SerializeField] float zoomSpeed;
        [SerializeField] float scrollSpeed;
        Camera mainCamera;
        float movement;
        Vector3 baseMovement;
        bool zoomed;

        #endregion

        #region Properties



        #endregion

        #region Public Methods

        public void OnEnterUp(BaseEventData eventData)
        {
            movement = 1;
        }

        public void OnExitUp(BaseEventData eventData)
        {
            movement = 0;
        }

        public void OnEnterDown(BaseEventData eventData)
        {
            movement = -1;
        }

        public void OnExitDown(BaseEventData eventData)
        {
            movement = 0;
        }

        #endregion

        #region Protected Methods

        /// <summary>Updates the object. Only called if the object is not paused.</summary>
        protected override void NotPausedUpdate()
        {
            if (zoomed)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1) && !UIManager.Instance.IsMenuOpen(MenuType.RhythmGame))
                {
                    Vector3 newPosition = mainCamera.transform.position;
                    newPosition.x = 0;
                    if (newPosition.y < Constants.CAMERA_SIZE)
                    {
                        newPosition.y = Constants.CAMERA_SIZE;
                    }
                    else if (newPosition.y > Constants.MAX_CAMERA_HEIGHT)
                    {
                        newPosition.y = Constants.MAX_CAMERA_HEIGHT;
                    }
                    StartCoroutine(ZoomCoroutine(Constants.CAMERA_SIZE, newPosition));
                    zoomed = false;
                    GameManager.Instance.CurRoom.GetComponent<RoomStatusScript>().MuteAudioSources();
                    GameManager.Instance.CurRoom.GetComponent<Collider2D>().enabled = true;
                    GameManager.Instance.CurRoom = null;
                }
            }
            else
            {
                if (movement != 0)
                {
                    Vector3 newPosition = mainCamera.transform.position + (baseMovement * movement * Time.deltaTime);
                    if (newPosition.y < mainCamera.orthographicSize)
                    {
                        newPosition.y = mainCamera.orthographicSize;
                    }
                    else if (newPosition.y > Constants.MAX_CAMERA_HEIGHT)
                    {
                        newPosition.y = Constants.MAX_CAMERA_HEIGHT;
                    }
                    mainCamera.transform.position = newPosition;
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>Awake is called when the script instance is being loaded.</summary>
        void Awake()
        {
            Initialize();
            mainCamera = Camera.main;
            mainCamera.orthographicSize = Constants.CAMERA_SIZE;
            baseMovement = Vector3.up * scrollSpeed * mainCamera.orthographicSize;
            mainCamera.transform.position = new Vector3(0, mainCamera.orthographicSize, mainCamera.transform.position.z);
            GameManager.Instance.RoomClickedEvent.Add(room =>
            {
                if (zoomed)
                {
                    return;
                }
                Vector3 newPosition = room.transform.position;
                newPosition.y += Constants.BLOCK_HEIGHT;
                newPosition.z = mainCamera.transform.position.z;
                StartCoroutine(ZoomCoroutine(Constants.BLOCK_HEIGHT, newPosition));
                zoomed = true;
            });
        }

        IEnumerator ZoomCoroutine(float targetSize, Vector3 targetPosition)
        {
            float startSize = mainCamera.orthographicSize;
            Vector3 startPosition = mainCamera.transform.position;
            float time = 0;
            while (time < 1)
            {
                time += Time.deltaTime * zoomSpeed;
                mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, time);
                mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, time);
                yield return null;
            }
            mainCamera.orthographicSize = targetSize;
            mainCamera.transform.position = targetPosition;
        }

        #endregion
    }
}