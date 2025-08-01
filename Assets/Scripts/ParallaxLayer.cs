using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor = 0.5f;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        lastCameraPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        Vector3 delta = Camera.main.transform.position - lastCameraPosition;
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);
        lastCameraPosition = Camera.main.transform.position;
    }
}
