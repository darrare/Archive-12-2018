using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterGroundCheck : MonoBehaviour
{
    [SerializeField]
    CharacterController cController;
    [SerializeField] LayerMask mask;
    BoxCollider2D bCol;

    void Start()
    {
        bCol = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        cController.IsGrounded = bCol.IsTouchingLayers(mask);
    }
}
