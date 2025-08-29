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

    public float dragSpeed = 10f; // smooth movement

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        zCoord = cam.WorldToScreenPoint(transform.position).z;

        // Disable gravity while dragging so it doesn’t fall
        rb.useGravity = false;
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.useGravity = true; // turn gravity back on so it rests naturally
        rb.velocity = Vector3.zero; // stop weird momentum
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = zCoord;
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

            // Move smoothly using physics
            Vector3 targetPos = new Vector3(worldPos.x, worldPos.y, worldPos.z);
            Vector3 newPos = Vector3.Lerp(rb.position, targetPos, Time.fixedDeltaTime * dragSpeed);

            rb.MovePosition(newPos);
        }
    }
}
