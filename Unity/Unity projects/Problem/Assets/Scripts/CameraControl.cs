//Followed tutorial by Sebastian Lague on youtube titled "[Unity] Creating a 2D Platformer (E11. camera follow)"
using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public GameObject player;
	public float verticalOffset;
	public float lookAheadDstX;
	public float lookSmoothTimeX;
	public float lookAheadDstY;
	public float lookSmoothTimeY;
	public float verticalSmoothTime;
	public Vector2 focusAreaSize;

	FocusArea focusArea;

	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirX;

	float currentLookY;
	float targetLookY;
	float lookAheadDirY;

	float smoothLookVelocityX;
	float smoothVelocityY;

	bool isPaused = false;

	void Start()
	{
		focusArea = new FocusArea (player.GetComponent<BoxCollider2D> ().bounds, focusAreaSize);
	}

	void LateUpdate()
	{
		if (!isPaused && Time.timeScale == 1) {
			focusArea.Update (player.GetComponent<BoxCollider2D> ().bounds);
			Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

			if (focusArea.velocity.x != 0) {
				lookAheadDirX = Mathf.Sign (focusArea.velocity.x);
			}

			lookAheadDirY = InputManager.GetAxisRaw ("CameraPan");

			targetLookAheadX = lookAheadDirX * lookAheadDstX;
			currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

			targetLookY = lookAheadDirY * lookAheadDstY;
			currentLookY = Mathf.SmoothDamp (currentLookY, targetLookY, ref smoothVelocityY, lookSmoothTimeY);

			focusPosition += Vector2.right * currentLookAheadX;
			focusPosition += Vector2.up * currentLookY;

			transform.position = (Vector3)focusPosition + Vector3.forward * -10;
		}
	}

	public void PauseCamera()
	{
		isPaused = true;
	}

	public void UnPauseCamera(GameObject player)
	{
		this.player = player;
		//focusArea = new FocusArea (player.GetComponent<BoxCollider2D> ().bounds, focusAreaSize);
		isPaused = false;
	}
		

//	void OnDrawGizmos()
//	{
//		Gizmos.color = new Color (1, 0, 0, .5f);
//		Gizmos.DrawCube (focusArea.center, focusAreaSize);
//	}

	struct FocusArea {
		public Vector2 center;
		public Vector2 velocity;
		float left, right;
		float top, bottom;

		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			center = new Vector2((left + right) / 2, (top + bottom) / 2);
		}

		public void Update (Bounds targetBounds) {
			float shiftX = 0;
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			float shiftY = 0;
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			bottom += shiftY;
			top += shiftY;
			center = new Vector2 ((left + right) / 2, (top + bottom) / 2);
			velocity = new Vector2 (shiftX, shiftY);
		}
	}
}
