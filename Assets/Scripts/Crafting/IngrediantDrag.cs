using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngrediantDrag : MonoBehaviour
{
    private Camera cam;
    private bool isDragging = false;
    private float zCoord;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;
        zCoord = cam.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = zCoord;
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(worldPos.x, worldPos.y, worldPos.z);
        }
    }
}
