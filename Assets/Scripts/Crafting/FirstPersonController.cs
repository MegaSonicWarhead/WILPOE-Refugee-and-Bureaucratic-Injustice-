using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public Camera playerCamera;
    public float moveSpeed = 4.5f;
    public float mouseSensitivity = 160f;
    public float gravity = -20f;
    public float jumpHeight = 1.2f;

    private CharacterController _cc;
    private float _pitch;
    private Vector3 _velocity;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Look
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mx);
        _pitch = Mathf.Clamp(_pitch - my, -85f, 85f);
        if (playerCamera != null)
            playerCamera.transform.localEulerAngles = new Vector3(_pitch, 0f, 0f);

        // Move
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * h + transform.forward * v) * moveSpeed;
        _cc.Move(move * Time.deltaTime);

        // Gravity / Jump
        if (_cc.isGrounded && _velocity.y < 0) _velocity.y = -2f;
        if (Input.GetButtonDown("Jump") && _cc.isGrounded)
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        _velocity.y += gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);

        // Unlock cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
