using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleCarController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Vertical") * moveSpeed;
        float turn = Input.GetAxis("Horizontal") * turnSpeed;

        // Forward/backward
        rb.MovePosition(rb.position + transform.forward * move * Time.fixedDeltaTime);

        // Rotation
        rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * turn * Time.fixedDeltaTime));
    }
}
