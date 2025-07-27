using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveCamera : MonoBehaviour
{
    [SerializeField] private float _perspectiveAngle = 15f;
    [SerializeField] private float _depthScale = 0.1f;

    private void SetupPerspective()
    {
        // Implement camera angle and positioning
        transform.rotation = Quaternion.Euler(_perspectiveAngle, 0, 0);
    }
}
