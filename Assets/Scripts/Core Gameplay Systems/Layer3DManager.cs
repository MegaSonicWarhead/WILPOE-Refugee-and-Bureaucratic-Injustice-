using UnityEngine;

public class Layer3DManager : MonoBehaviour
{
    [System.Serializable]
    public class LayerData
    {
        public string name;
        public float zPosition;
        public float parallaxFactor;
        public Transform layerTransform;
    }

    [Header("Layer Configuration")]
    [SerializeField] private LayerData[] _layers = new LayerData[]
    {
        new LayerData { name = "Sky_Background", zPosition = 15f, parallaxFactor = 0.05f },
        new LayerData { name = "Distant_Environment", zPosition = 12f, parallaxFactor = 0.1f },
        new LayerData { name = "Far_Trees", zPosition = 10f, parallaxFactor = 0.2f },
        new LayerData { name = "Camp_Background", zPosition = 5f, parallaxFactor = 0.4f },
        new LayerData { name = "Main_Camp", zPosition = 0f, parallaxFactor = 0.7f },
        new LayerData { name = "Front_Camp", zPosition = -5f, parallaxFactor = 0.8f },
        new LayerData { name = "Foreground", zPosition = -10f, parallaxFactor = 0.9f }
    };

    [Header("References")]
    [SerializeField] private PerspectiveCamera _perspectiveCamera;

    private void Start()
    {
        if (_perspectiveCamera == null)
            _perspectiveCamera = Camera.main.GetComponent<PerspectiveCamera>();

        InitializeLayers();
    }

    private void InitializeLayers()
    {
        foreach (var layer in _layers)
        {
            if (layer.layerTransform != null)
            {
                // Set initial Z position
                Vector3 pos = layer.layerTransform.position;
                pos.z = layer.zPosition;
                layer.layerTransform.position = pos;

                // Add or get ParallaxBackground component
                var parallax = layer.layerTransform.GetComponent<ParallaxBackground>();
                if (parallax == null)
                    parallax = layer.layerTransform.gameObject.AddComponent<ParallaxBackground>();

                // Configure parallax
                parallax.SetParallaxEffect(layer.parallaxFactor);
            }
        }
    }

    public float GetLayerZ(string layerName)
    {
        foreach (var layer in _layers)
        {
            if (layer.name == layerName)
                return layer.zPosition;
        }
        return 0f;
    }

    public float GetLayerParallax(string layerName)
    {
        foreach (var layer in _layers)
        {
            if (layer.name == layerName)
                return layer.parallaxFactor;
        }
        return 1f;
    }
}