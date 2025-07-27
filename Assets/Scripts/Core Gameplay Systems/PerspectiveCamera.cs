using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PerspectiveCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float tiltAngle = 10f;
    public float heightOffset = 5f;
    public float smoothSpeed = 5f;
    
    [Header("Depth Settings")]
    public float depthScale = 0.1f;
    public Transform target;
    
    private Camera cam;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        cam = GetComponent<Camera>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        
        // Apply initial tilt
        ApplyPerspectiveTilt();
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate target position with height offset
            Vector3 targetPosition = new Vector3(
                target.position.x,
                target.position.y + heightOffset,
                initialPosition.z
            );

            // Smoothly move camera
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                smoothSpeed * Time.deltaTime
            );
        }

        // Update perspective based on movement
        UpdatePerspective();
    }

    void ApplyPerspectiveTilt()
    {
        // Apply tilt while maintaining orthographic projection
        transform.rotation = Quaternion.Euler(tiltAngle, 0, 0);
    }

    void UpdatePerspective()
    {
        // Update sprite sorting based on Y position
        SpriteRenderer[] sprites = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            // Calculate sorting order based on Y position
            float yPos = sprite.transform.position.y;
            sprite.sortingOrder = Mathf.RoundToInt(-yPos * 100f);

            // Apply subtle scale variation based on Y position for depth effect
            float scaleModifier = 1f + (yPos * depthScale);
            sprite.transform.localScale = new Vector3(scaleModifier, scaleModifier, 1f);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ResetCamera()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        ApplyPerspectiveTilt();
    }
} 