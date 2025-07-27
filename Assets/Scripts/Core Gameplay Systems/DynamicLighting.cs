using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DynamicLighting : MonoBehaviour
{
    [Header("Light Settings")]
    public float intensityMin = 0.5f;
    public float intensityMax = 1.2f;
    public float flickerSpeed = 2f;
    
    [Header("Shadow Settings")]
    public float shadowDistanceMin = 0.8f;
    public float shadowDistanceMax = 1.2f;
    public float shadowFlickerSpeed = 1.5f;

    private Light2D light2D;
    private float initialIntensity;
    private float timeOffset;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        initialIntensity = light2D.intensity;
        timeOffset = Random.value * 100f; // Random offset for varied flickering
    }

    void Update()
    {
        // Smooth light intensity variation
        float intensityNoise = Mathf.PerlinNoise(Time.time * flickerSpeed, timeOffset);
        light2D.intensity = Mathf.Lerp(intensityMin, intensityMax, intensityNoise) * initialIntensity;

        // Dynamic shadow distance
        float shadowNoise = Mathf.PerlinNoise(Time.time * shadowFlickerSpeed, timeOffset + 50f);
        light2D.shadowIntensity = Mathf.Lerp(shadowDistanceMin, shadowDistanceMax, shadowNoise);
    }

    public void SetLightColor(Color color)
    {
        if (light2D != null)
        {
            light2D.color = color;
        }
    }

    public void SetIntensity(float intensity)
    {
        if (light2D != null)
        {
            initialIntensity = intensity;
        }
    }
} 