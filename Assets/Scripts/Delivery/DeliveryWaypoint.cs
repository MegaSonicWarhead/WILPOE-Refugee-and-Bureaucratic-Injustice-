using UnityEngine;

public class DeliveryWaypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeliveryRouteManager manager = FindObjectOfType<DeliveryRouteManager>();
            if (manager != null)
            {
                manager.OnWaypointReached(gameObject);
            }
            else
            {
                Debug.LogError("No DeliveryRouteManager found in scene!");
            }
        }
    }
}
