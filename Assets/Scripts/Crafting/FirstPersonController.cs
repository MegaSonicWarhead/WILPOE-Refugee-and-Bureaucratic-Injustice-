using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Movement Settings")]
    public float moveSpeed = 4.5f;
    public float mouseSensitivity = 160f;
    public float gravity = -20f;
    public float jumpHeight = 1.2f;

    private CharacterController _cc;
    private float _pitch;
    private Vector3 _velocity;

    // Cursor state
    private bool cursorLocked = true;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        LockCursor(true);
    }

    private void Update()
    {
        HandleCursorToggle();

        if (cursorLocked)
        {
            HandleLook();
            HandleMovement();
            HandleGravity();
        }
    }

    private void HandleCursorToggle()
    {
        // Press Escape to unlock cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockCursor(false);
        }

        // Press Left Click to relock cursor (only if not dragging items/UI)
        if (Input.GetMouseButtonDown(0) && !cursorLocked)
        {
            // Check if we're over UI — prevents snapping back while clicking buttons
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                LockCursor(true);
            }
        }
    }

    private void HandleLook()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mx);
        _pitch = Mathf.Clamp(_pitch - my, -85f, 85f);

        if (playerCamera != null)
            playerCamera.transform.localEulerAngles = new Vector3(_pitch, 0f, 0f);
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * h + transform.forward * v) * moveSpeed;
        _cc.Move(move * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (_cc.isGrounded && _velocity.y < 0) _velocity.y = -2f;
        if (Input.GetButtonDown("Jump") && _cc.isGrounded)
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        _velocity.y += gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);
    }

    private void LockCursor(bool shouldLock)
    {
        cursorLocked = shouldLock;
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}
