using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform transform;
        [Range(0f, 1f)]
        public float parallaxEffect = 0.1f;
        public bool infiniteHorizontal = true;
        public float additionalScale = 1f;
    }

    public ParallaxLayer[] layers;
    public float smoothing = 1f;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float[] layerStartPosX;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
        layerStartPosX = new float[layers.Length];

        // Initialize starting positions
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].transform != null)
            {
                layerStartPosX[i] = layers[i].transform.position.x;
                
                // Apply additional scale for depth effect
                Vector3 currentScale = layers[i].transform.localScale;
                layers[i].transform.localScale = new Vector3(
                    currentScale.x * layers[i].additionalScale,
                    currentScale.y * layers[i].additionalScale,
                    currentScale.z
                );
            }
        }
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].transform == null) continue;

            float parallax = (previousCameraPosition.x - cameraTransform.position.x) * layers[i].parallaxEffect;
            float backgroundTargetPosX = layers[i].transform.position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, layers[i].transform.position.y, layers[i].transform.position.z);
            layers[i].transform.position = Vector3.Lerp(
                layers[i].transform.position,
                backgroundTargetPos,
                smoothing * Time.deltaTime
            );

            if (layers[i].infiniteHorizontal)
            {
                float cameraDeltaX = cameraTransform.position.x - previousCameraPosition.x;
                if (Mathf.Abs(cameraTransform.position.x - layers[i].transform.position.x) >= layers[i].transform.GetComponent<SpriteRenderer>().bounds.size.x)
                {
                    float offsetPositionX = (cameraTransform.position.x - layers[i].transform.position.x) % layers[i].transform.GetComponent<SpriteRenderer>().bounds.size.x;
                    layers[i].transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, layers[i].transform.position.y, layers[i].transform.position.z);
                }
            }
        }

        previousCameraPosition = cameraTransform.position;
    }
} 