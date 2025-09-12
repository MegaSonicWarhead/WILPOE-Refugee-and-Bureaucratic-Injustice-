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

    public float dragSpeed = 10f;
    private bool inBowl = false;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        if (inBowl) return; // prevent re-dragging after placed

        isDragging = true;
        zCoord = cam.WorldToScreenPoint(transform.position).z;
        rb.useGravity = false;
        rb.isKinematic = false;
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.useGravity = true;

        // If released in bowl, snap it and freeze
        if (inBowl)
        {
            rb.velocity = Vector3.zero;
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

            Vector3 targetPos = new Vector3(worldPos.x, worldPos.y, worldPos.z);
            Vector3 newPos = Vector3.Lerp(rb.position, targetPos, Time.fixedDeltaTime * dragSpeed);

            rb.MovePosition(newPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bowl"))
        {
            inBowl = true;

            // Snap down a bit inside bowl
            Vector3 pos = transform.position;
            pos.y = other.bounds.center.y; // adjust to bowl depth
            transform.position = pos;
        }
    }
}
