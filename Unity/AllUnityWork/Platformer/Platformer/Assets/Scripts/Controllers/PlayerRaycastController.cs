using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class PlayerRaycastController : MonoBehaviour
{
    #region fields

    [SerializeField]
    LayerMask collisionMask;

    [SerializeField]
    float maxSlopeAngle = 80f;

    const float skinWidth = .015f;
    const float distBetweenRays = .25f;
    int horizontalRayCount;
    int verticalRayCount;
    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D boxCollider;
    RaycastOrigins raycastOrigins;
    CollisionInfo collisions;

    Vector2 playerInput;

    #endregion

    #region properties

    /// <summary>
    /// detailed information about the current collisions
    /// </summary>
    public CollisionInfo Collisions
    {
        get { return collisions; }
    }

    #endregion

    #region public methods

    /// <summary>
    /// Moves the player a specific amount if allowed
    /// </summary>
    /// <param name="moveAmount">The amount to move</param>
    /// <param name="standingOnPlatform">Whether or not the character is standing on the platform</param>
    public void Move(Vector2 moveAmount, bool standingOnPlatform)
    {
        Move(moveAmount, Vector2.zero, standingOnPlatform);   
    }

    /// <summary>
    /// Moves the player a specific amount if allowed
    /// </summary>
    /// <param name="moveAmount">The amount to move</param>
    /// <param name="input">The players input on the character</param>
    /// <param name="standingOnPlatform">Whether or not the character is standing on the platform</param>
    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.MoveAmountOld = moveAmount;
        playerInput = input;

        //If the player is dropping down a slope
        if (moveAmount.y < 0)
        {
            DescendSlope(ref moveAmount);
        }

        //if the player is moving in the horizontal direction
        if (playerInput.x != 0)
        {
            collisions.FaceDir = (int)Mathf.Sign(moveAmount.x);
        }

        HorizontalCollisions(ref moveAmount);
        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);

        if (standingOnPlatform)
        {
            collisions.Below = true;
        }

    }

    #endregion

    #region private methods

    // Use this for initialization
    void Awake ()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();

        collisions.FaceDir = 1; //face to the right
	}

    /// <summary>
    /// Checks the horizontal rays to see if we are colliding with something
    /// </summary>
    /// <param name="moveAmount">The amount we want to move</param>
	void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = collisions.FaceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if (Mathf.Abs(moveAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.green);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (collisions.DescendingSlope)
                    {
                        collisions.DescendingSlope = false;
                        moveAmount = collisions.MoveAmountOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.SlopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.ClimbingSlope || slopeAngle > maxSlopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.ClimbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.Left = directionX == -1;
                    collisions.Right = directionX == 1;
                }
            }
        }
    }

    /// <summary>
    /// Check the vertical rays to see if we have any collisions
    /// </summary>
    /// <param name="moveAmount">The amount we want to move</param>
    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.BottomLeft : raycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                if (hit.collider.tag == "Through")
                {
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (collisions.FallingThroughPlatform)
                    {
                        continue;
                    }
                    if (playerInput.y == -1)
                    {
                        collisions.FallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .5f);
                        continue;
                    }
                }

                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.ClimbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisions.Below = directionY == -1;
                collisions.Above = directionY == 1;
            }
        }

        if (collisions.ClimbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.SlopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.SlopeAngle = slopeAngle;
                    collisions.SlopeNormal = hit.normal;
                }
            }
        }
    }

    /// <summary>
    /// Climb up a slope
    /// </summary>
    /// <param name="moveAmount">The amount we want to move</param>
    /// <param name="slopeAngle">The angle of the slope</param>
    /// <param name="slopeNormal">The normal of the slope</param>
    void ClimbSlope(ref Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal)
    {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= climbmoveAmountY)
        {
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            collisions.Below = true;
            collisions.ClimbingSlope = true;
            collisions.SlopeAngle = slopeAngle;
            collisions.SlopeNormal = slopeNormal;
        }
    }

    /// <summary>
    /// Walk down a slope
    /// </summary>
    /// <param name="moveAmount">The amount we want to move</param>
    void DescendSlope(ref Vector2 moveAmount)
    {

        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.BottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + skinWidth, collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.BottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + skinWidth, collisionMask);
        if (maxSlopeHitLeft ^ maxSlopeHitRight)
        {
            SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmount);
            SlideDownMaxSlope(maxSlopeHitRight, ref moveAmount);
        }

        if (!collisions.SlidingDownMaxSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomRight : raycastOrigins.BottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                        {
                            float moveDistance = Mathf.Abs(moveAmount.x);
                            float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                            moveAmount.y -= descendmoveAmountY;

                            collisions.SlopeAngle = slopeAngle;
                            collisions.DescendingSlope = true;
                            collisions.Below = true;
                            collisions.SlopeNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// We are on a slope that is too steep, we need to slide down it
    /// </summary>
    /// <param name="hit">Which raycast recorded the hit</param>
    /// <param name="moveAmount">The amount we want to move</param>
    void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount)
    {
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle > maxSlopeAngle)
            {
                moveAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                collisions.SlopeAngle = slopeAngle;
                collisions.SlidingDownMaxSlope = true;
                collisions.SlopeNormal = hit.normal;
            }
        }
    }

    /// <summary>
    /// Automatically calculate the spacing of the rays on each side of our collider
    /// </summary>
    void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / distBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / distBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    /// <summary>
    /// Updates the raycast origins to meet the box colliders bounds
    /// </summary>
    void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    #endregion

    #region custom structs

    /// <summary>
    /// Information about any collisions that are occuring
    /// </summary>
    public struct CollisionInfo
    {
        //Are there any collisions on these areas?
        public bool Above, Below, Left, Right;

        public bool ClimbingSlope, DescendingSlope, SlidingDownMaxSlope;
        public float SlopeAngle, SlopeAngleOld;

        public Vector2 SlopeNormal;
        public Vector2 MoveAmountOld;

        public int FaceDir;

        public bool FallingThroughPlatform;

        /// <summary>
        /// Resets everything to be false
        /// </summary>
        public void Reset()
        {
            Above = Below = Left = Right = false;
            ClimbingSlope = DescendingSlope = SlidingDownMaxSlope = false;
            SlopeNormal = Vector2.zero;
            SlopeAngleOld = SlopeAngle;
            SlopeAngle = 0;
        }

    }

    /// <summary>
    /// Origin locations for the raycasts
    /// </summary>
    public struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight, BottomLeft, BottomRight;
    }

    #endregion
}
