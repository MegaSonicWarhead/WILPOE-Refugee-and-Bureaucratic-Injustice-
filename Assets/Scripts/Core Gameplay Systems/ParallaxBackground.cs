using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("How much this layer moves relative to camera movement (0 = stays still, 1 = moves with camera)")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxEffect = 0.5f;

    [Tooltip("If true, object will also move vertically with parallax")]
    [SerializeField] private bool useVerticalParallax = true;

    [Header("Infinite Scrolling (Optional)")]
    [Tooltip("Enable if this background should repeat infinitely")]
    [SerializeField] private bool infiniteHorizontal = false;
    [SerializeField] private bool infiniteVertical = false;

    private Camera mainCamera;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    private void Start()
    {
        mainCamera = Camera.main;
        lastCameraPosition = mainCamera.transform.position;

        // Get sprite size for infinite scrolling
        Sprite sprite = GetComponent<SpriteRenderer>()?.sprite;
        if (sprite != null)
        {
            Texture2D texture = sprite.texture;
            textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
            textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
        }
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = mainCamera.transform.position - lastCameraPosition;
        
        // Calculate parallax position
        float parallaxX = deltaMovement.x * parallaxEffect;
        float parallaxY = useVerticalParallax ? deltaMovement.y * parallaxEffect : 0f;

        // Apply movement
        transform.position += new Vector3(parallaxX, parallaxY, 0);

        // Handle infinite scrolling if enabled
        if (infiniteHorizontal || infiniteVertical)
        {
            Vector3 cameraPosition = mainCamera.transform.position;
            
            if (infiniteHorizontal)
            {
                float distanceX = cameraPosition.x * (1 - parallaxEffect);
                if (Mathf.Abs(cameraPosition.x - transform.position.x) >= textureUnitSizeX)
                {
                    float offsetPositionX = (cameraPosition.x - transform.position.x) % textureUnitSizeX;
                    transform.position = new Vector3(cameraPosition.x + offsetPositionX, transform.position.y, transform.position.z);
                }
            }

            if (infiniteVertical)
            {
                float distanceY = cameraPosition.y * (1 - parallaxEffect);
                if (Mathf.Abs(cameraPosition.y - transform.position.y) >= textureUnitSizeY)
                {
                    float offsetPositionY = (cameraPosition.y - transform.position.y) % textureUnitSizeY;
                    transform.position = new Vector3(transform.position.x, cameraPosition.y + offsetPositionY, transform.position.z);
                }
            }
        }

        lastCameraPosition = mainCamera.transform.position;
    }

    // Editor-only: Show visual indicator for parallax effect
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f); // Orange, semi-transparent
        Vector3 position = transform.position;
        Vector3 size = GetComponent<SpriteRenderer>()?.bounds.size ?? Vector3.one;
        Gizmos.DrawWireCube(position, size);

        // Draw arrow to indicate parallax direction
        Gizmos.color = new Color(1f, 1f, 0f, 0.8f); // Yellow, more opaque
        Vector3 arrowStart = position;
        Vector3 arrowEnd = position + Vector3.right * parallaxEffect;
        Gizmos.DrawLine(arrowStart, arrowEnd);
        Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(-0.2f, 0.2f, 0));
        Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(-0.2f, -0.2f, 0));
    }
} 