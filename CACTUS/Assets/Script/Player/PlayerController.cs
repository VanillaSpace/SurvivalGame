using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    public float lookSensitivity;
    private float camCurXRot;
    private Vector2 mouseDelta;


    // components
    private Rigidbody rig;
    
    void Awake()
    {
        // get our components
        rig = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Lock the cursor at the start of the game
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        // calculate the move direction relative to where we're facing.
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rig.velocity.y;

        // assign our Rigidbody velocity
        rig.velocity = dir;
    }

    void LateUpdate()
    {
        // We want to rotate the camera after the player has moved. 
        CameraLook();
    }

    void CameraLook()
    {
        // Rotate the camera container up and down
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        // Look up and down
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // Look left and right
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    // Called when we move our mouse - managed by the Input System
    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    // Called when we press WASD - managed by the Input System
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        // are we holding down a movement button?
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        // have we let go of a movement button?
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }
}
