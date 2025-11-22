using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IngrediantDrag : MonoBehaviour
{
    private Camera cam;
    private Rigidbody rb;
    private bool isDragging = false;
    private float zCoord;
    private bool inBowl = false;

    [Header("Drag Settings")]
    public float dragSpeed = 15f;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void OnMouseDown()
    {
        if (inBowl) return; // can't pick up after placed

        isDragging = true;
        zCoord = cam.WorldToScreenPoint(transform.position).z;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.drag = 10f; // high drag while dragging
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (!inBowl)
        {
            // Restore physics naturally
            rb.useGravity = true;
            rb.drag = 0f;
        }
        else
        {
            // Lock position in bowl
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = zCoord;
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

            Vector3 smoothedPos = Vector3.Lerp(rb.position, worldPos, Time.fixedDeltaTime * dragSpeed);
            rb.MovePosition(smoothedPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bowl"))
        {
            inBowl = true;

            // Snap neatly into bowl
            Vector3 snapPos = other.bounds.center;
            snapPos.y = other.bounds.min.y + 0.05f;
            transform.position = snapPos;
        }
    }
}
